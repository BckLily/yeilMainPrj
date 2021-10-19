using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    private Transform tr; // 카메라의 위치
    public Transform neckTr; // 카메라가 기준으로 잡을 목 위치

    public float forward;
    public float up;
    public float right;


    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        offset = new Vector3(0.1f, -0.06f, 0.01f);

    }


    private void LateUpdate()
    {
        // 목의 위치를 기준으로 Off Set 값 만큼 항상 상대적으로 위쪽에 있게 설정.
        tr.position = neckTr.position - (neckTr.up * offset.y + neckTr.right * offset.x + neckTr.forward * offset.z);

    }


}
