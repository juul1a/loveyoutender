using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float fireRate = 0;
	float timeToFire = 0;
	Transform firePoint;
	public GameObject spelloPrefab;
	// private StateManager stateManager;


	//[System.Serializable]
    //public class Spell
    //{
		public float castRange = 30;
		float timeToCast;
		public float castRate;
		public float spellDamage = 10;
		public GameObject auraPrefab;
		public Color auraColour;
		public float spellDuration = 5;
    //}
	//public Spell[] Spells;

	private Player player;

	// Use this for initialization
	void Start () {
		firePoint = transform.Find("FirePoint");
		if(firePoint == null){
			// Debug.LogError("No Firepoint Object found");
		}
		player = gameObject.GetComponentInParent<Player>();
		// stateManager = player.stateManager;
	}
	
	// Update is called once per frame
	void Update () {
		//Shoot();
		//Single burst
		if(fireRate == 0){
			if(Input.GetButtonDown("Fire1")){
				Shoot();
			}
		}
		if(castRate == 0){
			if(Input.GetButtonDown("Fire2")){
				Enemy baddie = BaddieInRange();
				if(baddie != null){
					Cast(baddie);
				}
			}
		}
		//Automatic
		else{
			if(Input.GetButtonDown("Fire1") && Time.time > timeToFire){
				timeToFire = Time.time + 1/fireRate;
				Shoot();
			}
			if(Input.GetButtonDown("Fire2") && Time.time > timeToCast){
				timeToCast = Time.time + 1/castRate;
				Enemy baddie = BaddieInRange();
				if(baddie != null){
					Cast(baddie);	
				}
			}
		}
	}

	Enemy BaddieInRange(){
		RaycastHit2D baddieHit = Physics2D.CircleCast(transform.position, castRange, Vector3.right);
		if(baddieHit.transform.tag == "Enemy") {
			bool infrontBaddieLeft = player.facingRight && (baddieHit.transform.position.x - transform.position.x ) > 0 ;
			bool infrontBaddieRight = !player.facingRight && (transform.position.x - baddieHit.transform.position.x) > 0;
			bool attackRange = Mathf.Abs(baddieHit.transform.position.x - transform.position.x)<=castRange;
			if((infrontBaddieLeft || infrontBaddieRight) && attackRange){
				// Debug.Log("Baddie is in range!");
				Enemy enemy = baddieHit.transform.gameObject.GetComponentInParent<Enemy>();
				return enemy;
			}
		}
		// Debug.Log("Baddie is NAHT in range!");
		return null;
	}

	void Shoot(){
		if(player.stateManager.IsPlaying()){
			Instantiate(spelloPrefab, firePoint.position, firePoint.rotation);
		}
		
		
	
		
		
		//Projects screen co-ordinates of mouse into world position
		//Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		//RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit); //params are Origin, directio (= final point - origin), distance, layermask of what not to hit
		//Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.black);
		//if(hit.collider != null){
		//	 Debug.DrawLine(firePointPosition, hit.point, Color.red);
		//}
	}

	void Cast(Enemy baddie){
		
		baddie.TakeDamage(spellDamage);
		Vector3 auraPos = new Vector3(baddie.transform.position.x, baddie.transform.position.y-7, baddie.transform.position.z);
		GameObject aura = Instantiate(auraPrefab, auraPos, baddie.transform.rotation);
		AuraController auraCon = aura.GetComponent<AuraController>();
		auraCon.timeout = spellDuration;
		aura.transform.parent = baddie.transform;
		// Debug.Log("Casting");
	}
}
