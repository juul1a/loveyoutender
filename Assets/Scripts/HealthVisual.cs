using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject healthObj;
    [SerializeField]
    private float healthBarYOffset = 5f;
    [SerializeField]
    private float distBetweenImages = 5f;

    private float health, prevHealth;
    [SerializeField]
    private GameObject playerObj;
    private Player player;

    public List<GameObject> healthObjs;

    void Start(){

        // GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
        health = player.GetHealth();
        prevHealth = health;

        Sprite healthSprite = healthObj.GetComponent<Image>().sprite;
        healthObjs = new List<GameObject>();
        
        float spriteWidth = healthSprite.rect.width * healthObj.transform.localScale.x;
        float spriteHeight = healthSprite.rect.height * healthObj.transform.localScale.y;
        float healthYPos = -1 * (spriteHeight + healthBarYOffset);
        float offsetX = spriteWidth + distBetweenImages;
        Vector2 offset = new Vector2(offsetX,0);
        
        Vector2 prevHealthPos = new Vector2(distBetweenImages,healthYPos);
         //if(player == null){
            GameObject createdObj = CreateHealthImage(prevHealthPos);
            healthObjs.Add(createdObj);
            prevHealthPos = createdObj.transform.localPosition;
        //}
        
        for(int i = 1; i < health; i++){
            Vector2 newPosition = prevHealthPos + offset;
            
            createdObj = CreateHealthImage(newPosition);
            prevHealthPos = createdObj.transform.localPosition;

            healthObjs.Add(createdObj);
        }
    }

    private GameObject CreateHealthImage(Vector2 anchoredPos){
        GameObject healthGameObject = Instantiate(healthObj);
        // healthGameObject.transform.parent = transform;
        healthGameObject.transform.SetParent(transform, false);
        healthGameObject.transform.localPosition = Vector3.zero;

        healthGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        // healthGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(233,275);
        return healthGameObject;
    }

    void Update(){
        health = player.GetHealth();
        if(health != prevHealth){
            UpdateVisual();
        }
        prevHealth = health;
    }

    private void UpdateVisual(){
        // prevHealth = health;
        // health = player.health;
        
        if(health != prevHealth){
            int healthFloored = Mathf.FloorToInt(health);
            for(int i = 0; i < healthFloored; ++i){
                UIHealthItem healthItem = healthObjs[i].GetComponent<UIHealthItem>();
                healthItem.Full();
            }
            //Half
            if(health - healthFloored > 0){
                UIHealthItem healthItem = healthObjs[healthFloored].GetComponent<UIHealthItem>();
                healthItem.Half();
            }
            int maxHealth = healthObjs.Count;
            for(int j = maxHealth-1; j >= Mathf.CeilToInt(health); j--){
                UIHealthItem healthItem = healthObjs[j].GetComponent<UIHealthItem>();
                healthItem.Deduct();
            }
        }


        // //Player gained health
        // if(health>prevHealth){
        //     float healthDelta = health - prevHealth;
        //     float healthDeltaFloored = Mathf.Floor(healthDelta);
        //     //Maxhealth increase
        //     if((int)health>healthObjs.Count){
        //         //Add and place new object
        //         Debug.Log("New health obj");
        //     }
        //     else{
        //         for(float i = 0; i < healthDeltaFloored; i++){
        //             int index = (int)(health + i) - 1;
        //             // Debug.Log("Updating index "+index);
        //             UIHealthItem healthItem = healthObjs[index].GetComponent<UIHealthItem>();
        //             healthItem.Full();
        //         }
        //         if (healthDelta - healthDeltaFloored > 0){
        //             int index = (int)(health + healthDeltaFloored)-1;
        //             UIHealthItem healthItem = healthObjs[index].GetComponent<UIHealthItem>();
        //             healthItem.Half();
        //         }
        //     }
        // }
        // //Player lost health
        // else if(health<prevHealth){
        //     // Debug.Log("Setting health");
        //     float healthDelta = prevHealth - health;
        //     // Debug.Log("helthData = "+healthDelta);
        //     float healthDeltaFloored = Mathf.Floor(healthDelta);
        //     // Debug.Log("helthDataFloored = "+healthDeltaFloored);
        //     for(float i = healthDeltaFloored; i>0; i--){
        //         // Debug.Log("Deducting");
        //         int index = (int)(health + i) - 1;
        //         UIHealthItem healthItem = healthObjs[index].GetComponent<UIHealthItem>();
        //         healthItem.Deduct();
        //     }
        //     float healthFraction = healthDelta - healthDeltaFloored;
        //     if(healthFraction >0){
        //         int index = (int)(health + healthDeltaFloored);
        //         UIHealthItem healthItem = healthObjs[index].GetComponent<UIHealthItem>();
        //         float currentHealthFraction = health - Mathf.Floor(health);
        //         if(currentHealthFraction>0){
        //             healthItem.Half();
        //         }
        //         else{
        //             healthItem.Deduct();
        //         }
                
        //     }

        // }

    }

    private void DeductHealth(GameObject health){
        //take object and delete it or invis it or something
        Image thisImage = health.GetComponent<Image>();
        
    }

}
