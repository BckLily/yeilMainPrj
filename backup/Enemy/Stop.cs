using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{
    public Rigidbody rigid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BUNKERDOOR") || other.CompareTag("DEFENSIVEGOODS"))
        {
            rigid.velocity = Vector3.zero;
        }
    }
}
