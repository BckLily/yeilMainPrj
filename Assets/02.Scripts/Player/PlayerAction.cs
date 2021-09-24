using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Target Information")]
    /// <summary>
    /// Player가 보고 있는 타겟의 정보를 표시해줄 Text
    /// </summary>
    public Text targetInfoText;
    /// <summary>
    /// Player가 보고 있는 타겟의 정보를 표시할 Panel
    /// </summary>
    public GameObject targetInfoPanel;

    [Space(5, order = 0)]
    // Order >> Serialize중 출력 순서?
    [Header("Aim Point", order = 1)]
    public GameObject crosshair; // 플레이어 공격의 조준점
    public GameObject gaugeRing; // 특정 동작시 0에서 1까지 Gauge가 찰 Ring


    #region LayerMask
    // 타겟으로 사용할 Layer
    LayerMask allLayerMask;

    #endregion


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

        LayerMask enemyLayer = LayerMask.NameToLayer("ENEMY");
        LayerMask defensiveGoodsLayer = LayerMask.NameToLayer("DEFENSIVEGOODS");
        LayerMask storeLayer = LayerMask.NameToLayer("STORE");
        LayerMask playerLayer = LayerMask.NameToLayer("PLAYER");
        LayerMask mainDoorLayer = LayerMask.NameToLayer("MAINDOOR");
        LayerMask wallLayer = LayerMask.NameToLayer("WALL");

        allLayerMask = (1 << enemyLayer) | (1 << defensiveGoodsLayer) | (1 << storeLayer) | (1 << playerLayer) | (1 << mainDoorLayer) | (1 << wallLayer);

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
        if (searchTime >= searchDelay)
        {
            searchTime -= searchDelay;
            target = (GameObject)cameraRaycast.GetRaycastTarget(10f, allLayerMask);

            try
            {

                targetTag = target.tag;
            }
            catch (NullReferenceException e)
            {
                //Debug.Log(e);
                targetTag = null;
                targetInfoPanel.SetActive(false);
                return;
            }
        }
    }

    /// <summary>
    /// target의 Tag에 따라 어떤 동작을 할지 결정하는 함수
    /// </summary>
    private void Action()
    {
        // 보고 있는 대상이 없는 경우 함수 바로 종료
        if (target == null)
        {
            GaugeClear();
            return;
        }

        //Debug.Log(target);

        // 방어 물자가 건설되어 있지 않은 상태이면 건설할 수 있다고 표시해주어야 한다.
        if (targetTag == "DEFENSIVEGOODS")
        {
            // 각각의 상황에 따로 SetActive를 처리하는 이유는 Raycast를 통해서 무언가를 비추고는 있는데
            // 내가 원하는 타겟이 아닌 경우가 있기 때문에 원하는 타겟일 경우에만 표시하도록 처리한 것.
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }

            targetInfoText.text = TargetInfoTextSetting(targetTag);
            // 방어물자가 건설되어 있지 않은 상태이면 건설할 수 있다고 표시
            // 건설된 상태이면? 수리 가능한 상황일 경우 따로 표시를 해서 수리 진행하게 처리.
            //Debug.Log("Defensive Goods State");


            // 건설할 수 있는 상황이면
            //if(canBuild && FillGauge(buildTime)){ // Build 과정 진행 }
            // 수리할 수 있는 상황이면
            //else if(canRepair && FillGauge(repairTime)){ // 수리 과정 진행 }

        }
        // 상점에 다가가면 상점이라고 표시가 뜨고 키를 누르면 상점이 열린다.
        else if (targetTag == "STORE" && Vector3.Distance(this.transform.position, target.transform.position) <= 5f)
        {
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }
            targetInfoText.text = TargetInfoTextSetting(targetTag);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Store Open");
            }
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    FillGauge(5f);
            //}
            //else if (Input.GetKeyUp(KeyCode.E))
            //{
            //    GaugeClear();
            //}
        }
        // 플레이어에게 다가가면 플레이어의 이름이 표시된다.
        else if (targetTag == "PLAYER")
        {
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }

            //Debug.Log("Player Live State");
        }
        // 보고있는 대상은 있는데 그 대상이 내가 원하는 대상이 아닐 경우 정보 표시가 필요없다.
        else
        {
            GaugeClear();
            if (targetInfoPanel.activeSelf == true)
            {
                targetInfoPanel.SetActive(false);
            }
        }

    }

    #region Target Information UI
    /// <summary>
    /// Target의 Information Text를 Setting할 때 사용하는 함수
    /// </summary>
    /// <param name="_text">Change this text to bold</param>
    /// <returns>필요한 부분을 Bold체로 변경한 값을 반환</returns>
    private string TargetInfoTextSetting(string _text)
    {
        string _string = string.Format($"<b>{_text}</b>");
        return _string;
    }


    #endregion

    #region Gauge UI Control Function
    /// <summary>
    /// 특정 동작시 게이지를 채우기 위해서 동작하는 함수
    /// </summary>
    /// <param name="_chargingTime">게이지를 채우는데 걸리는 시간</param>
    /// <returns>게이지가 100%가 되면 true 아닐경우 false</returns>
    private bool FillGauge(float _chargingTime)
    {
        //Debug.Log("____FILL GAUGE____");
        if (crosshair.activeSelf == true) { crosshair.SetActive(false); }
        if (gaugeRing.activeSelf == false) { gaugeRing.SetActive(true); }
        Image ringImgae = gaugeRing.GetComponent<Image>();
        ringImgae.fillAmount += (1 / _chargingTime) * Time.deltaTime;

        if (ringImgae.fillAmount >= 1f) { return true; }

        return false;
    }

    /// <summary>
    /// FillGauge를 실행하던 중 취소되거나 완료 시 실행되는 함수.
    /// </summary>
    private void GaugeClear()
    {
        //Debug.Log("____GAUGE CLEAR____");
        // ring이 표시되어있는 상태일 경우 fillAmount를 0으로 해서 다음에 시도할때 0부터 차도록 한다.
        if (gaugeRing.activeSelf == true)
        {
            gaugeRing.GetComponent<Image>().fillAmount = 0f;
            gaugeRing.SetActive(false);
        }
        // 마우스의 기본 표시는 crosshair이므로 기본으로 되돌려준다.
        if (crosshair.activeSelf == false) { crosshair.SetActive(true); }

    }
    #endregion

}
