using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreCtrl : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform originParent;

    // 지금은 상점의 버튼과 Canvas 등을 전부 하나하나 프리팹으로 생성해뒀지만
    // 나중에 JSON 같은 것으로 정리하면 열 때 정보를 가져와서 로드하는 방식을 사용할 수 있을 것이다.

    // 상점에서 판매하는 것들의 종류를 구분한 버튼 리스트
    public List<UnityEngine.UI.Button> typeButtonList;
    // 구분된 종류를 표시할 패널 리스트
    public List<UnityEngine.UI.Image> typePanelList;

    // 무기의 종류를 구분하는 버튼 리스트
    public List<UnityEngine.UI.Button> weaponTypeButtonList;
    // 구분된 패널 리스트
    public List<UnityEngine.UI.Image> weaponTypePanelList;
    // Rifle 리스트
    public List<UnityEngine.UI.Button> rifleList;
    // SMG 리스트
    public List<UnityEngine.UI.Button> smgList;
    // SG 리스트
    public List<UnityEngine.UI.Button> sgList;

    // 아이템의 종류를 구분하는 버튼 리스트
    public List<UnityEngine.UI.Button> itemTypeButtonList;
    // 구분된 아이템 패널 리스트
    public List<UnityEngine.UI.Image> itemTypePanelList;
    // 아이템 리스트
    public List<UnityEngine.UI.Button> itemList;
    // 특전 리스트
    public List<UnityEngine.UI.Button> perkList;

    // 방어 물자의 정류를 구분하는 버튼 리스트
    public List<UnityEngine.UI.Button> defStructTypeButtonList;
    // 구분된 패널 리스트
    public List<UnityEngine.UI.Image> defStructTypePanelList;
    // 방어 물자 리스트
    public List<UnityEngine.UI.Button> defStructList;


    // 판매 목록의 정보를 표시해주는 텍스트
    public UnityEngine.UI.Text infoText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        // 기존에 있었던 위치(상점)을 받아온다.
        originParent = transform.parent.transform;
        // UI가 켜졌으므로 커서 고정을 끈다.
        CursorState.CursorLockedSetting(false);


    }


    /// <summary>
    /// 상점을 닫는 버튼을 눌렀을 때 동작하는 함수
    /// </summary>
    public void OnStoreCloseBtn()
    {
        //Debug.Log("____ Close Button Click ____");

        Transform playerTr = transform.parent.GetComponent<Transform>();
        // Store Canvas를 Store의 자식으로 다시 되돌린다.
        transform.SetParent(originParent);
        // Store가 가지고 있는 Close Store 함수를 실행시킨다.
        originParent.GetComponent<Store>().CloseStore(playerTr);
        // UI가 꺼질 것이므로 커서 고정을 켠다
        CursorState.CursorLockedSetting(true);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        MyButton _myButton = eventData.pointerCurrentRaycast.gameObject.GetComponent<MyButton>();

        Debug.Log("____ On Pointer Click: " + eventData.pointerCurrentRaycast.gameObject.name + " ____");

        switch (_myButton.buttonType)
        {
            // 무기 목록 표시
            case StoreButtonType.ButtonType.WeaponType:
                TypePanelActive(0);
                break;
            // 방어 물자 목록 표시
            case StoreButtonType.ButtonType.DefStructType:
                TypePanelActive(1);
                break;
            // 아이템 목록 표시
            case StoreButtonType.ButtonType.ItemType:
                TypePanelActive(2);
                break;
            // 라이플 목록 표시
            case StoreButtonType.ButtonType.RifleList:
                WeaponPanelActive(0);
                break;
            // 기관단총 목록 표시
            case StoreButtonType.ButtonType.SMGList:
                WeaponPanelActive(1);
                break;
            // 샷건 목록 표시
            case StoreButtonType.ButtonType.SGList:
                WeaponPanelActive(2);
                break;
            // 방어 물자 목록 표시
            case StoreButtonType.ButtonType.DefStructList:
                DefStructPanelActive(0);
                break;
            // 사용 아이템 목록 표시
            case StoreButtonType.ButtonType.ItemList:
                ItemPanelActive(0);
                break;
            // 특전 목록 표시
            case StoreButtonType.ButtonType.PerkList:
                ItemPanelActive(1);
                break;
            // 구매 아이템 버튼 클릭.
            case StoreButtonType.ButtonType.BuyButton:



                break;
            default:

                break;

        }

    }


    #region Panel Active 모음
    // 판매 목록 패널
    private void TypePanelActive(int _idx)
    {
        // 반복문이 돌아가는데 코루틴으로 돌리는게 맞을지도 모르겠다.

        // 위랑 아래 중에 뭐가 더 빠른지는 모르겠다. 위에 어디갔어...
        for (int i = 0; i < typePanelList.Count; i++)
        {
            if (i == _idx)
            {
                typePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                typePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 무기 패널
    private void WeaponPanelActive(int _idx)
    {

        for (int i = 0; i < weaponTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                weaponTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                weaponTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 방어 물자 패널
    private void DefStructPanelActive(int _idx)
    {

        for (int i = 0; i < defStructTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                defStructTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                defStructTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 아이템 패널
    private void ItemPanelActive(int _idx)
    {

        for (int i = 0; i < itemTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                itemTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                itemTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion


    // 마우스를 올렸을 때 정보를 가져오기 위해서 사용
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("___ Pointer Enter: " + eventData.pointerCurrentRaycast.gameObject.name + " ____");
        try
        {
            // 구매할 수 있는 목록의 버튼에 올라가면 그 버튼의 아이템 정보를 가져온다.
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<MyButton>().buttonType == StoreButtonType.ButtonType.BuyButton)
            {
                MyBuyButton buyButton = eventData.pointerCurrentRaycast.gameObject.GetComponent<MyBuyButton>();

                // text에 문자를 저장할 때 string Builder라는 것을 사용하는 것이 좋다고 한다. 후에 찾아보자.
                // 아이템의 정보를 표시해준다.
                infoText.text = buyButton._info;
                infoText.text += $"   가격: {buyButton._price}";
            }

        }
        catch (System.Exception e)
        {
            // 어떤 오브젝트에 닿아서 어떤 에러가 발생했는지 확인
            Debug.LogWarning(eventData.pointerCurrentRaycast.gameObject.name + e.Message);
        }

    }

    // 빈 곳으로 마우스가 옮겼을 때 이전 정보를 초기화하기 위해서 사용
    public void OnPointerExit(PointerEventData eventData)
    {
        try
        {
            infoText.text = "";
        }
        catch(System.Exception e)
        {
            Debug.LogWarning(e);
        }

    }
}
