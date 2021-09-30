using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InterfaceSet;
using System;
using UnityEngine.UI;



public class PlayerCtrl : LivingEntity, IAttack, IDamaged
{

    public Image playerHpImage; // 플레이어 체력바
    public Text playerHpText; // 플레이어 체력 텍스트

    #region 플레이어 Status 관련 변수
    /// <summary>
    /// 플레이어 최대 체력
    /// </summary>
    public float maxHp { get { return startHp + addHP; } }
    /// <summary>
    /// 플레이어 추가 체력
    /// </summary>
    public float addHP { get; private set; }
    /// <summary>
    /// 플레이어 현재 체력
    /// </summary>
    //public float currHP { get; private set; } // LivingEntity 에서 받음
    /// <summary>
    /// 플레이어 추가 방어력
    /// </summary>
    public float addArmour { get; private set; }


    //private float addAttack =0f;
    internal float addAttack =0f;
    // 데미지 증가 Perk 발동 확률
    private float attackPerk_Percent = 0f;
    private float addAttack_Perk = 0f;
    /// <summary>
    /// 현재 플레이어 추가 공격력
    /// </summary>
    public float currAddAttack
    {
        get
        {
            float _value = addAttack;

            if (perk2_Level >= 1)
            {
                int percent = UnityEngine.Random.Range(0, 100);
                if (!(percent >= (100 - attackPerk_Percent)))
                {
                    _value += addAttack_Perk;
                    //Debug.Log("___VALUE: " + _value + "___");
                }
            }

            return _value;
        }
    }

    public Image bloodScreen;

    private int statusMaxLevel = 5;
    private int abilityMaxLevel = 3;
    private int perkMaxLevel = 1;

    public int level = 1;

    #endregion

    #region 플레이어 이동 관련 변수
    public CharacterController controller; // 플레이어의 움직임에 사용될 CharacterController 컴포넌트

    private float useSpeed; // 플레이어의 이동에 직접 사용될 변수
    private float changedSpeed; // 플레이어의 동작이 변경했을 때 바뀌는 속도
    // Lerp를 사용해서 useSpeed가 changedSpeed로 천천히 변한다.
    private float walkSpeed; // 플레이어가 걷는 속도
    private float runSpeed; // 플레이어가 달리는 속도
    private float crouchSpeed; // 플레이어가 앉았을 때 속도
    private float gravity; // 플레이어 적용 중력

    private float motionChangeSpeed; // 플레이어의 이동 속도 보간 값.

    #endregion

    [Space(5)]
    [Header("Player Camera", order = 1)]
    #region 플레이어 카메라 관련 변수
    public Transform playerCameraTr; // 플레이어 카메라 위치
    private PlayerAction playerAction; // 플레이어의 동작과 관련 내용이 있는 스크립트
    private Vector3 cameraPosition; // 플레이어 카메라의 포지션 변경 값 저장 변수

    private Rigidbody myRb; // 플레이어의 Rigidbody

    /// <summary>
    /// 플레이어의 상체 회전 속도
    /// </summary>
    private float upperBodyRotation;
    /// <summary>
    /// 상체 회전의 한계 값.
    /// </summary>
    private float upperBodyRotationLimit;
    /// <summary>
    /// 화면(카메라) 회전 속도
    /// </summary>
    /// <param name=""></param>
    private float lookSensitivity;
    /// <summary>
    /// 카메라가 움직일 때 보간 값
    /// </summary>
    private float cameraMoveSpeed = 4f;
    private float cameraMoveValue = 0.375f;

    #endregion

    #region 플레이어 행동 bool 변수
    private bool isMove; // 플레이어가 움직이고 있는지 판단하는 변수
    private bool isCrouch; // 플레이어가 앉아있는지 판단하는 변수
    private bool doCrouch; // 플레이어가 앉아있을 때 달리면 이 값을 True로 만들어서 TryCrouch가 동작하게 한다.

    #endregion

    #region 플레이어 관련 변수
    private Transform tr; // 플레이어의 위치(Transform)
    public string playerName { get; private set; } // 플레이어 이름(유저 닉네임)
    public PlayerClass.ePlayerClass playerClass { get; private set; } // Player의 Class(직업)
    public Animator playerAnim; // 플레이어의 Animator

    #endregion

    #region 플레이어 직업 관련 변수
    Dictionary<string, string> classDict = null;

    #endregion

    #region 플레이어 스킬 관련 변수
    private int status0_Level = 0;
    private int status1_Level = 0;
    private int status2_Level = 0;
    private int ability0_Level = 0;
    private int ability1_Level = 0;
    private int perk0_Level = 0;
    private int perk1_Level = 0;
    private int perk2_Level = 0;

    #endregion

    #region 플레이어 무기 관련 변수
    public WeaponManager weaponManager = null;
    // 스킬 설정을 잘못해서 스킬을 레벨당 다른 결과 값을 출력하도록 되어있는데
    // 스킬은 레벨이 한 번에 한 번씩 오르므로 그냥 변수에 한 번 씩 값을 증가시키도록 했으면 됐는데
    // 다른 결과가 출력되서 증가가 아니라 대입을 시키는 방식으로 되어있어서 어쩔 수 없이 Perk 용의 변수를 따로 만들어야 한다...
    // 그리고 반환용 변수도 따로 만들어야 한다.....
    // 수정할 여유가 생기면 스킬은 매번 레벨마다 발생하는 증가값을 반환하게 처리하고
    // 변수 하나에 더하는 방식으로 하는 것이 좋을 것 같다.
    public float incCarryBullet;

    // 증가하는 공격 속도
    private float incAttackSpeed;
    // 증가하는 공격 속도 Perk
    private float incAttackSpeed_Perk;
    public float currIncAttackSpeed
    {
        get
        {
            float _value = (incAttackSpeed + incAttackSpeed_Perk);
            return _value;
        }
    }


    #endregion


    private void Awake()
    {
        playerAction = playerCameraTr.GetComponent<PlayerAction>();
        tr = this.GetComponent<Transform>();

        myRb = this.controller.GetComponent<Rigidbody>();
        #region 주석
        // 캐릭터 컨트롤러에 붙어있는 Rigidbody를 받아오는 것??
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {


        playerName = string.Format("Player1");
        playerClass = PlayerClass.ePlayerClass.Soldier;
        // 플레이어의 이름 및 플레이어의 직업 설정
        #region 주석
        /*
         * 플레이어의 직업은 나중에 다른 입력값을 통해서 결정이 될 것이다.
         * 현재는 플레이어의 결정된 직업에 따라서 
         * 들어오는 데이터를 처리해야 한다.
         */
        #endregion

        // String을 Enum Type으로 변경하는 방법.
        //playerClass = (PlayerClass.ePlayerClass)Enum.Parse(typeof(PlayerClass.ePlayerClass), classDict["ClassName"]);

        // 플레이어가 선택한 직업을 가져와서 설정이 끝나면
        // 그 직업과 관련된 데이터를 가져와서 Dictionary에 저장해야한다.
        StartCoroutine(CoPlayerClassSetting());


        controller.enabled = true;



        // 지금은 그냥 설정하고 있지만 나중에는 Init함수를 만들어서 PlayerClassSetting 코루틴의 마지막에 함수를 실행하는 방식으로 변경
        // 나중에는 함수를 사용해서 체력 및 방어력, 공격력 등을 설정할 것.
        // DB에서 데이터를 받아오는 식으로.
        // 그냥 프리팹에 기본 데이터를 설정해놓고 그대로 가져와서 처음 값을 설정하는 방식으로 변경.
        // 저번에 쓴적 있는 데이터만 저장해놓는 부분을 만들고 거기서 가져오는 방식으로
        startHp = 100f;
        addHP = 0f;
        currHP = maxHp;
        addArmour = 0f;
        addAttack = 0f;
        addAttack_Perk = 0f;
        #region 주석
        /*
         * addHp >> 추가 Status에 영향을 주는 것은 초반엔 직업 밖에 없으므로
         * 직업을 설정할 때 추가 Status에 대한 증가 설정을 끝낸다.
         * 최대 체력은 100f + addHP이고 시작할 때 현재 체력은 maxHP이다.
         */
        #endregion

        walkSpeed = 6f;
        runSpeed = walkSpeed * 1.5f;
        crouchSpeed = walkSpeed * 0.35f;
        changedSpeed = walkSpeed;
        useSpeed = changedSpeed;
        gravity = -0.098f;

        motionChangeSpeed = 4f;
        #region 주석
        /*
         * 플레이어들의 이동 속도는 6/s로 고정이며, 달리는 속도는 걷는 속도의 1.5배,
         * 앉아서 이동하는 속도는 걷는 속도의 0.35배로 설정된다.
         * 처음 플레이어는 서있는 상태이므로 기본 이동 속도인 walkSpeed를
         * 플레이어가 목표로하는 이동속도 changedSpeed로 설정하고, 처음엔 목표 속도와 현재 이동 속도가 동일해도 되므로
         * moveSpeed를 changedSpeed로 설정한다.
         * 각각 달리거나 서거나 앉는 동작이 변화할 때의 보간 값은 4로 설정한다.
         */
        #endregion

        upperBodyRotation = 0f;
        lookSensitivity = 6f;
        upperBodyRotationLimit = 35f;
        #region 주석
        /*
         * 상체의 회전각은 게임이 시작할 때는 정면을 보고 있으므로 0이고
         * 플레이어의 카메라가 상하 및 좌우 회전을 할 때의 임계값을 8f설정하였다.
         * 한 번에 플레이어의 상체가 좌우로 회전할 수 있는 최대 각은 35이다.
         */
        #endregion

        // 시작했을 때 플레이어는 가만히 서있을 것이기 때문에 imMove와 isCrouch에 false를 둔다.
        // 앉아있다가 달릴때 doCrouch가 true가 되어야 하므로 초기값은 doCrouch가 false가 된다
        isMove = false;
        isCrouch = false;
        doCrouch = false;


        // 시작할 때 카메라의 현재 위치를 저장한다.
        cameraPosition = playerCameraTr.localPosition;


        // 무기 관련 변수
        incAttackSpeed = 0f;
        incAttackSpeed_Perk = 0f;
        incCarryBullet = 0f;


        HPGaugeChange();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        currHP = maxHp;
    }


    #region 플레이어 직업을 세팅하고 직업 관련 데이터를 가져오는 함수 관련
    /// <summary>
    /// 플레이어 직업 관련된 값을 Dictionary 변수에 저장할 때까지 시도하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CoPlayerClassSetting()
    {
        yield return new WaitForSeconds(0.5f);
        while (classDict == null)
        {
            classDict = DBManager.Instance.GetClassInfo(playerClass);
            yield return null;
        }

        //weaponManager.WeaponChange(classDict["WeaponUID"]);
        PlayerWeaponChange();
        //Debug.Log(classDict["ClassName"]);
    }

    /// <summary>
    /// 플레이어가 스킬을 획득시 변경되는 값들을 설정하는 함수
    /// </summary>
    /// <param name="_playerSkillUID">획득한 스킬 UID</param>
    /// <param name="_skillLevel">획득한 스킬의 Level</param>
    void PlayerSkillSetting(string _playerSkillUID, int _skillLevel)
    {
        int firstUID;
        int middleUID;
        int lastUID;
        // _skillUID.Substring(startIdx, endIdx);
        try
        {
            Debug.Log("___UID: " + _playerSkillUID + "___");
            firstUID = int.Parse(_playerSkillUID.Substring(0, 2));
            Debug.Log("____UID: " + firstUID + "____");
            if (firstUID != 03)
            {
                Debug.Log("____Worng Skill UID Input____");
                return;
            }

            middleUID = int.Parse(_playerSkillUID.Substring(2, 3));
            Debug.Log("____UID: " + middleUID + "____");
            lastUID = int.Parse(_playerSkillUID.Substring(5, 4));
            Debug.Log("____UID: " + lastUID + "____");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            return;
        }

        Dictionary<string, string> __skillInfo = DBManager.Instance.GetPlayerSkill(_playerSkillUID);

        string _name = __skillInfo["PlayerSkill_Name"];
        string _skillUID = __skillInfo["PlayerSkill_SkillUID"];
        float _coefficient = float.Parse(__skillInfo["PlayerSkill_Coefficient"]);


        switch (middleUID)
        {
            //FindPlayerSkill.GetPlayerSkill(__skillInfo["PlayerSkill_Name"], __skillInfo["PlayerSkill_SkillUID"], _skillLevel)[0] * float.Parse(__skillInfo["PlayerSkill_Coefficient"])
            case 000:
                switch (lastUID)
                {
                    case 0000:
                        // Increase Weapon Damage
                        addAttack = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("Add Attack: " + addAttack);

                        break;
                    case 0001:
                        // Increase Player Armor
                        addArmour = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("Add Armour: " + addArmour);

                        break;
                    case 0002:
                        // Increase Player Max HP
                        addHP = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log("___Max Hp: " + maxHp);
                        HPGaugeChange();
                        break;
                    default:
                        Debug.Log("__Wrong UID Input__");

                        break;
                }

                break;
            case 001:
                switch (lastUID)
                {
                    case 0000:
                        // Incrase Max Carry Bullet
                        incCarryBullet = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log(status0_Level);
                        Debug.Log("___Inc Carry Bullet: " + incCarryBullet + "___");

                        break;
                    case 0001:
                        // Increase Weapon Attack Speed
                        incAttackSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log(status0_Level);
                        Debug.Log("___Inc Attack Speed: " + incAttackSpeed + "___");

                        break;
                    case 0002:
                        // Increase Item Healing Point
                        playerAction.incHealingPoint = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log("___INC HEALING POINT: " + playerAction.incHealingPoint + "___");

                        break;
                    case 0003:
                        // Increase Healing Item Use Speed
                        playerAction.incHealingSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log("___INC HEALING SPEED: " + playerAction.incHealingSpeed + "___");

                        break;
                    case 0004:
                        // Increase Build Speed
                        playerAction.incBuildSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log("___INC BUILD SPEED: " + playerAction.incBuildSpeed + "____");

                        break;
                    case 0005:
                        // Increase Repair Speed
                        playerAction.incRepairSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        Debug.Log("___INC REPAIR SPEED: " + playerAction.incRepairSpeed + "___");

                        break;
                    default:
                        Debug.Log("__Wrong UID Input__");

                        break;
                }

                break;
            case 002:
                switch (lastUID)
                {
                    case 0000:
                        if (perk0_Level >= 2) { return; }
                        // Increase Attack Speed Perk
                        incAttackSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        StartCoroutine(weaponManager.WeaponStatusSetting());
                        Debug.Log("___INC ATTACK SPEED PERK: " + incAttackSpeed_Perk + "___");

                        break;
                    case 0001:
                        // Dont Use Bullet Perk
                        if (perk1_Level >= 2) { return; }
                        float _value = (FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0]);
                        weaponManager.dontUseBulletPercent = _value * _coefficient;
                        weaponManager.dontUseBullet = (_value == 1f * _skillLevel) ? true : false;

                        Debug.Log("___Dont Use Bullet: " + weaponManager.dontUseBullet + "___");

                        break;
                    case 0002:
                        // Increase Weapon Damage Perk
                        if (perk2_Level >= 2) { return; }
                        List<float> _list = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel);
                        attackPerk_Percent = _list[0];
                        addAttack_Perk = _list[1] * _coefficient;

                        Debug.Log("___Attack Perk: " + addAttack_Perk + "____");


                        break;
                    case 0003:
                        // Increase Healing Point Perk
                        if (perk0_Level >= 2) { return; }
                        // 회복 아이템의 회복량 증가
                        playerAction.incHealingPoint_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("____ Healing Point Perk Increase ____");

                        break;
                    case 0004:
                        // Increase Healing Speed Perk 
                        if (perk1_Level >= 2) { return; }
                        playerAction.incHealingSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("____ Healing Speed Perk Increase ____");

                        break;
                    case 0005:
                        // Dont Use Healing Item Perk
                        if (perk2_Level >= 2) { return; }
                        playerAction.dontUseHealingItem_Percent = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("____ Healing Item Dont Use ____");

                        break;
                    case 0006:
                        // Increase Build Speed Perk
                        if (perk0_Level >= 2) { return; }
                        playerAction.incBuildSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("____ Build Speed Up Perk ____" + playerAction.incBuildSpeed_Perk);

                        break;
                    case 0007:
                        // Increase Building Max Health Point Perk
                        if (perk1_Level >= 2) { return; }
                        playerAction.incBuildMaxHealthPoint = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        Debug.Log("___ Building Max Health Point Incrase ____" + playerAction.incBuildMaxHealthPoint);

                        break;
                    case 0008:
                        // Building Auto Repair Perk
                        playerAction.buildingAutoRepair = (FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] == 1);

                        Debug.Log("____ Building Auto Repair ____" + playerAction.buildingAutoRepair);


                        break;
                    default:
                        Debug.Log("__Wrong UID Input__");


                        break;
                }

                break;
            default:
                Debug.Log("__Wrong UID Input__");
                Debug.Log("Middle UID: " + middleUID);
                Debug.Log("Last UID: " + lastUID);


                break;
        }


    }


    private void SkillLevelUp(string _skillUID)
    {
        if (_skillUID == classDict["StatusSkill0_UID"])
        {
            if (status0_Level < statusMaxLevel) { status0_Level++; }
            PlayerSkillSetting(classDict["StatusSkill0_UID"], status0_Level);
        }
        else if (_skillUID == classDict["StatusSkill1_UID"])
        {
            if(status1_Level < statusMaxLevel) { status1_Level++; }
            PlayerSkillSetting(classDict["StatusSkill1_UID"], status1_Level);
        }
        else if (_skillUID == classDict["StatusSkill2_UID"])
        {
            if(status2_Level < statusMaxLevel ) { status2_Level++; }
            PlayerSkillSetting(classDict["StatusSkill2_UID"], status2_Level);
        }
        else if (_skillUID == classDict["AbilitySkill0_UID"])
        {
            if(ability0_Level < abilityMaxLevel) { ability0_Level++; }
            PlayerSkillSetting(classDict["AbilitySkill0_UID"], ability0_Level);
        }
        else if (_skillUID == classDict["AbilitySkill1_UID"])
        {
            if (ability1_Level < abilityMaxLevel) { ability1_Level++; }
            PlayerSkillSetting(classDict["AbilitySkill1_UID"], ability1_Level);
        }
        else if (_skillUID == classDict["Perk0_UID"])
        {
            if(perk0_Level < perkMaxLevel) { perk0_Level++; }
            PlayerSkillSetting(classDict["Perk0_UID"], perk0_Level);
        }
        else if (_skillUID == classDict["Perk1_UID"])
        {
            if(perk1_Level < perkMaxLevel) { perk1_Level++; }
            PlayerSkillSetting(classDict["Perk1_UID"], perk1_Level);
        }
        else if (_skillUID == classDict["Perk2_UID"])
        {
            if (perk2_Level < perkMaxLevel) { perk2_Level++; }
            PlayerSkillSetting(classDict["Perk2_UID"], perk2_Level);
        }
    }


    #endregion


    #region Player의 무기를 변경하는 함수 관련
    void PlayerWeaponChange()
    {
        // 상점에서 구매하려는 무기의 transform을 받아서 처리.
        weaponManager.WeaponChange(classDict["WeaponUID"]);
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        // Player Move 함수 실행
        PlayerMove();


        #region Editor Test Code
#if UNITY_EDITOR
        // 무기 변경 테스트 코드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerClass = PlayerClass.ePlayerClass.Soldier;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerClass = PlayerClass.ePlayerClass.Medic;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerClass = PlayerClass.ePlayerClass.Engineer;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerWeaponChange();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            status0_Level += 1;
            if (perk2_Level == 0)
            {
                perk2_Level += 1;
                Debug.Log("___PERK 2:" + perk2_Level + "___");
            }
            PlayerSkillSetting(classDict["Perk0_UID"], status0_Level);
            PlayerSkillSetting(classDict["Perk1_UID"], status0_Level);
            PlayerSkillSetting(classDict["Perk2_UID"], perk2_Level);
        }

        //// 체력 감소 테스트 코드
        //damageTime += Time.deltaTime;
        //if(damageDelay <= damageTime)
        //{
        //    damageTime -= damageDelay;
        //    Damaged(10f, Vector3.zero, Vector3.zero);
        //}
        ////
#endif
        #endregion

    }

    // 체력 감소 테스트 변수
    //float damageDelay = 5f;
    //float damageTime = 0f;

    private void LateUpdate()
    {
        // Player Rotation 함수 실행
        PlayerRotation();
        // Upper Body Rotation 함수 실행
        UpperBodyRotation();
        // Player Camera Move 함수 실행
        //PlayerCameraMove(); // 기능 제거
    }


    /// <summary>
    /// 플레이어가 앉는 동작을 실행(시도)하는 함수
    /// 사용하지 않는 함수.
    /// </summary>
    private void TryCrouch()
    {
        // Left Ctrl 키가 눌리면 실행, doCrouch가 true일 경우 실행
        if (Input.GetKey(KeyCode.LeftControl) || doCrouch)
        {
            // doCrouch를 false로
            doCrouch = false;
            // 앉아있으면 서고 서있으면 앉게 변경한다.
            isCrouch = !isCrouch;

            // 앉아 있으면
            if (isCrouch)
            {
                // 앉아있을 때의 속도로 설정
                changedSpeed = crouchSpeed;
                //cameraPosition.y -= cameraMoveValue;
            }
            // 서 있으면
            else
            {
                changedSpeed = walkSpeed;
                //cameraPosition.y += cameraMoveValue;
            }

        }
    }

    /// <summary>
    /// 플레이어의 움직임을 조절하는 함수
    /// </summary>
    private void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 왼쪽 shift가 눌리면 달린다.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //// 앉아있으면 일어서게 한다.
            //// 사용하지 않게 되었다.
            //if (isCrouch == true)
            //{
            //    doCrouch = true;
            //    TryCrouch();
            //}
            // 일어서기만 하면 이동 속도가 walkSpeed가 되기때문에
            // 이동속도를 runSpeed로 설정한다.
            changedSpeed = runSpeed;

        }
        // 왼쪽 shift가 눌리지 않았고 일어서있는 경우 이동속도를 walkSpeed로 설정한다.
        else if (isCrouch == false)
        {
            // 걷는 속도로 변경한다.
            changedSpeed = walkSpeed;
        }

        // 이동하는 방향 벡터를 설정.
        Vector3 dir = new Vector3(h, 0f, v);
        // dir을 크기가 1인 벡터로 만든다.
        dir.Normalize();

        playerAnim.SetFloat("Horizontal", dir.x);
        playerAnim.SetFloat("Vertical", dir.z);

        // 플레이어가 움직이는 속도가 변화했을 때 바로 변하는게 아니라
        // 선형 보간을 통해서 천천히 변화한다.
        useSpeed = Mathf.Lerp(useSpeed, changedSpeed, Time.deltaTime * motionChangeSpeed);
        // 사용하는 속도와 목표 속도가 거의 동일하면 useSpeed를 changedSpeed로 맞춰준다.
        if (useSpeed != changedSpeed && Mathf.Abs((useSpeed - changedSpeed) / changedSpeed) <= 0.1f)
        {
            useSpeed = changedSpeed;
        }

        // 이거 찾아봐야 하고
        dir = transform.TransformDirection(dir);

        //dir *= useSpeed;
        dir *= useSpeed * Time.deltaTime;

        // dir의 크기가 0.01보다 작으면 움직이는게 아니고
        // 0.01보다 크면 움직이는 것이다.
        // 위에서 dir을 크기가 1인 벡터로 만들도록 했으므로 방향키를 누르면 일단 움직인다고 판단한다.
        if (dir.magnitude >= 0.01f) { isMove = true; }
        else { isMove = false; }

        // Animation 변경에 필요한 값을 Animator로 Set
        playerAnim.SetBool("IsMove", isMove);
        playerAnim.SetBool("IsCrouch", isCrouch);
        playerAnim.SetFloat("Speed", useSpeed);

        //Debug.Log(dir);
        dir.y = gravity;
        // 이거 찾아봐야 하고
        controller.Move(dir);
    }

    /// <summary>
    /// Player 회전 함수
    /// </summary>
    private void PlayerRotation()
    {
        // 마우스의 회전 값을 입력 (값 >> - 1, 0, 1
        float yRotation = Input.GetAxisRaw("Mouse X");
        // 캐릭터가 회전하는 값을 LookSensitivity를 곱하여 설정
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        // 자기 자신을 기준으로 회전
        tr.Rotate(characterRotationY, Space.Self);

        //myRb.MoveRotation(myRb.rotation * Quaternion.Euler(characterRotationY));
    }

    /// <summary>
    /// 플레이어 상체 회전(위 아래) 함수
    /// </summary>
    private void UpperBodyRotation()
    {
        // 마우스의 회전 값을 입력
        float rotation = Input.GetAxisRaw("Mouse Y");
        // 회전하는 값을 lookSensitivity를 곱하여 설정
        float bodyRotation = rotation * lookSensitivity / 2.32f;

        // 현재 회전 값에서 마우스가 이동한 값만큼 이동
        upperBodyRotation -= bodyRotation;
        // 이동의 최대치와 최소치를 upperBodyRoationLimit로 설정
        upperBodyRotation = Mathf.Clamp(upperBodyRotation, -upperBodyRotationLimit, upperBodyRotationLimit);
        // upperBodyRotation으로 넣으면 상 하 반전 있음.
        playerAnim.SetFloat("Looking", -upperBodyRotation);

        //playerCameraTr.localRotation = Quaternion.Euler(new Vector3(upperBodyRotation * 0.75f, playerCameraTr.rotation.y, playerCameraTr.localRotation.z));
        // 자연스럽게 카메라가 위 아래를 보도록 값을 보정
        playerCameraTr.localRotation = Quaternion.Euler(new Vector3(upperBodyRotation * 0.85f, -12.5f, 0));

    }

    /// <summary>
    /// 플레이어의 카메라가 움직이는 함수(앉았다 일어설 때)<br/>
    /// 기능 제거
    /// </summary>
    private void PlayerCameraMove()
    {
        // Slerp, 구형 보간, Lerp, 선형 보간
        //playerCameraTr.localPosition = Vector3.Lerp(playerCameraTr.localPosition, cameraPosition, Time.deltaTime * cameraMoveSpeed);
    }

    /// <summary>
    /// 피해를 받을 때 호출되는 함수
    /// </summary>
    /// <param name="damage">받는 데미지</param>
    /// <param name="hitPoint">공격 받은 위치</param>
    /// <param name="hitNormal">공격 받은 위치의 노말 벡터</param>
    public override void Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Damaged(damage - addArmour, hitPoint, hitNormal);

        StartCoroutine(ShowBloodScreen()); // 피격시 피격했다는 의미로 붉은 테두리가 깜빡인다.
        HPGaugeChange(); // HP 게이지를 변경시켜준다.
    }


    /// <summary>
    /// 피격시 붉은 테두리가 잠깐 생겼다가 사라진다.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBloodScreen()
    {
        //Debug.Log("____SHOW BLOOD SCREEN____");
        //  BloodScreen 텍스처의 알파값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.25f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        //  BloodScreen 텍스처의 색상을 모두 0으로 변경
        bloodScreen.color = Color.clear;
    }



    //public void Attack()
    //{

    //}

    /// <summary>
    /// 플레이어의 체력이 변경될 경우 체력 게이지를 변경시켜주는 함수<br/>
    /// 체력 숫자 값도 같이 변경시켜준다.
    /// </summary>
    private void HPGaugeChange()
    {
        playerHpImage.fillAmount = currHP / maxHp;
        playerHpText.text = string.Format($"<b>{currHP}</b>");

    }




}
