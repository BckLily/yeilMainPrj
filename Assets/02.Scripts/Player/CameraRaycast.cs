using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    #region LayerMask
    //LayerMask allLayerMask;
    //LayerMask enemyLayer;
    //LayerMask defensiveGoodsLayer;
    //LayerMask storeLayer;
    //LayerMask playerLayer;
    //LayerMask mainDoorLayer;
    //LayerMask wallLayer;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //enemyLayer = LayerMask.NameToLayer("ENEMY");
        //defensiveGoodsLayer = LayerMask.NameToLayer("DEFENSIVEGOODS");
        //storeLayer = LayerMask.NameToLayer("STORE");
        //playerLayer = LayerMask.NameToLayer("PLAYER");
        //mainDoorLayer = LayerMask.NameToLayer("MAINDOOR");
        //wallLayer = LayerMask.NameToLayer("WALL");

        //allLayerMask = (1 << enemyLayer) | (1 << defensiveGoodsLayer) | (1 << storeLayer) | (1 << playerLayer) | (1 << mainDoorLayer);

        //Debug.Log("EnemyLayer: " + enemyLayer.value);
        //Debug.Log("GodsLayer: " + defensiveGoodsLayer.value);
        //Debug.Log("StoreLayer: " + storeLayer.value);
        //Debug.Log("PlayerLayer: " + playerLayer.value);
        //Debug.Log("MainDoorLayer: " + mainDoorLayer.value);
        //Debug.Log("ALL Layer: " + string.Format($"{allLayerMask.value}"));

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 보고 있는 타겟 오브젝트를 그대로 넘겨주는 함수
    /// </summary>
    /// <param name="_raycastRange">Raycast disance</param>
    /// <returns>Return target GameObject<br/>If target is Empty return null </returns>
    /// 매개변수에 Raycast 대상이 될 LayerMask도 같이 받아서 처리해야겠다.
    /// 그래야지 철책뒤의 적에 대해서 인식을 할 수 있을 것이다.
    public GameObject GetRaycastTarget(float _raycastRange, LayerMask targerLayerMasks)
    {
        GameObject target = null;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, _raycastRange, targerLayerMasks))
        {
            target = hit.transform.gameObject;
            //Debug.Log(target.tag);
        }        

        return target;
    }



}
