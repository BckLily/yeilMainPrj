using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    // 실제 구조물
    public GameObject structure;
    // 청사진
    public GameObject bluePrint;
    private Collider collider;

    public bool isBuild;

    void Start()
    {
        collider = GetComponent<Collider>();
        isBuild = false;
    }

    void Update()
    {
        // Unity 에디터에서만 동작하는 코드
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BuildingBuild();
        }
#endif
    }

    public void BuildingBuild()
    {
        structure.SetActive(true);
        bluePrint.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isBuild = true;
        collider.enabled = false;
    }

    public void BuildingDestroy()
    {
        bluePrint.SetActive(true);
        structure.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        isBuild = false;
        collider.enabled = true;
    }


}
