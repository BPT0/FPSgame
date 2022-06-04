using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 근접 무기 이름

    // 웨폰 유형
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    // public string handName; // 너클이나 맨손을 구분
    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점
    public float attackDelayB; // 공격 비활성화 시점

    // 공격이 닿는 범위는 1인칭 시점이기에 십자처럼 과녁정해서
    // 마크처럼 처리
    public Animator anim; // 애니메이션
}
