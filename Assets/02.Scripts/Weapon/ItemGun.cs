using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Guns 스크립트와 합쳐도 된다.
// Scriptable Object를 사용하지 않은 이유는 고정된 수치를 직접 입력해야 하기 때문.
// Asset Menu 생성 / 파일 이름 / 메뉴 이름 / 메뉴에서 위치
[CreateAssetMenu(fileName ="New Gun", menuName ="New Gun/Gun", order =int.MaxValue)]
public class ItemGun : ScriptableObject
{
    // 새로 클래스를 만드는게 나았을 것 같네
    // 무기 종류 (Enum)
    public enum GunType
    {
        Rifle= 0, SMG=1, SG =2,
    }

    // 무기 레어도 (Enum)
    public enum GunRarity
    {
        Common =0, Rare=1, Epic=2,
    }
    
    // 프로퍼티는 대문자로 작성하는게 맞나봄.
    // 대문자로 시작해야한다고 표시가 계속 나오네
    public GunType gunType { get; private set; } // 무기 종류 변수
    public GunRarity gunRarity { get; private set; } // 무기 레어도 변수

    public int reloadBullet { get; private set; } // 재장전 총알 수
    public int maxBullet { get; private set; } // 최대 들고 있을 수 있는 총알 수

    public float damage { get; private set; } // 총 공격력
    public float fireDelay { get; private set; } // 총 발사 딜레이
    public float reloadTime { get; private set; } // 총 재장전 시간

    public string gunName { get; private set; } // 총의 이름

    //public GameObject gunPrefab;  // 총의 프리팹 (사용될 프리팹)
    //public Sprite gunImage; // 총 이미지 (상점에 사용될 이미지)


}
