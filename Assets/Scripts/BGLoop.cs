using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLoop : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private bool vertical;

    // private SpriteRenderer[] bgSprites;
    [SerializeField]
    private GameObject top, middle, bottom;

    private float spriteHeight;


    // Start is called before the first frame update
    void Start()
    {
        spriteHeight = bottom.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(mainCam.transform.position.y > (middle.transform.position.y + spriteHeight/2)){
            bottom.transform.position = new Vector3(bottom.transform.position.x, top.transform.position.y + spriteHeight, bottom.transform.position.z);
            GameObject tempTop = top;
            top = bottom;
            bottom = middle;
            middle = tempTop;
        }
        if(mainCam.transform.position.y < (middle.transform.position.y - spriteHeight/2)){
            top.transform.position = new Vector3(top.transform.position.x, bottom.transform.position.y - spriteHeight, top.transform.position.z);
            GameObject tempBottom = bottom;
            bottom = top;
            top = middle;
            middle = tempBottom;
        }

    }
}
