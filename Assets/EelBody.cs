using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelBody : MonoBehaviour
{
   private int myOrder;
   [SerializeField] private Transform head, prevPart;
   [SerializeField] private float overTime = 0.5f;
   private Vector3 movementVel;


   void Update(){
    //    transform.position = Vector3.SmoothDamp(transform.position, prevPart.position, ref movementVel, overTime);
    //    float angle = Mathf.Atan2(prevPart.position.y, prevPart.position.x) * Mathf.Rad2Deg;
        float angle = prevPart.rotation.z*Mathf.Rad2Deg;
        // Debug.Log("angle of "+prevPart.name+" = "+angle);
        Quaternion angleQuat = Quaternion.Euler(0, 0, angle);
        // Debug.Log("Rotating by angle "+angle);
        // Debug.Log("prevpart rotation is "+prevPart.rotation);
        // transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis(angle, new Vector3(0,0,1)), Time.deltaTime*overTime*2);
        transform.rotation = Quaternion.Slerp (transform.rotation, angleQuat, Time.deltaTime*overTime*2);
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
       //Debug.Log("Lookat Z = "+lookAt);
   }
}
