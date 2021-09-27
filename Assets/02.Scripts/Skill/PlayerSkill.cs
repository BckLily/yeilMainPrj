using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    // 이거도 Invokde로 써야하나.


    public void IncreaseWeaponDamage(int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo("IncreaseDamage", _skillLevel);

        float incDamage = skillinfo[0];

        // 데미지 증가량 반환
        // return incDamage;
    }




}
