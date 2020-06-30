using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgObjPool : MonoBehaviour
{
    //Static instance of this bullet pool for external reference
    public static BgObjPool bgPoolInstance;

    //All bullets that have been instantiated
    private List<GameObject> objs;

    private void Awake(){
        bgPoolInstance = this;
        objs = new List<GameObject>();
    }

    //Public function that returns the reference to a bullet object - either newly created, or previously used but inactive.
    //We recycle bullets here
    public GameObject GetPooledObj(GameObject thisPrefab){
        if(objs.Count > 0){
            //Goes through all bulets in the list
            for(int i = 0; i < objs.Count; i++){
                //Grabs the first inactive one
                if(!objs[i].activeInHierarchy && objs[i].name.Contains(thisPrefab.name)){
                    //Returns the inactive bullet
                    Debug.Log("returning "+objs[i].name);
                    return objs[i];
                }
            }
        }
        // If it gets here then there were not enough bullets in the pool, aka there were no inactive bullets to instantiate
        // So a new bullet is instantiated, added to the list, and returnet.
        GameObject newObj = Instantiate(thisPrefab);
        newObj.SetActive(false);
        objs.Add(newObj);
        return newObj;
    }
    
}
