using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieSC : LivingEntity
{
    public LayerMask target;           // 추적 대상 레이어
    private GameObject targetEntity;   // 추적 대상
    public GameObject mainDoor;
    public GameObject player;
    public GameObject attackColl;      // 공격 판정 콜라이더
    float traceRange = 10f;            // 추적 반경
    float attackDistance = 1.5f;       // 공격 거리
    float timeBetAttack = 0.5f;        // 공격 간격
    float lastAttackTime;              // 마지막 공격 시점

    private NavMeshAgent pathFinder;   // 경로 계산 에이전트
    private Animator enemyAnimator;    // 애니매이션

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttack = false;
    bool isAttacking = false;

    Coroutine co_updatePath;
    Coroutine co_chageTarget;

    public enum eCharacterState
    {
        Trace,
        Attack,
        Die
    }

    List<GameObject> list = new List<GameObject>();

    private eCharacterState state;

    private void Awake()    //초기화
    {
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup();
    }

    public void Setup(float newHP = 20f, float newSpeed = 3f, float newDamage = 10f)
    {
        startHp = newHP;
        currHP = newHP;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    /// <summary>
    /// 애니매이션 Duration 값 얻기
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public float MoveDuration(eCharacterState moveType)
    {
        string name = string.Empty;
        switch (moveType)
        {
            case eCharacterState.Trace:
                name = "zombieRunning";
                break;
            case eCharacterState.Attack:
                name = "Zombie Attack";
                break;
            default:
                return 0;
        }

        float time = 0;

        RuntimeAnimatorController ac = enemyAnimator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name)
            {
                time = ac.animationClips[i].length;
            }

        }
        return time;
    }

    private void Start()
    {
        co_updatePath = StartCoroutine(UpdatePath());
        co_chageTarget = StartCoroutine(ChangeTarget());
    }

    private void Update()
    {
        //플레이어가 사거리에 들어오면 공격
        if (dead)
            return;

        if (state == eCharacterState.Trace && Vector3.Distance(targetEntity.transform.position, transform.position) <= attackDistance && !isAttacking)
        {
            NowAttack();
        }

        if (isAttacking == true)
        {
            Quaternion lookRot = Quaternion.LookRotation(targetEntity.transform.position - this.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 60f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 추적 대상을 찾아서 경로를 갱신
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdatePath()
    {
        yield return new WaitForSeconds(0.3f);
        while (!dead)
        {
            if (pathFinder.enabled)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 추적 대상을 변경
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeTarget()
    {
        while (!dead)
        {
            // 오버랩 스피어로 범위내에 있는 PLAYER 레이어 콜라이더 추출
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, traceRange, 1 << LayerMask.NameToLayer("PLAYER"));

            if (colliders.Length >= 1)
                targetEntity = colliders[0].gameObject;
            else
                targetEntity = mainDoor;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            //Debug.Log("Player Contact");
            if (!list.Contains(other.gameObject))
            {
                list.Add(other.gameObject);
                isTrace = false;

                Vector3 hitPoint = other.ClosestPoint(gameObject.GetComponent<Collider>().bounds.center);
               
                Vector3 hitNormal = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z).normalized;

                ZombieSC zombie = other.GetComponent<ZombieSC>();
                other.GetComponent<LivingEntity>().Damaged(damage, hitPoint, hitNormal); ;

                //Debug.Log("HIT");
            }
            else
                return;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        isAttack = false;
        isTrace = true;
        enemyAnimator.SetBool("IsAttack", isAttack);
        enemyAnimator.SetBool("IsTrace", isTrace);
    }

    /// <summary>
    /// 추적 함수
    /// </summary>
    void NowTrace()
    {
        //Debug.Log("____TRACE____");
        state = eCharacterState.Trace;
        if (pathFinder.enabled)
        {
            pathFinder.isStopped = false;
            pathFinder.speed = 3f;
            isTrace = true;
            enemyAnimator.SetBool("IsTrace", isTrace);
        }
    }

    /// <summary>
    /// 공격 함수
    /// </summary>
    private void NowAttack()
    {
        isAttacking = true;

        //Debug.Log("START ATTACK");
        state = eCharacterState.Attack;

        pathFinder.isStopped = true;
        pathFinder.speed = 0f;
        enemyAnimator.SetTrigger("IsAttack");
        float collidertime = 0.8f;
        StartCoroutine(ColliderON(collidertime));
        collidertime = 1.5f;
        StartCoroutine(ColliderOff(collidertime));
        float attackdelayTime = MoveDuration(eCharacterState.Attack);
        StartCoroutine(EndAttacking(attackdelayTime));
        // StartCoroutine(ClearList());
    }

    //IEnumerator ClearList()
    //{
    //    float delaytime = MoveDuration(eCharacterState.Attack);
    //    yield return new WaitForSeconds(delaytime);

    //    list.Clear();
    //}

    public void ClearList()
    {
        list.Clear();
    }

    IEnumerator ColliderON(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        pathFinder.enabled = false;
        attackColl.SetActive(true);
    }

    IEnumerator ColliderOff(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        attackColl.SetActive(false);
        ClearList();
    }

    IEnumerator EndAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        isAttacking = false;
        pathFinder.enabled = true;
        NowTrace();
    }

    protected override void Down()
    {
        base.Down();
        Die();
        //pathFinder.enabled = false;
    }



}