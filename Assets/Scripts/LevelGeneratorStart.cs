using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script is attached to a gameobject that will parent all level generator systems. 
    The platform loader(s) are to be attached to the same game object as this script.
    Then all background renderers will be attached to children objects of this gameobject.
    This script simply loads the platform loader and background renderer objects and runs them.
*/

public class LevelGeneratorStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlatformLoader[] platformLoaders = gameObject.GetComponents<PlatformLoader>();
        BGRenderer[] bGRenderers = gameObject.GetComponentsInChildren<BGRenderer>();

        foreach (PlatformLoader pfloader in platformLoaders){
            pfloader.Run();
        }

        foreach (BGRenderer bgrenderer in bGRenderers){
            bgrenderer.Run();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
