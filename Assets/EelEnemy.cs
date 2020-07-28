using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelEnemy : MonoBehaviour
{

    private Transform root;
    private Transform target;
    private Vector3 moveDir;
    [SerializeField] private float speed, frequency, magnitude;

   [SerializeField] private float overTime = 0.5f;
   private Vector3 movementVel;

    private Vector3 cameraPos, mouseDir;

    // Start is called before the first frame update
    void Start()
    {
        root = transform;//gameObject.transform.Find("bone_1");
        target = gameObject.transform.Find("Target");

    }

    // Update is called once per frame
    void Update()
    {
        
        // cameraPos = Camera.main.WorldToScreenPoint(transform.position);
        // mouseDir = Input.mousePosition - cameraPos;
        // float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        // Debug.Log("Rotating by angle "+angle);
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.position = Vector3.SmoothDamp(transform.position, mouseDir, ref movementVel, overTime);
        
        SinFlippyFloppy();
    }

    void SinFlippyFloppy(){
		root.localPosition = root.localPosition + (new Vector3(0,1,0) * Mathf.Sin(Time.time * frequency) * magnitude);
        root.Rotate(new Vector3(0,0,2) * Mathf.Sin(Time.time * frequency) * magnitude); 

        // target.localPosition = target.localPosition + (new Vector3(0,1,0) * Mathf.Cos(Time.time * frequency) * magnitude);
	}
}
