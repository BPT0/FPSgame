using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미완성 코루틴을 포함하므로 미완성 클래스로 선언
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중??
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
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
    }
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true; // 중복실행 방지
        currentCloseWeapon.anim.SetTrigger("Attack");

        // 공격이 적중할때 까지 대기 시간 설정
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        // 공격을 하고있을때 코루틴 반복해서 실행
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        // 기본 대기시간에서 위에서 실행한 대기시간 2개를 빼줌
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }

    // 미완성 = 추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    // 가상함수: 완성 함수이지만, 추가 편집가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
