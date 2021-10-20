using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //public WeaponManager weaponManager;

    public BunkerDoor bunkerDoor;

    [Header("Player 특전")]
    // 각 특전이 활성화 되었는지 확인하기 위한 변수
    public bool perk0_Active = false;
    public bool perk1_Active = false;
    public bool perk2_Active = false;

    [Header("Stage")]
    private int maxStage = 30;
    public int _stage; // 현재 몇 스테이지인지를 저장한 변수
    public int _remainEnemyCount; // 스테이지에 남은 적 수


    [Header("Players")]
    // 멀티가 된다면 사용하게될? 플레이어들 목록
    // Photon에서는 PhotonNetwork.CurrentRoom.Players 을 사용해서 플레이어 목록을 확인할 수 있다.
    // 특전이 활성화되면 특전 활성화에 사용해야한다.
    public List<PlayerCtrl> players;

    // 적을 생성할 위치
    Transform[] enemyPoints;

    UnityEngine.UI.Text stageText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _stage = 1;

        /*
        // 지금은 게임 씬에서 시작하니까 바로 GameStart를 실행시킨다.
        GameStart();
        */
    }

    // Update is called once per frame
    void Update()
    {

        // 유니티 에디터에서 동작
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ExitGame();
        }

        // 윈도우에서 동작
#elif UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ExitGame();
        }


#endif


    }

    public void ExitGame()
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
        PlayerPrefs.Save();
        // 대충 화면이 어두워지고 Game Over 라거나 패배 라는 글자가 뜨고
        // 잠시후에 로비로 돌아가는 버튼이 활성화되거나 하면 된다.

        GameObject gameCanvas = GameObject.Find("GameWorldCanvas");
        gameCanvas.transform.Find("GameOverPanel").gameObject.SetActive(true);

        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        GameObject gameCanvas = GameObject.Find("GameWorldCanvas");
        UnityEngine.UI.Image fadein = gameCanvas.transform.Find("GameOverPanel").Find("GameOverFadeIn").GetComponent<UnityEngine.UI.Image>();

        while (fadein.color.a < 1f)
        {
            Color _color = fadein.color;
            _color.a += Time.deltaTime;
            fadein.color = _color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        try
        {
            gameCanvas.GetComponent<ButtonCtrl>().OnMainMenuButtonClick();
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e);
#endif
        }
        finally
        {
            GameFail();
        }

    }


    /// <summary>
    /// 게임 씬으로 넘어갔을 때 실행될 함수
    /// </summary>
    public void GameStart(UnityEngine.AsyncOperation _operation, string _playerNickName, string _className)
    {
        string _class;
        if (_className == "소총병")
        {
            _class = "Soldier";
        }
        else if (_className == "의무병")
        {
            _class = "Medic";
        }
        else if (_className == "공병")
        {
            _class = "Engineer";
        }
        else
        {
            _class = null;
        }

        StartCoroutine(CoGameStart(_operation, _playerNickName, _class));

    }

    // 나중에 플레이어 정보가 DB에 있게되면 DB에서 플레이어 정보를 받아올 수 있을까?
    // 직업 설정은 어떻게 넘겨줄까?
    IEnumerator CoGameStart(UnityEngine.AsyncOperation _operation, string _playerNickName, string _className)
    {
        yield return _operation;

        // 벙커의 위치를 받아온다.
        bunkerDoor = GameObject.FindGameObjectWithTag("BUNKERDOOR").GetComponent<BunkerDoor>();

        // 플레이어 스폰 지점 목록을 가져온다.
        Transform[] points = GameObject.Find("PlayerSpawnPoints").GetComponentsInChildren<Transform>();

        // 부모의 위치도 포함되기 때문에 부모인 0을 제외한 1부터 시작한다.
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 생성할 플레이어 프리팹을 찾음
        GameObject _playerPref = Resources.Load<GameObject>("Prefabs/Player/Player");
        // 생성할 위치를 설정
        players.Add(Instantiate(_playerPref, points[idx].position, Quaternion.identity).GetComponent<PlayerCtrl>());

        Debug.Log($"Players Count: {players.Count}");

        // 플레이어의 이름 및 직업 설정
        // Lobyy에서 입력받은 플레이어 이름과 선택된 직업을 사용해서 설정해야한다.
        players[0].playerName = _playerNickName;

        //players[0].playerClass = PlayerClass.ePlayerClass.Medic;
        players[0].playerClass = (PlayerClass.ePlayerClass)System.Enum.Parse(typeof(PlayerClass.ePlayerClass), _className);

        enemyPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
#if UNITY_EDITOR
        Debug.Log("EnemyPoints: " + enemyPoints.Length);
#endif

        stageText = GameObject.Find("StageText").GetComponent<UnityEngine.UI.Text>();
        stageText.text = _stage.ToString();

        ObjectCounting.zombieSpwanCount = 10;
        ObjectCounting.spiderSpwanCount = 0;
        ObjectCounting.clutchSpwanCount = 0;
        ObjectCounting.MovidicSpwanCount = 0;


        yield return new WaitForSeconds(1f);

    }


    internal void GameFail()
    {
        // 테스트 중에는 오류가 발생할 수 있지만
        // 일반적으로 게임 중에는 항상 적은 스폰되고 있을 것이기 때문에 _coEnemeySpawn이 null일 확률은 없다.
        // 테스트 코드

        Debug.Log("Game Fail");

        _stage = 1;

        perk0_Active = false;
        perk1_Active = false;
        perk2_Active = false;

        players.Clear();
        bunkerDoor = null;
        SpwanManager.Instance.enemies.Clear();

        CursorState.CursorLockedSetting(false);

    }

    /// <summary>
    /// 스테이지 클리어시 플레이어에게 포인트와 경험치를 주는 함수
    /// </summary>
    public void StageClear()
    {
        if (_stage > maxStage)
        {
            GameClear();
            // 클리어를 하면 게임 클리어라는 화면이 나오고 버튼을 누르면 메인 메뉴로 돌아온다.
            return;
            // 게임 클리어를 했으면 다음 스테이지를 진행할 필요가 없다.
        }
        StartCoroutine(StageClearCo());
    }

    /// <summary>
    /// 게임 클리어시 실행될 함수.
    /// </summary>
    private void GameClear()
    {
        //SceneLoadingFunction("MainMenuScene");
        CursorState.CursorLockedSetting(false);
        GameObject.Find("GameWorldCanvas").transform.Find("GameClearPanel").gameObject.SetActive(true);
    }

    IEnumerator StageClearCo()
    {
        foreach (var player in players)
        {
            Debug.Log("Stage Clear Coroutine");
            PlayerCtrl _playerCtrl = player.GetComponent<PlayerCtrl>();
            _playerCtrl._point += 200;
            _playerCtrl._playerExp += 50f;
            _playerCtrl.CheckLevelUp();
            yield return null;
        }
    }


    #region 씬 로딩 함수
    /// <summary>
    /// 다른 씬으로 넘어갈 때 로딩씬을 사용할 경우 사용하는 함수.
    /// </summary>
    /// <param name="_sceneName">Scene Name</param>
    public void SceneLoadingFunction(string _sceneName)
    {
        StartCoroutine(SceneLoadingCoroutine(_sceneName));
    }


    private IEnumerator SceneLoadingCoroutine(string _sceneName)
    {
        AsyncOperation _loadingOperation = SceneManager.LoadSceneAsync("LoadingScene");

        while (_loadingOperation.progress < 0.9f) { yield return null; }
        yield return new WaitForSeconds(0.5f);
        UnityEngine.UI.Slider _slider = GameObject.Find("LoadingSlider").GetComponent<UnityEngine.UI.Slider>();

        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneName);
        _operation.allowSceneActivation = false;

#if UNITY_EDITOR
        Debug.Log("___ Loading... ___");
#endif
        while (_operation.progress < 0.9f)
        {
            _slider.value = (float)_operation.progress;

            yield return null;
        }
#if UNITY_EDITOR
        Debug.Log("____ Loading almost complete ____ ");
#endif
        _slider.value = 0.9f;


        if (_sceneName == "MapScene")
        {
            //Game Manager Game Start
            GameManager.instance.GameStart(_operation,
                PlayerPrefs.GetString("Player_NickName"),
                PlayerPrefs.GetString("Player_Class"));
            // 이전에 쓰던 방식이 있어서 함수를 유지시켰지만
            // GameStart 함수 내에서 PlayerPrefs를 통해서 받아도 된다.
        }
        else if (_sceneName == "MainMenuScene")
        {
            GameFail();
        }

        int count = 0;
        while (count < 10)
        {
            _slider.value += 1 / 100f;
            count++;
            yield return new WaitForSeconds(0.5f);
        }

        _operation.allowSceneActivation = true;

    }

    #endregion


}
