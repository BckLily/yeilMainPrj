using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanManager : MonoBehaviour
{
    private static SpwanManager instance = null;
    public static SpwanManager Instance { get { return instance; } }

    [SerializeField]
    List<Transform> enemyPoints = new List<Transform>();

    // 현재 소환된 수 카운트 용
    public List<LivingEntity> enemies = new List<LivingEntity>();
    public int totalCount;
    private float waitNextStageTime = 45f;

    private IEnumerator _coEnemySpawn;


    #region UI 영역
    [SerializeField]
    UnityEngine.UI.Text remainEnemiesText;
    [SerializeField]
    UnityEngine.UI.Text stageText;
    [SerializeField]
    UnityEngine.UI.Text nextStageTimeText;

    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        enemyPoints.AddRange(GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>());
        Debug.Log("EnemyPoints: " + enemyPoints.Count);

        StageTextSetting();

        StartCoroutine(WaitNextStage());
    }

    private IEnumerator WaitNextStage()
    {
        // 다음 스테이지로 넘어갈 때만 다음 스테이지까지 몇 분이 남았는지 표시해주면 된다.
        nextStageTimeText.gameObject.SetActive(true);
        //Debug.Log("___ NEXT Stage ____");
        float _time = waitNextStageTime;

        while (_time > 0)
        {
            nextStageTimeText.text = string.Format($"{_time:N1}");
            yield return new WaitForFixedUpdate();
            _time -= Time.fixedDeltaTime;

        }
        _coEnemySpawn = EnemySpawn();
        nextStageTimeText.gameObject.SetActive(false);
        // 다음 스테이지가 시작되고 그 스테이지의 적들이 생성되면 된다.
        StartCoroutine(_coEnemySpawn);

    }


    public void EnemySpawnDebug()
    {
        if (_coEnemySpawn != null)
        {
            StopCoroutine(_coEnemySpawn);
            _coEnemySpawn = null;
        }
        else
        {
            _coEnemySpawn = EnemySpawn();
            StartCoroutine(_coEnemySpawn);
        }
    }

    IEnumerator EnemySpawn()
    {
        /*
         * 0
         * 1/ 2 3 4 5 6 7
         * 8/ 9 10 11 12 13 14
         * 15/ 16 17 18 19 20 21
         */

        // 어떤걸 몇 마리 소환하는지 받아옴
        List<int> enemyCount = ObjectCounting.SpwanCounting(GameManager.instance._stage);
        int zombieCount = enemyCount[0];
        int spiderCount = enemyCount[1];
        int clutchCount = enemyCount[2];
        int movidicCount = enemyCount[3];

        totalCount = zombieCount + spiderCount + clutchCount + movidicCount;
        float spawnTime = 0.5f;

        yield return new WaitForSeconds(3f);
        while (true)
        {

            int idx = UnityEngine.Random.Range(2, 8);
            idx += (UnityEngine.Random.Range(0, 3) * 7);

            //Debug.Log("____ Total Count: " + totalCount + " ____");

            if (enemies.Count < 15 && enemies.Count < totalCount)
            {
                int selectEnemy = UnityEngine.Random.Range(0, 4);

                Monster mob = (Monster)selectEnemy;

                //var obj = ObjectPooling.GetObject(Monster.Zombie);
                //obj.transform.position = enemyPoints[idx].position;
                //obj.SetActive(true);

                // 소환하려고한 대상이 남은 소환수 >> 0
                // spawnTime = 0.01
                // 소환 했으면 spawnTime 0.5

                if (enemyCount[selectEnemy] >= 1)
                {
                    GameObject obj;
                    switch (mob)
                    {
                        case Monster.Zombie:
                            obj = ObjectPooling.GetObject(Monster.Zombie);
                            obj.transform.position = enemyPoints[idx].position;
                            obj.SetActive(true);

                            break;
                        case Monster.Spider:
                            obj = ObjectPooling.GetObject(Monster.Spider);
                            obj.transform.position = enemyPoints[idx].position;
                            obj.SetActive(true);

                            break;
                        case Monster.Clutch:
                            obj = ObjectPooling.GetObject(Monster.Clutch);
                            obj.transform.position = enemyPoints[idx].position;
                            obj.SetActive(true);
                            break;
                        case Monster.Movidic:
                            obj = ObjectPooling.GetObject(Monster.Movidic);
                            obj.transform.position = enemyPoints[idx].position;
                            obj.SetActive(true);
                            break;
                        default:
                            obj = null;
                            break;
                    }

                    //Debug.Log("Reduce Spawn Count");

                    // 소환한 것 count 감소
                    enemyCount[selectEnemy]--;

                    //Debug.Log(enemyPoints[idx].position);
                    //Debug.Log(obj.transform.position);
                    if (obj != null)
                    {
                        enemies.Add(obj.GetComponent<LivingEntity>());
                    }
                }

            }

            //Debug.Log("____ ENEMY COUNT: " + enemies.Count + " ____");

            yield return new WaitForSeconds(spawnTime);

            // int형
            remainEnemiesText.text = totalCount.ToString();

            if (totalCount <= 0)
            {
                // 스테이지 클리어했다는 변수 하나 해가지고
                // 클리어 하면 다음 스테이지까지 여유시간 좀 주고
                // 여유시간 끝나면 바로 다음 스테이지 시작(코루틴 돌리고)
                GameManager.instance._stage++;
                StageTextSetting();
                break;
            }
        }
        StartCoroutine(WaitNextStage());
        _coEnemySpawn = null;

    }


    private void StageTextSetting()
    {
        // int 형
        stageText.text = string.Format($"<size=65>Stage</size>\n{GameManager.instance._stage.ToString()}");

        // 특전 획득에 성공했는지 확인하는 함수
        AchievementManager.CheckAchievement_Perk();
    }


}
