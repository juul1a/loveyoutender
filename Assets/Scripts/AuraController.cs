using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{
    public float timeout = 2;

    void Start(){
        Invoke("Die", timeout);
    }

    void Die(){
        Debug.Log("Dying");
        Destroy(gameObject);
    }
}
