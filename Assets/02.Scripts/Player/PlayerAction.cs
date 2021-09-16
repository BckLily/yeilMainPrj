using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Transform tr; // 플레이어의 Transform

    private bool isBuild; // 플레이어가 빌딩을 건설하고 있는가?
    private bool isBuy; // 플레이어가 물건을 사고 있는가? 상점을 이용하고 있는가?
    private bool isHeal; // 플레이어가 회복 동작을 수행하고 있는가?

    private Animator playerAnim; // 플레이어의 애니메이터


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        isBuild = false;
        isBuy = false;
        isHeal = false;

        //Cursor.visible = false; // 커서 보이지 않게 한다.
        // Cursor.visible = true; // 커서를 보이게 한다.
        //Cursor.lockState = CursorLockMode.Locked; // 커서를 고정한다.
        // Cursor.lockState = CorsorLockMode.None; // 커서 고정을 끈다
        // Cursor.lockState = CorsorLockMode.Confine; // 커서를 경계를 벗어나지 못하게 한다.
    }

    // Update is called once per frame
    void Update()
    {
        CheckRaycast();

    }

    // 플레이어가 보고 있는 위치게 어떤 물건이 있는지 확인하는 함수
    private void CheckRaycast()
    {

    }





}
