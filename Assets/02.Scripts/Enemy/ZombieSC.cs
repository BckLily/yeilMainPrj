using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieSC : LivingEntity
{
    public LayerMask Target;           // 추적 대상 레이어
    private LivingEntity targetEntity; // 추적 대상의 LivingEntity
    private NavMeshAgent pathFinder;   // 경로 계산 에이전트
    private Animator enemyAnimator;    // 애니매이션
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime;      // 마지막 공격 시점

    private bool hasTarget           // 추적대상의 존재 여부 판단
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()    //초기화
    {
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup();
    }
    public void Setup(float newHP = 100f, float newSpeed = 5f)
    {
        StartHP = newHP;
        CurrHP = newHP;
        //damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        //플레이어가 일정거리안에 잇으면 타겟을 플레이어로
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    // 추적 대상을 찾아서 경로를 갱신
    IEnumerator UpdatePath()
    {
        yield return new WaitForSeconds(0.3f);
        while (!dead)
        {
            //추적 대상이 존재한다
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                // 20의 반지름을 가진 구를 그리고 구에 겹치는 모든 콜라이더를 가져온다
                // 콜라이더는 Target 콜라이더만 필터링 함
                Collider[] colliders = null;
                try
                {
                    colliders = Physics.OverlapSphere(transform.position, 50f, 1 << LayerMask.NameToLayer("PLAYER")); // 단, Target 레이어를 가진 콜라이더만  필터링
                }
                catch(Exception e)
                {
                    Debug.LogWarning(e);
                    yield break;
                }

                for (int i = 0; i < colliders.Length; i++) // 콜라이더를 순회하면서 살아있는 livingEntity를 찾는다
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();// 콜라이더에서부터 livingEntity 컴포넌트 가져오기

                    if (livingEntity != null && !livingEntity.dead) // livingEntity 컴포넌트가 존재하며, 해당 livingEntity 가 사망하지 않았다면
                    {

                        targetEntity = livingEntity;

                        break; // for문 정지
                    }
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ChangeTarget()
    {
        while (!dead)
        {

            yield return new WaitForSeconds(0.3f);
        }
    }
}
