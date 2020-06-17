using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float swimForce = 2.0f;
    private bool dead = false;
    [SerializeField]
    private float maxSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }


	void InputHandler(){
		if(!dead){
			if(Input.GetKeyDown(KeyCode.Space)){
                if(rb.simulated == false)
                {
                    rb.simulated = true;
                }
                Debug.Log("Velocity = "+rb.velocity.y);
                if(rb.velocity.y < maxSpeed){
                    rb.AddForce(new Vector3(0,1,0) * swimForce);
                }
			}
		}
	}
}
