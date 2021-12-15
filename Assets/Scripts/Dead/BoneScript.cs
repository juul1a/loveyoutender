using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScript : MonoBehaviour
{
    private Vector3 moveDir;
    [SerializeField] private float speed;

   [SerializeField] private float overTime = 1f;

   private float time = 0f;
   private Vector3 movementVel;

    private Vector3 cameraPos, mouseDir;

    private Quaternion slerpTo, slerpFrom;
    
    [SerializeField] private float maxMinDegrees = 90f;
    private float addDouble;

    private bool started = false;

    public float delay = 0f;

    public bool move = false;

    // Start is called before the first frame update
    void Start()
    {
        addDouble = maxMinDegrees*2;

        slerpTo =  Quaternion.Euler(0, 0, maxMinDegrees);
        slerpFrom = transform.localRotation;

        Invoke("StartNow", delay);
        //InvokeRepeating("SlerpChange", frequency, frequency);
        // slerpDown =  Quaternion.Euler(0, 0, -90);

    }

    // Update is called once per frame
    void Update()
    {
        // transform.position+= (root.localRotation * Vector3.down * speed);
        if(started){
            if(move){
                transform.position += (transform.localRotation * Vector3.down * speed);
            }
            if(time>=2.0f){
                SlerpChange();
            }
            SinFlippyFloppy();
        }
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
        transform.localRotation = Quaternion.Slerp (slerpFrom, slerpTo, time);

        // target.localPosition = target.localPosition + (new Vector3(0,1,0) * Mathf.Cos(Time.time * frequency) * magnitude);
	}

    void SlerpChange(){
        time=0f;
        addDouble *= -1;
        maxMinDegrees += addDouble;
        slerpTo =  Quaternion.Euler(0, 0, maxMinDegrees);
        slerpFrom = transform.localRotation;
        Debug.Log("Slerpin to "+maxMinDegrees);
    }

    void StartNow(){
        started = true;
    }
}
