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

    #endregion 



    private void Start()
    {
        statusUIisOpen = false;
        //statusUIisOpen = playerStatusUI.activeSelf;
        //StartCoroutine(StatusUIActive());

        StartCoroutine(HaveSkillPoint());
    }

    private void Update()
    {
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
    }

    private IEnumerator HaveSkillPoint()
    {
        float _alphaChange = 0.1f;
        while (true)
        {

            while (playerCtrl.skillPoint >= 1)
            {

                Color _color = skillPointInfoObj.GetComponent<UnityEngine.UI.Image>().color;
                _color.a += _alphaChange;
                skillPointInfoObj.GetComponent<UnityEngine.UI.Image>().color = _color;

                if (_color.a >= 1f || _color.a <= 0f)
                {
                    _alphaChange *= -1f;
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

    }



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
            yield return new WaitForSeconds(1f);

        }

        yield break;
    }




}
