using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public WeaponManager weaponManager;

    public BunkerDoor bunkerDoor;

    // 각 특전이 활성화 되었는지 확인하기 위한 변수
    public bool perk0_Active = false;
    public bool perk1_Active = false;
    public bool perk2_Active = false;

    public int _stage; // 현재 몇 스테이지인지를 저장한 변수

    // 멀티가 된다면 사용하게될? 플레이어들 목록
    // Photon에서는 PhotonNetwork.CurrentRoom.Players 을 사용해서 플레이어 목록을 확인할 수 있다.
    // 특전이 활성화되면 특전 활성화에 사용해야한다.
    public List<PlayerCtrl> players;

    // 적을 생성할 위치
    Transform[] enemyPoints;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        bunkerDoor = GameObject.FindGameObjectWithTag("BUNKERDOOR").GetComponent<BunkerDoor>();

        // 지금은 게임 씬에서 시작하니까 바로 GameStart를 실행시킨다.
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {

        // 유니티 에디터에서 동작
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ExitGame();
        }
        // 윈도우에서 동작
#elif UNITY_STANDALONE_WIN

#endif



        #region Get Skill Information Test Code
        //List<float> skillInfo = FindSkill.GetSkillInfo("Healing", 2);
        //for(int i = 0; i < skillInfo.Count; i++)
        //{
        //    Debug.Log(skillInfo[i]);
        //}
        #endregion

    }

    private void ExitGame()
    {

        // 유니티 에디터에서 동작
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        // 윈도우에서 동작
#elif UNITY_STANDALONE_WIN
        Application.Quit();

#endif


    }

    // 게임이 종료되면 실행되는 함수.
    public void GameOver()
    {
        // 대충 화면이 어두워지고 Game Over 라거나 패배 라는 글자가 뜨고
        // 잠시후에 로비로 돌아가는 버튼이 활성화되거나 하면 된다.
    }


    /// <summary>
    /// 게임 씬으로 넘어갔을 때 실행될 함수
    /// </summary>
    private void GameStart()
    {
        // 플레이어 스폰 지점 목록을 가져온다.
        Transform[] points = GameObject.Find("PlayerSpawnPoints").GetComponentsInChildren<Transform>();

        // 부모의 위치도 포함되기 때문에 부모인 0을 제외한 1부터 시작한다.
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 생성할 플레이어 프리팹을 찾음
        GameObject _playerPref = Resources.Load<GameObject>("Prefabs/Player/Player");
        // 생성할 위치를 설정
        players.Add(Instantiate(_playerPref, points[idx].position, Quaternion.identity).GetComponent<PlayerCtrl>());
        // 플레이어의 이름 및 직업 설정
        // Lobyy에서 입력받은 플레이어 이름과 선택된 직업을 사용해서 설정해야한다.
        players[0].playerName = "Player1";
        players[0].playerClass = PlayerClass.ePlayerClass.Engineer;


        enemyPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
        Debug.Log("EnemyPoints: " + enemyPoints.Length);

        StartCoroutine(EnemySpawn());

    }


    private void GameFail()
    {

        perk0_Active = false;
        perk1_Active = false;
        perk2_Active = false;

        players.Clear();


    }


    public List<LivingEntity> enemies = new List<LivingEntity>();

    IEnumerator EnemySpawn()
    {
        /*
         * 0
         * 1 2 3 4 5 6 7
         * 8 9 10 11 12 13 14
         * 15 16 17 18 19 20 21
         */


        yield return new WaitForSeconds(3f);
        GameObject zombie = Resources.Load<GameObject>("Prefabs/Enemy/Zombie");
        while (true)
        {
            int idx = UnityEngine.Random.Range(2, 7);
            if (enemies.Count < 15)
            {
                enemies.Add(Instantiate(zombie, enemyPoints[idx].position, Quaternion.identity).GetComponent<LivingEntity>());
            }

            //Debug.Log("____ ENEMY COUNT: " + enemies.Count + " ____");

            yield return new WaitForSeconds(0.2f);
        }




    }


}
