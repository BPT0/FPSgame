using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField] // private 기능을 유지하면서 인스펙터창에 표시
    private float walkSpeed; // 걷기속도
    [SerializeField]
    private float runSpeed; // 뛰기 속도
    private float applySpeed; // 적용 속도
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 민감도
    [SerializeField]
    private float lookSensitivity; // 카메라 민감도

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit; // 카메라 회전각도 제한값
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;


    // 땅 착지 여부 구별하기 위해 콜라이더 가져옴
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();

        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CameraRotation();
        CharacterRotation();
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // 앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    // 부드러운 앉기 동작 
    IEnumerator CrouchCoroutine()
    {
        // 코루틴을 이용하여 부드럽게 카메라 이동시킴
        // 코루틴이란 함수를 반복해서 호출하여 해당 동작을 자연스럽게 처리할 때 사용
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        // 보간함수 시작점에서는 빨리 증가하다가 중간점에서는 천천히 증가
        while(_posY != applyCrouchPosY)
        {
            // 마지막 값이 클수록 더 빨리 증가
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null; // 1프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // 지반 체크
    private void IsGround()
    {
        // 1.지면으로 콜라이더의 반지름만큼 y값 만큼 레이저 쏘기 +0.1f 여유를 준다
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
        theCrosshair.RunningAnimation(!isGround);
    }

    // 점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // 점프 동작
    private void Jump()
    {
        // 앉은상태 점프시 앉기 해제
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    // 달리기 실행
    private void Running()
    {
        // 달리기 실행시 앉기 취소
        if (isCrouch)
            Crouch();
        theGunController.CancelFineSight();
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.CrouchingAnimation(isRun);
        applySpeed = walkSpeed;
    }

    // 움직임 실행
    private void Move()
    {
        // 마우스 입력값 좌 1, 우 -1, 안누를시 0 에 따라 값 대입 
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // x는 좌우
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // z는 상하

        // transform 객체가 가지고 있는 값
        // 마우스 입력값에 따라 좌우 상하 위치값 설정
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // nomalized : 
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        // Time.deltaTime : 1초동안 일정거리를 이동한다. 자연스럽게 이동시킴
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        // 뛰고 있지 않고 웅크리지 않고 있으면
        if (!isRun && !isCrouch && isGround)
        {
            // 전프레임이 현재 플레이어 위치가 0.01f 보다 크면
            if (Vector3.Distance(lastPos, transform.position)>=0.01f)
                isWalk = true;
            else
                isWalk = false;
            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        // 마우스 위아래 드래그시 조정값
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        // clamp 함수로 사이의 값만을 사용
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        /*Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);*/
    }

    // 좌우 케릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }


}
