using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelEnemy : MonoBehaviour
{

    private Transform root;
    private Transform target;
    private Vector3 moveDir;
    [SerializeField] private float speed;

   [SerializeField] private float overTime = 1f;

   private float time = 0f;
   private Vector3 movementVel;

    private Vector3 cameraPos, mouseDir;

    private Quaternion slerpTo, slerpFrom;
    
    [SerializeField] private float maxMinDegrees = 90f;
    private float addDouble;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        addDouble = maxMinDegrees*2;
        root = gameObject.transform.Find("bone_1");

        slerpTo =  Quaternion.Euler(0, 0, maxMinDegrees);
        slerpFrom = root.transform.localRotation;
        //InvokeRepeating("SlerpChange", frequency, frequency);
        // slerpDown =  Quaternion.Euler(0, 0, -90);

    }

    // Update is called once per frame
    void Update()
    {
        // transform.position+= (root.localRotation * Vector3.down * speed);
        
        //  Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        //  transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        root.transform.position += (root.transform.right * speed *-1);
        if(time>=1.5f){//half second between each turn
            SlerpChange();
        }
        // cameraPos = Camera.main.WorldToScreenPoint(transform.position);
        // mouseDir = Input.mousePosition - cameraPos;
        // float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        // Debug.Log("Rotating by angle "+angle);
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.position = Vector3.SmoothDamp(transform.position, mouseDir, ref movementVel, overTime);
        
        SinFlippyFloppy();
    }

    void SinFlippyFloppy(){
		// root.localPosition = root.localPosition + (new Vector3(0,1,0) * Mathf.Sin(Time.time * frequency) * magnitude);
        // int multiplier = 1;
        // if(Mathf.Cos(Time.time)<0){
        //     multiplier = -1;
        // }
        // Debug.Log("rotating by angle "+ Mathf.Rad2Deg*(Mathf.Acos(Mathf.Sin(Time.time * frequency) * magnitude)* multiplier));
        // root.Rotate(new Vector3(0,0,1) * Mathf.Acos(Mathf.Sin(Time.time * frequency) * magnitude)* multiplier); 
        time += Time.deltaTime / overTime;
        root.localRotation = Quaternion.Slerp (slerpFrom, slerpTo, time);

        // target.localPosition = target.localPosition + (new Vector3(0,1,0) * Mathf.Cos(Time.time * frequency) * magnitude);
	}

    void SlerpChange(){
        time=0f;
        addDouble *= -1;
        maxMinDegrees += addDouble;
        slerpTo =  Quaternion.Euler(0, 0, maxMinDegrees);
        slerpFrom = root.transform.localRotation;
        Debug.Log("Slerpin to "+maxMinDegrees);
    }
}
