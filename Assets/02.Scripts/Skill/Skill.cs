using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Skill
{

    /// <summary>
    /// 일정 확률로 총알을 사용하지 않는 스킬
    /// </summary>
    /// <param name="_level">Skill Level</param>
    /// <returns>Percentage multiple skill level</returns>
    public List<float> DontUseBullet(int _level)
    {
        float skillPercent = 1f;
        List<float> _return = new List<float>();
        _return.Add(skillPercent * _level);

        return _return;
    }

    /// <summary>
    /// 일정 확률로 아이템을 사용하지 않는 스킬
    /// </summary>
    /// <param name="_level">Skill Level</param>
    /// <returns>Percentage multiple skill level</returns>
    public List<float> DontUseHealItem(int _level)
    {
        float skillPercent = 1f;
        List<float> _return = new List<float>();
        _return.Add(skillPercent * _level);

        return _return;
    }

    /// <summary>
    /// 자동으로 체력을 회복하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Recovery health point multiple skill level</returns>
    public List<float> AutoRecovery(int _level)
    {
        float skillPercent = 1f;
        List<float> _return = new List<float>();
        _return.Add(skillPercent * _level);

        return _return;
    }

    /// <summary>
    /// 공격력이 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase damage multiple skill level</returns>
    public List<float> IncreaseDamage(int _level)
    {
        float incDamage = 1f;
        List<float> _return = new List<float>();
        _return.Add(incDamage * _level);

        return _return;
    }

    /// <summary>
    /// 공격 속도가 증가하는 스킬
    /// </summary>
    /// <param name="_level"></param>
    /// <returns>Increase attack speed multiple skill level</returns>
    public List<float> IncreaseAttackSpeed(int _level)
    {
        float incAttackSpeed = 1f;
        List<float> _return = new List<float>();
        _return.Add(incAttackSpeed * _level);

        return _return;
    }

    /// <summary>
    /// 가지고 다닐 수 있는 최대 총알이 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill Level</param>
    /// <returns>increase max carry bullet multiple skill level</returns>
    public List<float> IncreaseMaxBullet(int _level)
    {
        float incMaxBullet = 1f;
        List<float> _return = new List<float>();
        _return.Add(incMaxBullet);

        return _return;
    }

    /// <summary>
    /// 방어력이 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase armor multiple skill level</returns>
    public List<float> IncreaseArmor(int _level)
    {
        float incArmor = 0.5f;
        List<float> _return = new List<float>();
        _return.Add(incArmor);

        return _return;
    }

    /// <summary>
    /// 최대 체력이 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase max health point multiple skill level</returns>
    public List<float> IncreaseMaxHealthPoint(int _level)
    {
        float incMaxHp = 1f;
        List<float> _return = new List<float>();
        _return.Add(incMaxHp);

        return _return;
    }

    /// <summary>
    /// 아이템 회복량이 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase healing point multiple skill level</returns>
    public List<float> IncreaseHealingPoint(int _level)
    {
        float incHealingPoint = 1f;
        List<float> _return = new List<float>();
        _return.Add(incHealingPoint);

        return _return;
    }

    /// <summary>
    /// 회복 아이템 사용 속도가 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase healing speed multiple skill level</returns>
    public List<float> IncreaseHealingSpeed(int _level)
    {
        float incHealingSpeed = 1f;
        List<float> _return = new List<float>();
        _return.Add(incHealingSpeed);

        return _return;
    }

    /// <summary>
    /// 건물 건설 속도를 증가시키는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase build speed multiple skill level</returns>
    public List<float> IncreaseBuildSpeed(int _level)
    {
        float incBuildSpeed = 1f;
        List<float> _return = new List<float>();
        _return.Add(incBuildSpeed);

        return _return;
    }

    /// <summary>
    /// 수리 속도가 증가하는 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Increase repair speed multiple skill level</returns>
    public List<float> IncreaseRepairSpeed(int _level)
    {
        float incRepairSpeed = 1f;
        List<float> _return = new List<float>();
        _return.Add(incRepairSpeed);

        return _return;
    }

    /// <summary>
    /// 출혈 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Bleeding damage multiple skill level</returns>
    public List<float> Bleeding(int _level)
    {
        float damage = 1f;
        float duration = 1f;
        List<float> _return = new List<float>();
        _return.Add(damage);
        _return.Add(duration);

        return _return;
    }

    /// <summary>
    /// 회복 스킬
    /// </summary>
    /// <param name="_level">Skill level</param>
    /// <returns>Healing health point multiple skill level</returns>
    public List<float> Healing(int _level)
    {
        float healing = 1f;
        List<float> _return = new List<float>();
        _return.Add(healing);

        return _return;
    }

}

/// <summary>
/// Skill을 찾는데 사용하는 FindSkill Class
/// </summary>
public class FindSkill
{
    /// <summary>
    /// Skill의 이름과 레벨을 받아서 계수같은 내부 값들을 반환하는 함수
    /// </summary>
    /// <param name="_skillName">Input skill name</param>
    /// <param name="_skillLevel">Input skill level</param>
    /// <returns>Return skill Information to float List</returns>
    public static List<float> GetSkillInfo(string _skillName, int _skillLevel)
    {
        if (_skillLevel == 0)
        {
            return null;
        }

        Debug.Log("____FIND SKILL____");

        // Skill class를 Type으로 받아온다.
        System.Type tp = typeof(Skill);
        // 입력한 스킬의 이름과 같은 스킬을 Skill class에서 찾는다.
        MethodInfo skillMethod = tp.GetMethod(_skillName);

        Debug.Log(_skillName);
        Debug.Log(skillMethod);

        Skill _skill = new Skill();

        object skillObj = skillMethod.Invoke(_skill, new object[] { _skillLevel });

        //foreach (var __skill in (List<float>)skillObj)
        //{
        //    Debug.Log(_skillName + "(" + _skillLevel + ") :" + __skill);
        //}

        Debug.Log("____END SKILL____");

        return (List<float>)skillObj;
    }

}