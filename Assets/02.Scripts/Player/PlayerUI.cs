using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
    public WeaponManager weaponManager;
    public PlayerAction playerAction;

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

    private void Start()
    {
        StartCoroutine(StatusUIActive(true));
    }

    private void Update()
    {

    }

    public IEnumerator StatusUIActive(bool _active)
    {
        while (true) {
            yield return new WaitForSeconds(2f);
            try
            {
                nameText.text = playerCtrl.playerName;
                hpText.text = playerCtrl.currHP.ToString() + " / " + playerCtrl.maxHp.ToString();
                classText.text = playerCtrl.playerClass.ToString();
                levelText.text = playerCtrl.level.ToString();
                armourText.text = playerCtrl.addArmour.ToString();
                weaponText.text = weaponManager.weaponNameText.text;
                damageText.text = (weaponManager.currGun.damage + playerCtrl.addAttack).ToString();
                attackSpeedText.text = weaponManager.currGun.fireDelay.ToString();
                healingPointText.text = playerAction.currHealingPoint.ToString();
                healingSpeedText.text = playerAction.currHealingSpeed.ToString();
                repairSpeedText.text = playerAction.currRepariSpeed.ToString();
                autoRepairText.text = playerAction.buildingAutoRepair.ToString();
                buildSpeedText.text = playerAction.currBuildSpeed.ToString();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
    }
    }




}
