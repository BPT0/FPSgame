using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    // 공격중??
    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;

    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        // GetButton 누르고 있을때 동작
        if (Input.GetButton("Fire1"))
        {
            // 좌클릭 총알 발사
            // left ctrl 은 앉기 동작이므로 유니티 기본 동작 Fire1에서 삭제처리
            if (!isAttack)
            {
                // 코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true; // 중복실행 방지
        currentHand.anim.SetTrigger("Attack");

        // 공격이 적중할때 까지 대기 시간 설정
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        // 공격을 하고있을때 코루틴 반복해서 실행
        StartCoroutine(HitCoroutine());
        
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        // 기본 대기시간에서 위에서 실행한 대기시간 2개를 빼줌
        yield return new WaitForSeconds(currentHand.attackDelay- currentHand.attackDelayA- currentHand.attackDelayB);


        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false; // 한번 적중했을때 실행 중단
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
