using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Hp_Bar_Script : MonoBehaviour
{
    Camera cam;//카메라 변수
    Canvas can;//캔버스 변수
    RectTransform rectParent;//부모의 rectTransform 변수
    RectTransform rect;//자신의 rectTransform 변수

    public Vector3 offset = Vector3.zero;//Hp바 위치 조절
    public Transform wall;//방어물자 위치

    bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        can = GetComponentInParent<Canvas>();
        //can.worldCamera = Camera.main;
        //can.sortingOrder = 15;
        //can.planeDistance = 0.011f;
        cam = can.worldCamera;
        //Debug.Log("Cam: " + cam);
        rectParent = can.GetComponent<RectTransform>();
        rect = this.gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        isFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(wall.position + offset);

        //Debug.Log($"ScreenPos: {screenPos}");

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, cam, out localPos);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, null, out localPos);

        //Debug.Log($"LocalPosition: {localPos}");

        if (isFirst)
        {
            isFirst = false;
            rect.localPosition = new Vector3(10000f, 10000f, 10000f);
        }

        //if (localPos.x >= 0 && localPos.y >= 0)
        if (screenPos.z > 0)
        {
            rect.localPosition = localPos;
        }

    }
}
