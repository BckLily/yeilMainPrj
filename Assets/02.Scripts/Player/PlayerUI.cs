using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
    public WeaponManager weaponManager;
    public PlayerAction playerAction;


    #region Status UI

    // 플레이어 스탯 UI
    public GameObject playerStatusUI;
    // 플레이어 스탯 UI가 열려있는가
    private bool statusUIisOpen = false;


    // 플레이어 이름 텍스트
    public Text nameText;
    // 플레이어 currHp / maxHp 텍스트
    public Text hpText;
    // 플레이어 직업 텍스트
    public Text classText;
    // 플레이어 레벨 텍스트
    public Text levelText;
    // 플레이어 방어력 텍스트
    public Text armourText;

    // 무기 이름 텍스트
    public Text weaponText;
    // 공격력 텍스트 weaponManage.currGun.damage + playerCtrl.incDamage
    public Text damageText;
    // 공격 속도 텍스트 weaponManager.currGun.fireDelay
    public Text attackSpeedText;

    // 아이템 회복량 playerAction.healingPoint
    public Text healingPointText;
    // 아이템 회복 속도 playerAction.healingSpeed
    public Text healingSpeedText;
    // 건설 속도 playerActioin.buildSpeed
    public Text buildSpeedText;
    // 수리 속도 playerAction.repairSpeed
    public Text repairSpeedText;
    // 방어 물자 자동 회복 playerAction.autoRepair
    public Text autoRepairText;

    Coroutine coUIUpdate;

    #endregion

    #region Player Skill

    // 스킬 포인트를 가지고 있다는 것을 알려주는 오브젝트
    public GameObject skillPointInfoObj;
    public bool havingSkillPoint_isRunning = false;

    // 획득할 수 있는 스킬을 표시해주는 오브젝트
    public GameObject skillSelectObj;
    public bool selectObjisOpen = false;


    #endregion 


    private void Start()
    {
        statusUIisOpen = false;
        //statusUIisOpen = playerStatusUI.activeSelf;
        //StartCoroutine(StatusUIActive());

    }

    private void Update()
    {

        // O(o) 키를 누르면 스탯 UI 창을 켰다 껐다한다.
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
        // 스킬 포인트를 가지고 있을 경우 스킬 선택 UI를 켜고
        // 스킬 선택 UI가 켜져있을 경우 끌 수 있게 한다.
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (playerCtrl.skillPoint >= 1 && skillSelectObj.activeSelf == false)
            {
                Cursor.lockState = UnityEngine.CursorLockMode.None; // 커서 고정을 끈다
                skillSelectObj.SetActive(true);
            }
            else if (skillSelectObj.activeSelf == true)
            {
                Cursor.lockState = CursorLockMode.Locked; // 커서를 고정한다.
                skillSelectObj.SetActive(false);
            }

            selectObjisOpen = skillSelectObj.activeSelf;
        }
    }


    /// <summary>
    /// 스킬 포인트를 가지고 있을 경우 Level 표시 근처에 켜졌다 꺼졌다하는 표시 갱신 코루틴. 
    /// <br/>스킬 포인트를 획득하면 실행된다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HaveSkillPoint()
    {
        float _alphaChange = 0.1f;
        havingSkillPoint_isRunning = true;

        Image _image = skillPointInfoObj.GetComponent<UnityEngine.UI.Image>();
        Color _color = _image.color;

        while (playerCtrl.skillPoint >= 1)
        {
            // UI Image의 color 값을 가져와서 alpha 값으 변경해준뒤 다시 대입하는 방식.
            // 직접 대입하는 방식이 되지 않는 것으로 알고 있다.
            _color.a += _alphaChange;
            _image.color = _color;

            // alpha 값은 0~1이고 최대로 차거나 최소로 줄어들면 증가하는 값이 반대로 바뀐다.
            if (_color.a >= 1f || _color.a <= 0f)
            {
                _alphaChange *= -1f;
            }

            yield return new WaitForSeconds(0.15f);
        }

        _color.a = 0f;
        _image.color = _color;

        // havingSkillPoint Coroutine이 끝났으므로 false로 만들어 준다.
        havingSkillPoint_isRunning = false;


        //Debug.Log($"Skill Point: Done");
    }


    /// <summary>
    /// 스탯 UI를 활성화하고 갱신하는 코루틴
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
                        classToKorean = "소총병";
                        break;
                    case PlayerClass.ePlayerClass.Medic:
                        classToKorean = "의무병";
                        break;
                    case PlayerClass.ePlayerClass.Engineer:
                        classToKorean = "공병";
                        break;
                    default:
                        classToKorean = "오류 발생";
                        break;
                }

                classText.text = $"{classToKorean.ToString()}";


                levelText.text = $"{playerCtrl.level.ToString()}";
                armourText.text = $"{playerCtrl.addArmour.ToString()}";
                weaponText.text = $"{weaponManager.weaponNameText.text.ToString()}";
                damageText.text = $"{(weaponManager.currGun.damage + playerCtrl.addAttack).ToString()}";
                attackSpeedText.text = $"{weaponManager.currGun.fireDelay.ToString()}";
                healingPointText.text = $"{playerAction.currHealingPoint.ToString()}";
                healingSpeedText.text = $"{playerAction.currHealingSpeed.ToString()}";
                repairSpeedText.text = $"{playerAction.currRepariSpeed.ToString()}";
                buildSpeedText.text = $"{playerAction.currBuildSpeed.ToString()}";

                string autoRepair = playerAction.buildingAutoRepair ? "보유" : "미보유";

                autoRepairText.text = $"{autoRepair.ToString()}";

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




}