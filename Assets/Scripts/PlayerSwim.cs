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
    private float maxVerticalSpeed, movementSpeed, rotationSpeed;
    private Quaternion originalRotation;
    private Transform spriteHolder;

    private Animator anim;

    private Quaternion leftRot, rightRot;
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = false;

        anim = GetComponent<Animator> ();
		if(anim == null){
			anim = GetComponentsInChildren<Animator>()[0];
		}
        spriteHolder = transform;//.GetChild(0);
        originalRotation = spriteHolder.rotation;

        leftRot = Quaternion.Euler(0, 0, 45);
        rightRot = Quaternion.Euler(0, 0, -45);
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
                    rb.AddRelativeForce(Vector3.up * swimForce);
                    anim.SetTrigger("Swim");
                }
			}
            if(Input.GetKey("a")){
                spriteHolder.rotation = Quaternion.Slerp (spriteHolder.rotation, leftRot, Time.deltaTime * rotationSpeed);
            }
            else if(Input.GetKey("d")){
                spriteHolder.rotation = Quaternion.Slerp (spriteHolder.rotation, rightRot, Time.deltaTime * rotationSpeed);
            }
            else{
                spriteHolder.rotation = Quaternion.Slerp (spriteHolder.rotation, originalRotation, Time.deltaTime*rotationSpeed*2);
            }
            // float horizontal = Input.GetAxis ("Horizontal");
            // rb.velocity = new Vector2(horizontal*movementSpeed, rb.velocity.y); // x = -1, y = 0
		}
	}
}
