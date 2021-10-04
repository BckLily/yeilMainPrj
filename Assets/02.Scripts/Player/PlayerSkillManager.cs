using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    private PlayerCtrl playerCtrl;

    #region 플레이어 스킬 관련 변수
    public PlayerUI playerUI;

    // internal은 외부 프로젝트에서 접근할 수 없는
    // 내부 프로젝트에서만 public으로 사용되는 제한자라고 한다.
    internal int status0_Level = 0;
    internal int status1_Level = 0;
    internal int status2_Level = 0;
    internal int ability0_Level = 0;
    internal int ability1_Level = 0;
    internal int perk0_Level = 0;
    internal int perk1_Level = 0;
    internal int perk2_Level = 0;

    // 스킬 세팅이 되어있는지 확인하는 변수
    public bool skillSettingComplete = false;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        playerUI = GetComponent<PlayerUI>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 설정된 스킬 3개를 가져와서 UID를 통해서 스킬 정보를 가져오고 UI에 표시하는 함수.
    /// 그런데 UID를 가져와도 정보를 다 가져오는게 아니라서 이 함수 내에서 String 값으로 텍스트에 직접 넣어줘야한다.
    /// 일반적인 방식은 아마 가졍는 데이터에 스킬의 설명도 같이 있어서 그 파일을 수정하면 되는 방식일텐데
    /// 가라로 만들다보니 많이 이상해졌다.
    /// </summary>
    /// <returns></returns>
    internal IEnumerator SelectSkillSetting()
    {


        yield return null;
    }







}
