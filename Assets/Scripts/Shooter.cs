using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public GameObject bullet;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Shoot", fireRate);
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        Invoke("Shoot", fireRate);
    }
}
