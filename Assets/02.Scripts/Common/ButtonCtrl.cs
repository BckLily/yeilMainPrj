using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCtrl : MonoBehaviour
{
    public void OnGameReadyButtonClick()
    {
        //    GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        //    _lobby.SetActive(true);
        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(false);

        // 플레이어 닉네임 입력하는 창
        // 나중에는 아이디 비밀번호를 입력하고
        // 닉네임을 받아와서 닉네임을 자동 입력하는 방식으로 처리.
        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(true);

    }

    public void OnGameStartButtonClick()
    {

        PlayerPrefs.SetString("Player_NickName", transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("PlayerNameText").GetComponent<UnityEngine.UI.Text>().text);
        PlayerPrefs.SetString("Player_Class", transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("ClassDropdown").Find("Label").GetComponent<UnityEngine.UI.Text>().text);
        // 로딩

#if UNITY_EDITOR
        Debug.Log("____ Game Start Button Clicked ____");
        //Debug.Log($"Get Player Prefs {PlayerPrefs.GetString("Player_NickName")}");
        //Debug.Log($"Get Player Prefs {PlayerPrefs.GetString("Player_Class")}");
#endif

        GameManager.instance.SceneLoadingFunction("MapScene");
    }

    // GameManager에 씬 로드에 사용할 함수를 만들어서 사용하고 있다.
    //IEnumerator GameSceneLoad()
    //{
    //    SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    //    AsyncOperation _operation = SceneManager.LoadSceneAsync("MapScene");
    //    _operation.allowSceneActivation = false;

    //    UnityEngine.UI.Slider _slider = GameObject.Find("LoadingSlider").GetComponent<UnityEngine.UI.Slider>();

    //    Debug.Log("___ Loading... ___");
    //    while (_operation.progress <= 0.9f)
    //    {
    //        _slider.value = _operation.progress;
    //        yield return null;
    //    }
    //    Debug.Log("____ Loading almost complete ____ ");

    //    _slider.value = 0.9f;

    //    //Game Manager Game Start
    //    GameManager.instance.GameStart(_operation,
    //        PlayerPrefs.GetString("Player_NickName"),
    //        PlayerPrefs.GetString("Player_Class"));
    //    // 이전에 쓰던 방식이 있어서 함수를 유지시켰지만
    //    // GameStart 함수 내에서 PlayerPrefs를 통해서 받아도 된다.

    //    int count = 0;
    //    while (count < 10)
    //    {
    //        _slider.value += 1 / 100f;
    //        count++;
    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    _operation.allowSceneActivation = true;
    //}


    public void OnGameExitButtonClick()
    {
        GameManager.instance.ExitGame();
    }


    public void OnLobbyExitButtonClick()
    {
        GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        _lobby.SetActive(false);

        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(true);
    }


    public void OnLogInSelectButtonClick()
    {
        string _playerNickName = transform.Find("LoginPanel").Find("NickNameInputField").Find("Text").GetComponent<UnityEngine.UI.Text>().text;

        if (_playerNickName == "" || _playerNickName == null || _playerNickName.Substring(0, 1) == " ")
        {
            transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.SetActive(true);
            return;
        }


        if (transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.activeSelf)
        {
            transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.SetActive(false);
        }

        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(false);

        GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        _lobby.SetActive(true);

        // 플레이어가 여러명이면 PlayerInfoPanel의 번호가 달라진다.
        transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("PlayerNameText").GetComponent<UnityEngine.UI.Text>().text = _playerNickName;
    }

    public void OnLogInExitButtonClick()
    {
        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(false);

        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(true);
    }



    public void OnContinueGameButtonClick()
    {
        PlayerCtrl playerCtrl = transform.parent.parent.GetComponent<PlayerCtrl>();
        playerCtrl.MenuOpen();
    }


    public void OnMainMenuButtonClick()
    {
        //StartCoroutine(MainMenuSceneLoad());
        GameManager.instance.SceneLoadingFunction("MainMenuScene");
    }

    public void OnKeyInformationOpenButtonClick()
    {
        transform.Find("KeyInfoPanel").gameObject.SetActive(true);
    }

    public void OnKeyInformationExitButtonClick()
    {
        transform.Find("KeyInfoPanel").gameObject.SetActive(false);
    }

    public void OnReadmeOpenButtonClick()
    {
        // 현재 폴더의 위치 확인.
        System.IO.DirectoryInfo currDir = new System.IO.DirectoryInfo(Application.dataPath);
        if (currDir.Parent != null) { currDir = currDir.Parent; }

#if UNITY_EDITOR
        Debug.Log($"____Path: {currDir.FullName}____");
#elif UNITY_STANDALONE_WIN
        Debug.LogWarning($"____Path: {currDir.FullName}____");
#endif
        System.Diagnostics.Process.Start(currDir + "/readme.txt");
    }

}
