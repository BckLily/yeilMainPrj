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
        if (GameManager.instance.perk0_Active == false && GameManager.instance._stage > 1)
        {
#if UNITY_EDITOR
            Debug.Log("특전 1 활성화");
#endif
            GameManager.instance.perk0_Active = true;
            Achieve_GetPerk(0);
        }
        else if (GameManager.instance.perk1_Active == false && GameManager.instance._stage > 2)
        {
#if UNITY_EDITOR
            Debug.Log("특전 2 활성화");
#endif
            GameManager.instance.perk1_Active = true;
            Achieve_GetPerk(1);
        }
        else if (GameManager.instance.perk2_Active == false && GameManager.instance._stage > 3)
        {
#if UNITY_EDITOR
            Debug.Log("특전 3 활성화");
#endif
            GameManager.instance.perk2_Active = true;
            Achieve_GetPerk(2);
        }
    }

    public static void Achieve_GetPerk(int num)
    {
        // 모든 플레이어를 대상으로 진행되어야 한다.
        // 멀티가 될 경우 PhotonNetwork.Players를 통해서 모든 플레이어를 가져올 수 있는 것으로 알고 있다.
        foreach (var player in GameManager.instance.players)
        {
            player.GetComponent<PlayerCtrl>().ItemSetting($"06001000{num}");
        }
    }


}
