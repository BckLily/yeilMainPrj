using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementManager
{
    //public static bool havePerk0;
    //public static bool havePerk1;
    //public static bool havePerk2;

    /// <summary>
    /// 특전 획득에 성공했는지 확인하는 함수
    /// </summary>
    public static void CheckAchievement_Perk()
    {
        if (GameManager.instance.perk0_Active == false && GameManager.instance._stage > 10)
        {
            GameManager.instance.perk0_Active = true;
            Achieve_GetPerk(0);
        }
        else if (GameManager.instance.perk1_Active == false && GameManager.instance._stage > 15)
        {
            GameManager.instance.perk1_Active = true;
            Achieve_GetPerk(1);
        }
        else if (GameManager.instance.perk2_Active == false && GameManager.instance._stage > 22)
        {
            GameManager.instance.perk2_Active = true;
            Achieve_GetPerk(2);
        }
    }

    public static void Achieve_GetPerk(int num)
    {
        foreach (var player in GameManager.instance.players)
        {
            player.GetComponent<PlayerCtrl>().ItemSetting($"06001000{num}");
        }
    }




}
