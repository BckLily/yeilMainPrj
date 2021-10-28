using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectCtrl : MonoBehaviour
{
    private float removeTime;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        removeTime = 0.35f;
    }

    private void OnEnable()
    {
        _time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Invoke를 사용하는 방법도 있었을 듯?
        if (_time >= removeTime)
        {
            _time = 0f;
            PlayerEffectCtrl.ReturnBloodEffect(this);
        }
        else
        {
            _time += Time.deltaTime;
        }
    }

}
