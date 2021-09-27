using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderSC : LivingEntity
{
    public LayerMask target;
    private GameObject targetEnitity;
    public GameObject mainDoor;
    public GameObject player;

    private float traceRange = 15f;
    private float attackDistance = 5f;
    private float damage = 12f;

    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttack = false;
    private bool isAttacking = false;

    public enum eCharacterState
    {
        Trace,
        Attack,
        Die
    }

    List<GameObject> list = new List<GameObject>();

    private eCharacterState state;


    Coroutine co_updatePath;

    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        SetUp();
    }
    /// <summary>
    /// 초기 스텟 설정
    /// </summary>
    /// <param name="newHP"></param>
    /// <param name="newSpeed"></param>
    /// <param name="newDamage"></param>
    public void SetUp(float newHP = 20f, float newSpeed = 3f, float newDamage = 12f)
    {
        maxHp = newHP;
        currHP = newHP;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }
    /// <summary>
    /// 애니매이션 Duration 값 얻기
    /// </summary>
    /// <param name="moveType"></param>
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
    void Start()
    {
        co_updatePath = StartCoroutine(UpdatePath());
    }


    void Update()
    {

    }

    /// <summary>
    /// 추적대상을 찾아서 경로를 갱신
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
                pathFinder.SetDestination(targetEnitity.transform.position);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
