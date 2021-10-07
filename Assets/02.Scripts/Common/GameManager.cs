using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public WeaponManager weaponManager;

    public BunkerDoor bunkerDoor;


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




}
