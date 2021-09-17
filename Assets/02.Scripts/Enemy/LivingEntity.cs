using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceSet;
//using System;
public class LivingEntity : MonoBehaviour, IAttack, IDamaged
{
    public float StartHP = 10f; // 시작 체력
    public float CurrHP;
    public bool dead;
    //public event Action OnDeath;
    protected virtual void OnEnable()  // 클래스가 생성될때 리셋되는 상태
    {
        dead = false;  // 사망상태가 아님
        CurrHP = StartHP; // 현재 체력은 시작 체력이랑 같음
    }
    public void Attack()  // 공격시 실행될 함수
    {
        
    }

    public void Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal) // 피헤 받을시 실행될 함수 (데미지, 피격 위치, 피격 방향)
    {
        CurrHP -= damage; // 현재체력에 데미지 만큼 감소
        if( CurrHP <= 0 && !dead) // 현재체력이 0보다 작고 사망 상태가 아닐떄
        {
            Die(); // DIE 함수 실행
        }
    }

    public void Die() // 사망함수
    {
        if(dead != true) // 사망 상태가 아닐때
        {
            OnDeath(); // 사망 애니메이션 함수 실행
        }

        dead = true;  // 상태를 사망으로
    }
 
    public void OnDeath()
    {
        // 사망 애니매이션 실행
        // 일정 시간 뒤 오브젝트 삭제
        Destroy(gameObject, 1f);
    }
}
