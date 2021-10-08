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

    private NavMeshAgent pathFinder;   // 경로 계산 에이전트
    private Animator enemyAnimator;    // 애니매이션

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttack = false;
    bool isAttacking = false;

    Coroutine co_updatePath;
    Coroutine co_chageTarget;


    List<GameObject> list = new List<GameObject>();

    private void Awake()    //초기화
    {
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup();

        exp = 1f;

    }

    // 임시로 적어둠
    protected override void OnEnable()
    {
        base.OnEnable();

        mainDoor = GameManager.instance.bunkerDoor.gameObject;
    }

    public void Setup(float newHP = 20f, float newAP = 0f, float newSpeed = 3f, float newDamage = 10f)
    {
        startHp = newHP;
        currHP = newHP;
        armour = newAP;
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
            case eCharacterState.Die:
                name = "Zombie Dying";
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

        if (state == eCharacterState.Trace && Vector3.Distance(new Vector3(targetEntity.transform.position.x, 0, targetEntity.transform.position.z), new Vector3(this.transform.position.x, 0, this.transform.position.z)) <= attackDistance && !isAttacking)
        {
            NowAttack();
        }

        if (isAttacking == true)
        {
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(targetEntity.transform.position.x, 0, targetEntity.transform.position.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z));
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
            Debug.Log("Player Contact");
            if (!list.Contains(other.gameObject))
            {
                list.Add(other.gameObject);
                isTrace = false;

                Vector3 hitPoint = other.ClosestPoint(gameObject.GetComponent<Collider>().bounds.center);

                Vector3 hitNormal = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z).normalized;

                ZombieSC zombie = other.GetComponent<ZombieSC>();
                other.GetComponent<LivingEntity>().Damaged(damage, hitPoint, hitNormal);

                Debug.Log("HIT");
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

        pathFinder.isStopped = true;
        pathFinder.speed = 0f;
        enemyAnimator.SetTrigger("IsAttack");
        float collidertime = 0.999f;
        StartCoroutine(StartAttacking(collidertime));
        collidertime = 1.166f;
        StartCoroutine(NowAttacking(collidertime));
        float attackdelayTime = MoveDuration(eCharacterState.Attack);
        StartCoroutine(EndAttacking(attackdelayTime));
        // StartCoroutine(ClearList());

        Debug.Log(MoveDuration(eCharacterState.Attack));
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

    IEnumerator StartAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        pathFinder.enabled = false;
    }

    IEnumerator NowAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        ClearList();
    }

    IEnumerator EndAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime * 0.8f);
        isAttacking = false;
        pathFinder.enabled = true;
        NowTrace();
    }

    void ColliderON()
    {
        attackColl.SetActive(true);
    }

    void ColliderOFF()
    {
        attackColl.SetActive(false);
    }

    protected override void Down()
    {
        base.Down();
        pathFinder.enabled = false;
        enemyAnimator.SetTrigger("IsDead");
        Debug.Log(MoveDuration(eCharacterState.Die));

        // 임시 작성
        if (GameManager.instance.enemies.Contains(this))
        {
            GameManager.instance.enemies.Remove(this);
        }

        Die();
    }



}