using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public ItemGun itemGun; // 총 프리팹을 만들기 위한 정보를 가지고 있는 ScriptableObject Script

    public Transform firePos; // 총구 Transform

    public Transform handleTr; // 오른손이 잡고 있어야 하는 손잡이 위치

    [SerializeField]
    private Transform playerTr; // 플레이어 오브젝트의 Transform

    public ItemGun.GunRarity gunRarity { get; private set; } // 무기의 레어도
    public ItemGun.GunType gunType { get; private set; } // 무기의 타입

    public int currBullet { get; set; } // 현재 장전된 총알 수
    public int reloadBullet { get; private set; } // 재장전 총알 수
    public int carryBullet { get; set; } // 현재 들고 있는 총알 수
    public int maxBullet { get; private set; } // 최대 들고 있을 수 있는 총알
    public float damage { get; private set; } // 총의 공격력
    [SerializeField]
    public float fireDelay { get; private set; } // 총의 발사 딜레이
    public float fireTime; // 총을 쏘고 지난 시간

    public int reloadTime { get; private set; } // 총의 재장전 시간
    public string gunName { get; private set; } // 무기의 이름


    // Start is called before the first frame update    
    private void Awake()
    {
        currBullet = itemGun.reloadBullet; // 시작시 총알 수는 재장전 총알 수와 동일하다
        carryBullet = itemGun.maxBullet - itemGun.reloadBullet; // 시작시 가지고 있는 여유 총알 수는 최대 총알 수에서 재장전 수를 뺀 수이다.
    }


    private void OnEnable()
    {
        playerTr = transform.parent.parent.GetComponent<Transform>(); // player의 Transform


    }


    private void Start()
    {
        fireDelay = itemGun.fireDelay;
        fireTime = fireDelay; // 총을 바로 발사할 수 있게 한다.
        damage = itemGun.damage; // 총의 공격력.
    }


    // Update is called once per frame
    void Update()
    {

    }


    // 발사 조건의 확인은 WeaponManager에서 한다.
    // 발사 동작을 하면 WeaponManager에서 BulletRaycast()를 시행한다.
    public void BulletRaycast()
    {

    }


}
