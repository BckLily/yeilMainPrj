using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Transform tr; // 플레이어의 Transform

    ///<summary>
    /// CameraRaycast Script
    ///</summary>
    private CameraRaycast cameraRaycast; // 카메라 Transform

    private bool isBuild; // 플레이어가 빌딩을 건설하고 있는가?
    private bool isBuy; // 플레이어가 물건을 사고 있는가? 상점을 이용하고 있는가?
    private bool isHeal; // 플레이어가 회복 동작을 수행하고 있는가?

    private Animator playerAnim; // 플레이어의 애니메이터

    private float searchTime = 0f;
    private float searchDelay = 0.05f;
    private GameObject target = null; // 보고 있는 타겟
    string targetTag = null; // 보고 있는 타겟의 태그



    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        cameraRaycast = GetComponent<CameraRaycast>();

        isBuild = false;
        isBuy = false;
        isHeal = false;

        Cursor.lockState = CursorLockMode.Locked; // 커서를 고정한다.
        // Cursor.lockState = CorsorLockMode.None; // 커서 고정을 끈다
        // Cursor.lockState = CorsorLockMode.Confine; // 커서를 경계를 벗어나지 못하게 한다.
        Cursor.visible = true; // 커서를 보이게 한다.
        //Cursor.visible = false; // 커서 보이지 않게 한다.



    }

    // Update is called once per frame
    void Update()
    {
        CheckLooking();
        Action();
    }


    /// <summary>
    /// 플레이어가 보고 있는 위치게 어떤 물건이 있는지 확인하는 함수
    /// </summary>
    private void CheckLooking()
    {
        searchTime += Time.deltaTime;
        // 0.05초마다 보고 있는 대상 확인
        if (searchTime >=searchDelay)
        {
            searchTime -= searchDelay;
            target = (GameObject)cameraRaycast.GetRaycastTarget(10f);

            try { targetTag = target.tag; }
            catch (NullReferenceException e)
            {
                //Debug.Log(e);
                return;
            }
        }
    }

    /// <summary>
    /// target의 Tag에 따라 어떤 동작을 할지 결정하는 함수
    /// </summary>
    private void Action()
    {
        // 방어 물자가 건설되어 있지 않은 상태이면 건설할 수 있다고 표시해주어야 한다.
        if (targetTag == "DEFENSIVEGOODS")
        {
            Debug.Log("Defensive Goods State");
        }
        // 상점에 다가가면 상점이라고 표시가 뜨고 키를 누르면 상점이 열린다.
        else if (targetTag == "STORE")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Store Open");
            }
        }
        // 플레이어에게 다가가면 플레이어의 이름이 표시된다.
        else if (targetTag == "PLAYER")
        {
            Debug.Log("Player Live State");
        }
    }




}
