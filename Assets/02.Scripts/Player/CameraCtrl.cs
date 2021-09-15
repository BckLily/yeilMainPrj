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

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 pos = new Vector3();
        //Vector3 value = tr.forward * (-0.035f) + tr.up * 0.075f + tr.right * (-0.035f);
        //pos = value + neckTr.position;

        //tr.position = Vector3.Lerp(tr.position, pos, 4f * Time.deltaTime);
        ////tr.up = transform.up;

    }

    

    private void LateUpdate()
    {
        Vector3 pos = new Vector3();
        Vector3 value = tr.forward * forward + tr.up * up + tr.right * right;
        pos = value + neckTr.position;

        tr.position = Vector3.Lerp(tr.position, pos, 4f * Time.deltaTime);
        //tr.up = transform.up;

    }
}
