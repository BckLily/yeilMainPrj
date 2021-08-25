using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform playerCameraTr; // 플레이어 카메라 Transform
    private Transform weaponTr; //weapon Transform

    private GameObject currWeapon; // 현재 총
    private Guns currGun; // 현재 총이 가지고 있는 Gun Script

    private bool isReload; // 플레이어가 재장전을 하고 있는가?



    // Start is called before the first frame update
    void Start()
    {
        weaponTr = GetComponent<Transform>(); // 무기의 위치

        isReload = false; // 재장전 상태 false
        WeaponChange(string.Format("00000000")); // 무기 변경(기본 무기 설정)



    }

    // Update is called once per frame
    void Update()
    {
        weaponTr.forward = playerCameraTr.forward; // 총의 정면과 플레이어 카메라의 정면을 동일하게 설정(어색하지 않게)
        TryReload(); // 재장전 시도 함수
        TryFire(); // 발사 시도 함수
    }

    private void WeaponChange(string UIDCode)
    {
        // UID 코드에 맞게 무기 가져옴.
        currWeapon = weaponTr.GetChild(0).gameObject;
        Debug.Log(currWeapon.name);
        currGun = currWeapon.GetComponent<Guns>();
    }

    private void TryReload()
    {
        if (isReload == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private void TryFire()
    {
        // 총을 쏘고나서 지난 시간은 계속 증가한다.
        currGun.fireTime += Time.deltaTime;

        // 재장전 중이 아닐 때
        if (isReload == false)
        {
            // 마우스 좌클릭을 누른 경우
            if (Input.GetMouseButtonDown(0))
            {
                // 총을 쏠 수 있는 상황이면 >> 첫 발사
                if (CheckFire())
                {
                    // fireTime을 0으로 초기화해서 fireTime을 다시 계산하게 한다.
                    currGun.fireTime = 0f;
                }
            }
            // 마우스 좌클릭이 유지된 경우 >> 연속적인 발사
            else if (Input.GetMouseButton(0))
            {
                // 총을 쏠 수 있는 상황이면
                if (CheckFire())
                {
                    // fireTime에서 delay만큼 감소시켜 공격 속도보다 빠른 발사를 할 수 없게 한다.
                    currGun.fireTime -= currGun.fireDelay;

                }
            }
        }
    }

    IEnumerator Reload()
    {
        // 현재 총알 수가 재장전 총알 수보다 작은 경우
        if (currGun.currBullet < currGun.itemGun.reloadBullet)
        {
            // 가지고 있는 총알이 0발보다 많은 경우
            if (currGun.carryBullet > 0)
            {
                Debug.Log("Reload");
                // isReload를 true로 변경
                isReload = true;
                // 재장전 동작이 끝날 때까지 기다린다.
                yield return new WaitForSeconds(currGun.itemGun.reloadTime);

                // 보유한 총알에 현재 총알만큼 더한 다음 재장전 총알만큼 빼준다.
                currGun.carryBullet += (currGun.currBullet - currGun.itemGun.reloadBullet);
                // 현재 총알을 재장전 총알로 한다.
                currGun.currBullet = currGun.itemGun.reloadBullet;

                // 총 탄 관련 UI 갱신

            }
        }

        // 재장전을 하고 있는 상태가 아니다.
        isReload = false;


    }
    private bool CheckFire()
    {
        bool canFire = false;

        // 현재 장전된 총알이 1발 이상인 경우
        if (currGun.currBullet > 0)
        {
            // 총 발사 딜레이보다 총을 쏘고 지난 시간이 더 크거나 같을 경우
            if (currGun.fireDelay <= currGun.fireTime)
            {
                canFire = true;
                // 총 발사를 진행
                currGun.currBullet -= 1;
                currGun.BulletRaycast();

                // 발사 애니메이션 및 Effect Sound 등이 실행될 장소
            }
        }
        else
        {
            // 장전된 총알이 0발인 경우 재장전을 수행한다.
            // 재장전 중이 아니기 때문에 재장전 중이라는 것을 알려주고,
            // 재장전 코루틴을 시작하면 된다.
            isReload = true;
            StartCoroutine(Reload());
        }

        return canFire;
    }



}
