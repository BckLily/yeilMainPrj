                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    public PlayerCtrl playerCtrl;
    public WeaponManager weaponManager;
    public PlayerAction playerAction;
    public PlayerSkillManager playerSkillManager;

    #region Status UI
    [Header("Player Status UI")]
    // ???????????? ?????? UI
    public GameObject playerStatusUI;
    // ???????????? ?????? UI??? ???????????????
    private bool statusUIisOpen = false;


    // ???????????? ?????? ?????????
    public Text nameText;
    // ???????????? currHp / maxHp ?????????
    public Text hpText;
    // ???????????? ?????? ?????????
    public Text classText;
    // ???????????? ?????? ?????????
    public Text levelText;
    // ???????????? ????????? ?????????
    public Text armourText;

    // ?????? ?????? ?????????
    public Text weaponText;
    // ????????? ????????? weaponManage.currGun.damage + playerCtrl.incDamage
    public Text damageText;
    // ?????? ?????? ????????? weaponManager.currGun.fireDelay
    public Text attackSpeedText;

    // ????????? ????????? playerAction.healingPoint
    public Text healingPointText;
    // ????????? ?????? ?????? playerAction.healingSpeed
    public Text healingSpeedText;
    // ?????? ?????? playerActioin.buildSpeed
    public Text buildSpeedText;
    // ?????? ?????? playerAction.repairSpeed
    public Text repairSpeedText;
    // ?????? ?????? ?????? ?????? playerAction.autoRepair
    public Text autoRepairText;

    Coroutine coUIUpdate;

    public Text pointText; // ?????? ????????? ?????????

    public Image expImage; // ???????????? ????????? ?????????

    #endregion

    #region Player Skill
    [Header("Player SKill")]
    // ?????? ???????????? ????????? ????????? ?????? ???????????? ????????????
    public GameObject skillPointInfoObj;
    public bool havingSkillPoint_isRunning = false;

    // ????????? ??? ?????? ????????? ??????????????? ????????????
    public GameObject skillSelectObj;
    public bool selectObjisOpen = false;

    public Image havingSkillPointImage;

    // ????????? ??? ?????? ????????? ????????? ??????????????? Object ?????????
    public List<GameObject> skillInfo_ObjList = new List<GameObject>();

    // ????????? ??? ?????? ????????? ????????? ???????????? Image??? Text
    public List<UnityEngine.UI.Image> skillInfo_ImageList = new List<Image>();
    public List<UnityEngine.UI.Text> skillInfo_TextList = new List<Text>();

    #endregion

    #region Player Item UI
    [Header("Player Item UI")]
    // ???????????? ?????? ??? ????????? ?????? Panel
    public Image itemPanel;
    // ???????????? ?????????
    public Image itemImg;

    #endregion

    #region Player Action UI
    [Header("Player Action UI")]
    // SerializeField??? ????????? Private
    // ?????? private??? ???????????? Awake?????? ????????? ????????? ??????????????? ????????? ?????????????????? ?????????.
    public Image playerActionPanel;
    public Text playerActionText;

    IEnumerator IEnumActionText = null;

    #endregion

    #region Menu UI
    [Header("Menu UI")]
    public Image menuPanel;
    #endregion

    private void Start()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();

        statusUIisOpen = false;
        //statusUIisOpen = playerStatusUI.activeSelf;
        //StartCoroutine(StatusUIActive());

    }

    private void Update()
    {

        // O(o) ?????? ????????? ?????? UI ?????? ?????? ????????????.
        if (Input.GetKeyDown(KeyCode.O))
        {
            statusUIisOpen = !statusUIisOpen;

            try
            {
                playerStatusUI.SetActive(statusUIisOpen);
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(e.GetType());
#endif
            }

            if (statusUIisOpen == true)
                coUIUpdate = StartCoroutine(StatusUIActive());
        }
        // ?????? ???????????? ????????? ?????? ?????? ?????? ?????? UI??? ??????
        // ?????? ?????? UI??? ???????????? ?????? ??? ??? ?????? ??????.
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (playerCtrl.skillPoint >= 1 && skillSelectObj.activeSelf == false)
            {
                //Cursor.lockState = UnityEngine.CursorLockMode.None; // ?????? ????????? ??????
                CursorState.CursorLockedSetting(false); // ?????? ????????? ??????.
                skillSelectObj.SetActive(true);
            }
            else if (skillSelectObj.activeSelf == true && playerCtrl.isUIOpen == false)
            {
                //Cursor.lockState = CursorLockMode.Locked; // ????????? ????????????.
                CursorState.CursorLockedSetting(true); // ????????? ????????????.
                skillSelectObj.SetActive(false);
            }

            selectObjisOpen = skillSelectObj.activeSelf;
        }
    }

    #region Skill Point UI

    public void HavingSkillPoint(bool _havingSkillPoint)
    {
        havingSkillPointImage.gameObject.SetActive(_havingSkillPoint);
    }

    #region ???????????? ?????? ??????
    /// <summary>
    /// ?????? ???????????? ????????? ?????? ?????? Level ?????? ????????? ????????? ??????????????? ?????? ?????? ?????????. 
    /// <br/>?????? ???????????? ???????????? ????????????.
    /// </summary>
    /// <returns></returns>
    //public IEnumerator HaveSkillPoint()
    //{
    //    float _alphaChange = 0.1f;
    //    havingSkillPoint_isRunning = true;

    //    Image _image = skillPointInfoObj.GetComponent<UnityEngine.UI.Image>();
    //    Color _color = _image.color;

    //    while (playerCtrl.skillPoint >= 1)
    //    {
    //        // UI Image??? color ?????? ???????????? alpha ?????? ??????????????? ?????? ???????????? ??????.
    //        // ?????? ???????????? ????????? ?????? ?????? ????????? ?????? ??????.
    //        _color.a += _alphaChange;
    //        _image.color = _color;

    //        // alpha ?????? 0~1?????? ????????? ????????? ????????? ???????????? ???????????? ?????? ????????? ?????????.
    //        if (_color.a >= 1f || _color.a <= 0f)
    //        {
    //            _alphaChange *= -1f;
    //        }

    //        yield return new WaitForSeconds(0.15f);
    //    }

    //    _color.a = 0f;
    //    _image.color = _color;

    //    // havingSkillPoint Coroutine??? ??????????????? false??? ????????? ??????.
    //    havingSkillPoint_isRunning = false;


    //    //Debug.Log($"Skill Point: Done");
    //}

    #endregion

    #endregion

    #region Status UI 
    /// <summary>
    /// ?????? UI??? ??????????????? ???????????? ?????????
    /// </summary>
    /// <returns></returns>
    public IEnumerator StatusUIActive()
    {
        while (statusUIisOpen)
        {
            try
            {
                nameText.text = $"{playerCtrl.playerName.ToString()}";
                hpText.text = $"{playerCtrl.currHP.ToString()} / {playerCtrl.maxHp.ToString()}";

                string classToKorean;

                switch (playerCtrl.playerClass)
                {
                    case PlayerClass.ePlayerClass.Soldier:
                        classToKorean = "?????????";
                        break;
                    case PlayerClass.ePlayerClass.Medic:
                        classToKorean = "?????????";
                        break;
                    case PlayerClass.ePlayerClass.Engineer:
                        classToKorean = "??????";
                        break;
                    default:
                        classToKorean = "?????? ??????";
                        break;
                }

                classText.text = $"{classToKorean.ToString()}";

                levelText.text = $"{playerCtrl.level.ToString()}";
                armourText.text = $"{playerCtrl.addArmour.ToString()}";
                weaponText.text = $"{weaponManager.weaponNameText.text.ToString()}";
                damageText.text = $"{(weaponManager.currGun.damage + playerCtrl.addAttack).ToString("F2")}";
                attackSpeedText.text = $"{weaponManager.currGun.fireDelay.ToString("F2")}";
                healingPointText.text = $"{playerAction.currHealingPoint.ToString("F2")}";
                healingSpeedText.text = $"{playerAction.currHealingSpeed.ToString("F2")}";
                repairSpeedText.text = $"{playerAction.currRepariSpeed.ToString("F2")}";
                buildSpeedText.text = $"{playerAction.currBuildSpeed.ToString("F2")}";

                string autoRepair = playerAction.buildingAutoRepair ? "??????" : "?????????";

                autoRepairText.text = $"{autoRepair.ToString()}";
                pointText.text = $"{playerCtrl._point.ToString()}";
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(e.GetType());
#endif
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }


    public void ExpUISetting()
    {
        expImage.fillAmount = playerCtrl._playerExp / playerCtrl.targetExp;
    }


    #endregion

    #region Button ??????

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        bool select = false;

        //Debug.Log("____Press Event: " + eventData.pointerCurrentRaycast.ToString());
        if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[0])
        {
            // ?????? ???????????? 1?????? ?????????.
            playerCtrl.skillPoint -= 1;
            // ?????? ????????? ?????? ???????????? ?????? ????????? ??????????????? false??? ????????????.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[0]);

            select = true;
        }
        else if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[1])
        {
            // ?????? ???????????? 1?????? ?????????.
            playerCtrl.skillPoint -= 1;
            // ?????? ????????? ?????? ???????????? ?????? ????????? ??????????????? false??? ????????????.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[1]);

            select = true;
        }
        else if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[2])
        {
            // ?????? ???????????? 1?????? ?????????.
            playerCtrl.skillPoint -= 1;
            // ?????? ????????? ?????? ???????????? ?????? ????????? ??????????????? false??? ????????????.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[2]);

            select = true;
        }

        if (select)
        {
            // ????????? ?????? ????????? ?????????????????? ?????? ?????? ?????? ??????.
            skillSelectObj.SetActive(false);

            // ?????? ?????? ???????????? ????????? ????????? ?????? ???????????????.
            // ?????? ???????????? ????????? ?????? ?????? ?????? ??? ????????? ?????? ?????? ???????????? ????????????.
            if (playerCtrl.skillPoint <= 0)
            {
                if (playerCtrl.isUIOpen == false)
                {
                    CursorState.CursorLockedSetting(true);
                }
            }
        }

    }

    #endregion

    #region Item UI
    public void ItemUISetting(int _lastUID)
    {

        if (playerCtrl.isHaveItem == true)
        {
            // Item Panel active
            itemPanel.gameObject.SetActive(true);

            if (playerCtrl.haveMedikit == true)
            {
                // Medikit Image acitve
                itemImg.sprite = Resources.Load<Sprite>("Store/ItemImage/MedikitImg");
            }
            else if (playerCtrl.haveDefStruct == true)
            {
                // DefStruct Image active
                // ??????
                if (_lastUID == 0)
                {
                    // Iron Fence image active
                    itemImg.sprite = Resources.Load<Sprite>("Store/DefensiveStructureImage/Fence");
                }
                // ?????????
                else if (_lastUID == 1)
                {
                    // Barbed Wire image active
                    itemImg.sprite = Resources.Load<Sprite>("Store/DefensiveStructureImage/BarbedWire");
                }
            }
        }
        else
        {
            // Item Panel inactive
            itemPanel.gameObject.SetActive(false);
        }


    }
    #endregion

    #region Player Action UI


    public void PlayerActionTextSetting(string _text)
    {
        // ???????????? ?????? ???????????? ???????????? ????????? ???????????? ?????? ????????? ??????
        if (IEnumActionText != null)
        {
            StopCoroutine(IEnumActionText);
        }
        // ????????? ???????????? ????????? ???????????? ???????????????.
        IEnumActionText = CoActionTextSetting(_text);
        StartCoroutine(IEnumActionText);
    }

    IEnumerator CoActionTextSetting(string _text)
    {
        // ?????? ???????????? ????????? Panel??? ???????????? ??????????????? ??????????????????.
        if (playerActionPanel.gameObject.activeSelf == false)
            playerActionPanel.gameObject.SetActive(true);
        playerActionText.text = string.Format($"{_text.ToString()}");
        yield return new WaitForSeconds(1f);
        playerActionText.text = string.Format("");
        // ?????? ???????????? ????????? Panel??? ???????????????????????? ??????????????????.
        if (playerActionPanel.gameObject.activeSelf == true)
            playerActionPanel.gameObject.SetActive(false);
        // ???????????? ??????????????? ????????? ??? ???????????? ????????? null??? ????????????.
        IEnumActionText = null;

    }

    #endregion 



}                                                                                                                                                                                                                                                                                                                                                                             