using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public WeaponManager weaponManager;

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

    }

    // Update is called once per frame
    void Update()
    {
        ExitGame();



    }


    private void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;


#elif UNITY_STANDALONE_WIN
            Application.Quit();


#endif


        }
    }






}
