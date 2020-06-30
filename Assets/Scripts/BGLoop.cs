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
    Vector3 camTopRight, camBottomLeft;

    private float spriteHeight;

    void Awake(){
        spriteHeight = bottom.GetComponent<SpriteRenderer>().bounds.size.y;
        moveWalls();
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
        // if(camBottomLeft != mainCam.ViewportToWorldPoint(new Vector3(0,0,0)) || camTopRight != mainCam.ViewportToWorldPoint(new Vector3(1,1,0))){
        //     Debug.Log("Moving walls");
        //     moveWalls();
        // }

    }

    void moveWalls(){
        BoxCollider2D[] walls = mainCam.GetComponentsInChildren<BoxCollider2D>();
        camBottomLeft = mainCam.ViewportToWorldPoint(new Vector3(0,0,0));
        camTopRight = mainCam.ViewportToWorldPoint(new Vector3(1,1,0));
        if(walls.Length>0){
            foreach(BoxCollider2D wall in walls){
                if(wall.gameObject.name.Contains("Right")){
                    wall.transform.position = new Vector3(camTopRight.x, wall.transform.position.y, wall.transform.position.z);
                }
                else if(wall.gameObject.name.Contains("Left")){
                    wall.transform.position = new Vector3(camBottomLeft.x, wall.transform.position.y, wall.transform.position.z);
                }
                wall.size = new Vector2(wall.size.x, (camTopRight.y - camBottomLeft.y));
            }
        }
    }
}
