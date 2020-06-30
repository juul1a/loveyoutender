using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenV2 : MonoBehaviour
{

    
    [SerializeField] private float playerGenDist;
    [Tooltip("Farthest away two objects should be")]
    [SerializeField] private float maxDistBtwnObjs;

    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    
    private Vector3 latestEndPos;

    // [SerializeField] private bool horizontal, vertical;

    // [Tooltip("Whether or not to (mildly)scale the assets")]
	// [SerializeField] private bool scalability;
	[Tooltip("Whether or not to rotate assets (full 360)")]
	[SerializeField] private bool rotatability;
	[Tooltip("Whether or not to reflect the sprite horizontally")]
	[SerializeField] private bool horizontalReflect;
	[Tooltip("Whether or not to reflect the sprite vertically")]
	[SerializeField] private bool verticalReflect;

    //Parent obj to nest all bg objs under
    [SerializeField] private GameObject activeObjsParent, inActiveObjsParent;

	//All background objects we make
	// private GameObject[] bgObjsBuilt;
    private float minSpawnX, maxSpawnX;

    //All background objects prefabs
	[SerializeField] private GameObject[] bgObjs;

    

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 camBottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,0));
        Vector3 camTopRight = cam.ViewportToWorldPoint(new Vector3(1,1,0));
        minSpawnX = camBottomLeft.x;
        maxSpawnX = camTopRight.x;
        latestEndPos = player.transform.position;
        //Generate a good chunk of level first
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
        SpawnNewObj();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, latestEndPos)<playerGenDist){
            SpawnNewObj();
            SpawnNewObj();
            SpawnNewObj();
            SpawnNewObj();
            DeactivateOld();
        }
    }

    void SpawnNewObj(){
        float setX = Random.Range(minSpawnX, maxSpawnX);
        float setY = Random.Range(latestEndPos.y, latestEndPos.y+maxDistBtwnObjs);
        int k = Random.Range (0, bgObjs.Length-1);//determines which prefab to use
        GameObject bgObjectPrefab = bgObjs[k];
        GameObject newBgObj = BgObjPool.bgPoolInstance.GetPooledObj(bgObjectPrefab);
        newBgObj.transform.SetParent(activeObjsParent.transform, true);
        newBgObj.transform.position = new Vector3(setX, setY, newBgObj.transform.position.z);
        newBgObj.SetActive(true);
        latestEndPos = newBgObj.transform.position;
        if (rotatability) {
            doSpriteRotation (newBgObj.transform);
        }
        if (horizontalReflect) {
            doSpriteFlip (newBgObj.transform, "x");
        }
        if (verticalReflect) {
            doSpriteFlip (newBgObj.transform, "y");
        }

        //pick random prefab
        //spawn from pool
        //set parent as active
    }

    void DeactivateOld(){
        Transform[] activeObjs = activeObjsParent.GetComponentsInChildren<Transform>();
        for(int i = 1; i < activeObjs.Length; i++){
            if(Vector3.Distance(player.transform.position, activeObjs[i].position)>playerGenDist*1.5){
                activeObjs[i].gameObject.SetActive(false);
                activeObjs[i].SetParent(inActiveObjsParent.transform, true);
            }
        }
    }

    private void doSpriteRotation(Transform spriteyBoyTransf){
		int rotation = Random.Range (0, 360);
		spriteyBoyTransf.Rotate (Vector3.forward * rotation); //rotation.Set(spriteyBoyTransf.rotation.x, spriteyBoyTransf.rotation.y, spriteyBoyTransf.rotation.z, (float)rotation);

	}
	private void doSpriteFlip(Transform spriteyBoyTransf, string axis){
		int flip = Random.Range(0,1); //should we flip? (not always!!)
		if (flip == 1) {
			spriteyBoyTransf.localScale = new Vector3 (((-1) * spriteyBoyTransf.localScale.x), spriteyBoyTransf.localScale.y, spriteyBoyTransf.localScale.z);
		}
    }

}
