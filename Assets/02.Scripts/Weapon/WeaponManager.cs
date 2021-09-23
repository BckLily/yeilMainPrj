using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Transform playerCameraTr; // 플레이어 카메라 Transform
    public Transform leftHandTr; // 플레이어 왼손 위치
    private Transform weaponTr; //weapon Transform

    public CameraRaycast cameraRaycast; // 플레이어 카메라의 CameraRaycast

    private GameObject currWeapon; // 현재 총
    private Guns currGun; // 현재 총이 가지고 있는 Gun Script

    private bool isReload; // 플레이어가 재장전을 하고 있는가?

    private Dictionary<string, string> weaponDict = null; // 현재 무기의 정보를 저장하고 있는 dictionary
    private string weaponPath; // Resources폴더의 weapon이 있는 Path.

    public Animator anim;

    bool isFire = false; // 플레이어가 총을 발사하면 true가 된다.

    [Space(5)]
    [Header("Weapon Info")]
    [Space(2)]
    public Text weaponBulletText; // 현재 무기의 잔탄 텍스트
    public Text weaponNameText; // 현재 무기의 이름 텍스트


    private void Awake()
    {
        weaponPath = "Weapons/";
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponTr = GetComponent<Transform>(); // 무기의 위치

        isReload = false; // 재장전 상태 false
                          //WeaponChange(string.Format("00000000")); // 무기 변경(기본 무기 설정)


    }

    // Update is called once per frame
    void Update()
    {
        //weaponTr.forward = playerCameraTr.forward; // 총의 정면과 플레이어 카메라의 정면을 동일하게 설정(어색하지 않게)
        TryReload(); // 재장전 시도 함수
        TryFire(); // 발사 시도 함수

        try
        {
            weaponTr.LookAt(leftHandTr);
        }
        catch (Exception e)
        {

        }
    }

    #region 무기 변경
    /// <summary>
    /// 무기 변경에 사용하는 함수
    /// </summary>
    /// <param name="UIDCode">무기 UID</param>
    public void WeaponChange(string UIDCode)
    {
        //Debug.Log(UIDCode);
        // UID 코드에 맞게 무기 가져옴.
        // UID 코드를 넘겨줬으니까 UID 코드에 맞는 무기를 DBManager를 통해서 찾고
        // 거기의 Weapon_Name을 사용해서 Prefab을 찾아야 한다.
        Dictionary<string, string> _weaponDict = null;
        while (_weaponDict == null)
        {
            _weaponDict = DBManager.Instance.GetWeaponInfo(UIDCode);
        }
        weaponDict = _weaponDict;

        // 현재 가지고 있는 무기가 있을 경우 무기를 제거하고
        if (currWeapon != null)
        {
            Destroy(currWeapon.gameObject);
        }

        //Debug.Log(weaponPath + weaponDict["Weapon_Name"]);
        // 새로 생성한 무기를 현재 무기로 만들어준다.
        currWeapon = (GameObject)Instantiate(Resources.Load(weaponPath + weaponDict["Weapon_Name"]), this.transform);

        //currWeapon = weaponTr.GetChild(0).gameObject;
        //Debug.Log(currWeapon.name);
        // 현재 무기의 Guns 컴포넌트를 받아온다.
        currGun = currWeapon.GetComponent<Guns>();

        // 현재 무기의 rotation을 현재 weaponPos의 rotation으로 맞춰준다.(왼손쪽을 보는 회전)
        currWeapon.transform.rotation = weaponTr.rotation;
        //currWeapon.transform.Translate(weaponTr.localPosition - currGun.handleTr.localPosition);
        // 현재 총의 손잡이 부분을 오른손의 중앙에 맞게 이동
        currWeapon.transform.Translate(-currGun.handleTr.localPosition);

        //weaponTr.LookAt(leftHandTr);
        // 받아온 무기 정보 딕셔너리를 현재 총에 넘겨준다.
        currGun.weaponDict = this.weaponDict;

        WeaponNameChange();
        WeaponBulletChange();
    }

    #endregion

    #region 재장전
    /// <summary>
    /// 재장전 시도 함수
    /// </summary>
    private void TryReload()
    {
        // 재장전을 하는 중이면
        if (isReload == true)
        {
            return;
        }
        // R키를 눌렀을 경우 재장전 시도
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        // 현재 총알 수가 재장전 총알 수보다 작은 경우
        if (currGun.currBullet < currGun.reloadBullet)
        {
            //Debug.Log("Reload");
            // 가지고 있는 총알이 0발보다 많은 경우
            if (currGun.carryBullet > 0)
            {
                isFire = false;
                anim.SetBool("IsFire", isFire);
                //Debug.Log("Reload");
                // isReload를 true로 변경
                isReload = true;
                // 재장전 동작이 끝날 때까지 기다린다.
                yield return new WaitForSeconds(currGun.reloadTime);

                // 보유한 총알에 현재 총알만큼 더한 다음 재장전 총알만큼 빼준다.
                currGun.carryBullet += (currGun.currBullet - currGun.reloadBullet);
                // 현재 총알을 재장전 총알로 한다.
                currGun.currBullet = currGun.reloadBullet;

                // 총 탄 관련 UI 갱신

            }
        }

        // 재장전을 하고 있는 상태가 아니다.
        isReload = false;
        //WeaponBulletChange();
    }

    #endregion


    #region 발사
    /// <summary>
    /// 발사를 시도하는 함수
    /// </summary>
    private void TryFire()
    {
        // 미완성 상태일 때 계속 에러가 나서 에러 발생시 그냥 함수 종료하게 함.

        // 재장전 중이 아닐 때
        if (isReload == false)
        {
            try
            {
                // currGun이 null일 경우 NullException Error가 발생한다.
                // 그러면 catch로 이동해서 함수를 종료.
                // 총을 쏘고나서 지난 시간은 계속 증가한다.
                currGun.fireTime += Time.deltaTime;
            }
            catch (Exception e)
            {
                //Debug.LogWarning(e);
                return;
            }

            // 마우스 좌클릭을 누른 경우
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Try Fire");
                // 총을 쏠 수 있는 상황이면 >> 첫 발사
                if (CheckCanFire())
                {
                    // fireTime을 0으로 초기화해서 fireTime을 다시 계산하게 한다.
                    currGun.fireTime = 0f;
                }
            }
            // 마우스 좌클릭이 유지된 경우 >> 연속적인 발사
            else if (Input.GetMouseButton(0))
            {
                // 총을 쏠 수 있는 상황이면
                if (CheckCanFire())
                {
                    // fireTime에서 delay만큼 감소시켜 공격 속도보다 빠른 발사를 할 수 없게 한다.
                    currGun.fireTime = 0f;

                }
            }
            // 마우스 좌클릭을 떼었을 경우
            else if (Input.GetMouseButtonUp(0))
            {
                // 발사가 끝났으므로 isFire은 false
                isFire = false;
                // animation에 bool값을 설정
                anim.SetBool("IsFire", isFire);
                // fireTime을 
                currGun.fireTime = currGun.fireDelay;
            }
        }
        WeaponBulletChange();
    }

    /// <summary>
    /// 발사를 시도했을 때 발사할 수 있는 상황인지 판단하는 함수
    /// </summary>
    /// <returns>true: 가능 false: 불가능</returns>
    private bool CheckCanFire()
    {
        bool canFire = false;

        // 현재 장전된 총알이 1발 이상인 경우
        if (currGun.currBullet > 0)
        {
            // 총 발사 딜레이보다 총을 쏘고 지난 시간이 더 크거나 같을 경우
            if (currGun.fireDelay <= currGun.fireTime)
            {
                isFire = true; // 총 발사가 가능한 경우
                anim.SetBool("IsFire", isFire);
                canFire = true;
                // 총 발사를 진행
                currGun.currBullet -= 1;
                // 발사 이펙트 생성
                currGun.BulletFire();
                // 레이 캐스트를 확인해서 판정하는 함수
                CheckFireRaycast();
            }
            else
            {
                isFire = false;
                anim.SetBool("IsFire", isFire);
            }
        }
        else
        {
            isFire = false; // 발사 불가능한 경우
            anim.SetBool("IsFire", isFire);
            // 장전된 총알이 0발인 경우 재장전을 수행한다.
            // 재장전 중이 아니기 때문에 재장전 중이라는 것을 알려주고,
            // 재장전 코루틴을 시작하면 된다.
            isReload = true;
            StartCoroutine(Reload());
        }


        return canFire;
    }


    private void CheckFireRaycast()
    {
        // 무기 사거리 내의 타겟 정보를 가져온다.
        GameObject target = cameraRaycast.GetRaycastTarget(currGun.attackDistance);
        // Raycast 했을 때 대상이 무엇인가
        if (target.CompareTag("ENEMY"))
        {

        }
        else if (target.CompareTag("WALL"))
        {

        }


    }

    /// <summary>
    /// 무기의 이름을 설정하여 UI에 표시하는 함수
    /// </summary>
    private void WeaponNameChange()
    {
        weaponNameText.text = weaponDict["Weapon_Name"];
    }

    /// <summary>
    /// 무기의 총알 수를 UI에 표시하는 함수
    /// </summary>
    private void WeaponBulletChange()
    {
        weaponBulletText.text = string.Format($"<b>{currGun.currBullet} / {currGun.carryBullet}</b>");
    }




    #endregion



}
