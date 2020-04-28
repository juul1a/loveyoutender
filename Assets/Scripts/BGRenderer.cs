using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class BGRenderer : MonoBehaviour {

    [SerializeField]
	private bool turnedOff;

	[SerializeField]
	private bool followPlatforms;

	//Whether or not to (mildly)scale the assets
	[SerializeField]
	private bool scalability;

	//Whether or not to rotate assets (full 360)
	[SerializeField]
	private bool rotatability;

	//Whether or not to make all assets the same size
	[SerializeField]
	private bool cohesiveSizing;

	//Whether or not to reflect the sprite horizontally
	[SerializeField]
	private bool horizontalReflect;

	//Whether or not to reflect the sprite vertically
	[SerializeField]
	private bool verticalReflect;

	// Sorting layer of the objs
	[SerializeField]
	private string bgSortingLayer;

	//all sprites in map
	private Sprite[] bgSprites;

	//All background objects prefabs
	[SerializeField]
	private GameObject[] bgObjs;

	//All background objects we make
	private GameObject[] bgObjsBuilt;

	//start position of background
	private Vector2 bgOrigin;

	//end position of background
	[SerializeField]
	private int bgTerminateX;
	private Vector2 bgTerminate;

	//Max and min areas for sprites on sheet
	private float spriteBigArea, spriteSmallArea;

	//How many background objects we should make
	[SerializeField]
	private int numBgObjs = 40;

	[SerializeField]
	private int maxSpriteY;

	//bg object prefab
	[SerializeField]
	private GameObject bgObject;

	//z value
	[SerializeField]
	private int layer;

	[SerializeField]
	private string assetPath;


	/* PARALLAX FIELDS */
	[SerializeField]
	public float parallaxSpeed;

	private Transform cameraTransform;
	private Transform[] bgObjects;
	private float cameraX;
	private float cameraY;

	private bool running = false;

	public void Awake(){
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	public void Run(){
		if(!turnedOff){
			running = true;
			//we need to know where all of the platforms are so we can place the background objects accordingly.
			PlatformLoader plLoader = gameObject.GetComponentInParent<PlatformLoader>();
			List<Vector2> allPlatforms = plLoader.GetPlatformPositions();
			int closest = 0; //closest platform when we start will be the first one


			// bgSprites = Resources.LoadAll<Sprite>(assetPath); //This is going to have to be done in a better way. Name of the folder in Resources to pull
			if (cohesiveSizing) {
				//prepareCohesiveSizing ();
			}

			bgObjsBuilt = new GameObject[numBgObjs];
			bgOrigin = new Vector2 (-100, transform.position.y);
			bgTerminate = new Vector2 (bgTerminateX, -10);

			float iter = (bgTerminate.x - bgOrigin.x) / numBgObjs; //Length of each background "slice"
			float minX = bgOrigin.x; //Minimum x position to place the bgobj
			float maxX, setX, setY; //max xpos and Actual x and y positions of the bgobj

			for (int i = 0; i < numBgObjs; i++) {
				maxX = minX + iter; //(bgTerminate.x/4) + minX;
				int k = Random.Range (0, bgObjs.Length-1);//determines which sprite to uses
				bgObject = bgObjs[k];
				bgObjsBuilt[i] = Instantiate (bgObject);
				bgObjsBuilt[i].transform.SetParent(transform, true);


				SpriteRenderer bgRenderer = bgObjsBuilt[i].GetComponent<SpriteRenderer>();
				// int j = Random.Range (0, bgSprites.Length);//determines which sprite to uses

				// bgRenderer.sprite = bgSprites[j];
				Transform bgTransform = bgObjsBuilt[i].GetComponent<Transform> ();

				//MAP position
				setX = Random.Range (minX, maxX);
				if(followPlatforms){
					//Compare x of sprite to closest platform
					Vector2 currentClosest = allPlatforms[closest];
					Vector2 nextClosest = allPlatforms[closest+1];

					float distCurrClosest = Mathf.Abs(currentClosest.x - setX);
					float distNextClosest = Mathf.Abs(nextClosest.x-setX);
					if(distNextClosest < distCurrClosest){
						closest++;
					}

					bgOrigin.y = allPlatforms[closest].y;
				}

				setY = bgOrigin.y; // + (bgRenderer.sprite.bounds.size.y); //bottom of the bgOrigin plus half the size of the sprite's height
				if (maxSpriteY > 0) {
					setY = Random.Range (setY-maxSpriteY, setY+maxSpriteY);
				}
				bgTransform.position = new Vector3 (setX, setY, layer); 
				bgRenderer.sortingLayerName = bgSortingLayer;
				bgRenderer.sortingOrder = layer;


				//Sprite transformations
				//i think i have to pass the single sprite back and forth between all the functions
				//original sprite is bgSprites[j]
				//sprite renderer is bgRenderer.sprite though and thats what i do the transforms to i think
				//Nope it's obviously the bgTransform that does the transforms DUH julia.
				if (cohesiveSizing) {
					doCohesiveSizing (bgRenderer.sprite, ref bgTransform);
				}
				if (scalability) {
					doSpriteScaling (bgRenderer.sprite, ref bgTransform);
				}
				if (rotatability) {
					doSpriteRotation (bgRenderer.sprite, ref bgTransform);
				}
				if (horizontalReflect) {
					doSpriteFlip (bgRenderer.sprite, ref bgTransform, "x");
				}
				if (verticalReflect) {
					doSpriteFlip (bgRenderer.sprite, ref bgTransform, "y");
				}

				//set minX for next iteration
				minX = bgTransform.position.x;

			}


			//now we can initialize for the parallax
			cameraX = cameraTransform.position.x;
			cameraY = cameraTransform.position.y;
		}
	}

	private void prepareCohesiveSizing( ){
		float bigArea, smallArea, area;

		bigArea = bgSprites [0].bounds.size.x * bgSprites [0].bounds.size.y;
		smallArea = bgSprites [0].bounds.size.x * bgSprites [0].bounds.size.y;
		for (int i = 1; i < bgSprites.Length; i++){
				area = bgSprites [i].bounds.size.x * bgSprites [i].bounds.size.y;
				if (area > bigArea) {
					bigArea = area;
				} else if (area < smallArea) {
					smallArea = area;
				}
		}
		spriteBigArea = bigArea;
		spriteSmallArea = smallArea;
		}

	private void doCohesiveSizing(Sprite spriteyBoy, ref Transform spriteyBoyTransf){

		float area = spriteyBoy.bounds.size.x * spriteyBoy.bounds.size.y;
//		float resizeBy = (spriteSmallArea / area);
		if( Mathf.Abs(area - spriteSmallArea) > Mathf.Abs(spriteBigArea - area)){
			spriteyBoyTransf.localScale = spriteyBoyTransf.localScale / 3; //this bothers me!!!
//			spriteyBoyTransf.localScale = spriteyBoyTransf.localScale * resizeBy;
		}
	}

	private void doSpriteScaling(Sprite spriteyBoy, ref Transform spriteyBoyTransf){
		int bigScale = Random.Range(50, 150);
		float scale = (float)(bigScale / 100.0);
		// Debug.Log ("scale = " + scale);
		spriteyBoyTransf.localScale = spriteyBoyTransf.localScale * scale;

	}
	private void doSpriteRotation(Sprite spriteyBoy, ref Transform spriteyBoyTransf){
		int rotation = Random.Range (0, 360);
		spriteyBoyTransf.Rotate (Vector3.forward * rotation); //rotation.Set(spriteyBoyTransf.rotation.x, spriteyBoyTransf.rotation.y, spriteyBoyTransf.rotation.z, (float)rotation);

	}
	private void doSpriteFlip(Sprite spriteyBoy, ref Transform spriteyBoyTransf, string axis){
		int flip = Random.Range(0,1); //should we flip? (not always!!)
		if (flip == 1) {
			spriteyBoyTransf.localScale = new Vector3 (((-1) * spriteyBoyTransf.localScale.x), spriteyBoyTransf.localScale.y, spriteyBoyTransf.localScale.z);
		}

	}
		
	// void FixedUpdate() {
	// 	if(!turnedOff && running){
	// 		float deltaX = cameraX - cameraTransform.position.x;
	// 		float deltaY = cameraY - cameraTransform.position.y;
	// 		for (int i = 0; i < bgObjs.Length; ++i) {
	// //			Debug.Log ("Changing position "+bgObjs [i].transform.position.x+" to " + (bgObjs [i].transform.position.x + (parallaxSpeed * deltaX)));
	// 			bgObjs [i].transform.position = new Vector3 ((bgObjs [i].transform.position.x + (parallaxSpeed * deltaX)), (bgObjs [i].transform.position.y + (parallaxSpeed * deltaY)), bgObjs [i].transform.position.z);
	// 		}
	// 		cameraX = cameraTransform.position.x;
	// 		cameraY = cameraTransform.position.y;
	// 	}
	// }

}
