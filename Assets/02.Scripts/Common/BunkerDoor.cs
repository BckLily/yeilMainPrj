using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : DefensiveStructure
{
    [SerializeField]
    private UnityEngine.UI.Image bunkerInfoPanel;

    protected override void Awake()
    {
        base.Awake();
        startHp = 250f;
    }

    // Start is called before the first frame update
    void Start()
    {
        bunkerInfoPanel = GameObject.Find("StagePanel").transform.Find("InfomationPanel").GetComponent<UnityEngine.UI.Image>();
    }

    protected override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Damaged(10f, Vector3.zero, Vector3.zero);
        //}
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currHP = startHp;
        //Debug.Log($"____ Bunker Hp {currHP} ____");
    }

    public override void OnDeath()
    {
        GameManager.instance.GameOver();

    }

    public override float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Damaged(damage, hitPoint, hitNormal);
        //Debug.Log($"____ Bunker Curr Hp: {this.currHP} ____");

        StartCoroutine(BunkerDamaged());

        return 0;
    }

    IEnumerator BunkerDamaged()
    {
        // 이미 공격을 받고 있다는 패널이 활성화 중이면 코루틴을 끝낸다.
        if (bunkerInfoPanel.gameObject.activeSelf) { yield break; }

        bunkerInfoPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        bunkerInfoPanel.gameObject.SetActive(false);

    }


}
