using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovidicSC : LivingEntity
{
    public LayerMask target;

    private GameObject targetEntity;
    public GameObject maindoor;
    public GameObject attackColl;

    float traceRange = 10f;
    float attackDistance = 4f;

    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttack = false;
    private bool isAttacking = false;

    Coroutine co_updatePath;
    Coroutine co_changeTarget;

    private eCharacterState state;

    List<GameObject> list = new List<GameObject>();

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup();
    }

    public void Setup(float newHp = 300f, float newAP = 5f, float newSpeed = 2f, float newDamage = 20f)
    {
        startHp = newHp;
        currHP = newHp;
        armour = newAP;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    public float MoveDuration(eCharacterState moveType)
    {
        string name = string.Empty;
        switch (moveType)
        {
            case eCharacterState.Trace:
                name = "Run";
                break;
            case eCharacterState.Attack:
                name = "AttackBite";
                break;
            case eCharacterState.Die:
                name = "DeathHit";
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
    void Start()
    {
        NowTrace();
        co_updatePath = StartCoroutine(UpdatePath());
        co_changeTarget = StartCoroutine(ChangeTarget());
    }

    void Update()
    {
        if (dead)
            return;

        if (state == eCharacterState.Trace && Vector3.Distance(targetEntity.transform.position, this.transform.position) <= attackDistance && !isAttacking)
        {
            NowAttack();
        }

        if (isAttacking == true)
        {
            Quaternion LookRot = Quaternion.LookRotation(targetEntity.transform.position - this.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookRot, 60f * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            //Debug.Log("=== Player Contact ===");
            if (!list.Contains(other.gameObject))
            {
                list.Add(other.gameObject);
                isTrace = false;

                Vector3 hitPoint = other.ClosestPoint(gameObject.GetComponent<Collider>().bounds.center);
                Vector3 hitnormal = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z).normalized;

                MovidicSC modivic = other.GetComponent<MovidicSC>();
                other.GetComponent<LivingEntity>().Damaged(damage, hitPoint, hitnormal);

                //Debug.Log("=== HIT ===");
            }
            else
                return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isAttack = false;
        isTrace = true;

        enemyAnimator.SetBool("IsTrace", isTrace);
    }
    /// <summary>
    /// 추적 함수
    /// </summary>
    void NowTrace()
    {
        state = eCharacterState.Trace;
        if (pathFinder.enabled)
        {
            pathFinder.isStopped = false;
            pathFinder.speed = 3f;
            isTrace = true;
            enemyAnimator.SetBool("IsTrace", isTrace);
        }
    }

    void NowAttack()
    {
        isAttacking = true;

        state = eCharacterState.Attack;

        pathFinder.enabled = true;
        pathFinder.speed = 0f;
        enemyAnimator.SetTrigger("IsAttack");
        float attackTime = 0.5f;
        StartCoroutine(StartAttacking(attackTime));
        attackTime = 0.8f;
        StartCoroutine(NowAttacking(attackTime));
        float attackdelayTime = MoveDuration(eCharacterState.Attack);
        StartCoroutine(EndAttacking(attackdelayTime));

        Debug.Log(MoveDuration(eCharacterState.Attack));
    }

    public void ClearList()
    {
        list.Clear();
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
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, traceRange, 1 << LayerMask.NameToLayer("PLAYER"));

            if (colliders.Length >= 1)
                targetEntity = colliders[0].gameObject;
            else
                targetEntity = maindoor;

            yield return new WaitForSeconds(0.1f);
        }
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

    void ColliderOn()
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
        Die();
    }
}
