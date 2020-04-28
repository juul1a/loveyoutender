using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthItem : MonoBehaviour
{

    private Image img;

    public Sprite wholeHealth;
    public Sprite halfHealth;

    private bool whole;
    private bool isActivated;

    public Color deactivated;
    public Color activated;

    // Start is called before the first frame update
    void Awake()
    {
        img = gameObject.GetComponent<Image>();
        whole = true;
        isActivated = true;
    }

    public void Deduct(){
        img.color = deactivated;
        img.overrideSprite = halfHealth;
        whole = false;
        isActivated = false;
        // img.sprite = null;
    }

    public void Half(){
        img.color = activated;
        if(!whole && isActivated){
            img.overrideSprite = wholeHealth;    
            whole = true;
        }
        else{
            img.overrideSprite = halfHealth;
            whole = false;
        }
        isActivated = true;
    }

    public void Full(){
        img.color = activated;
        img.overrideSprite = wholeHealth;
        whole = true;
        isActivated = true;
        // img.sprite = wholeHealth;
    }

}
