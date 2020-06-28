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
    private float maxVerticalSpeed, movementSpeed;


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
                if(rb.velocity.y < maxVerticalSpeed){
                    rb.AddForce(new Vector3(0,1,0) * swimForce);
                }
			}
            float horizontal = Input.GetAxis ("Horizontal");
            rb.velocity = new Vector2(horizontal*movementSpeed, rb.velocity.y); // x = -1, y = 0
		}
	}
}
