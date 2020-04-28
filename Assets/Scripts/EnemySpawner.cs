using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private float maxSpawnX, minSpawnX;
    private Vector3 camBottomLeft, camTopRight;
    private Camera cam;

    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private float spawnTime = 2.0f;

    private List<GameObject> spawnedEnemies, deadEnemies;
    
    [SerializeField]
    private int maxEnemies = 10;

    [SerializeField]
    public int totalEnemies = 40;
    public int enemiesSpawned = 0;
    public bool allEnemiesDead = false;

    void Awake(){
        cam = gameObject.GetComponentInParent<Camera>();
        camBottomLeft = cam.ViewportToWorldPoint(new Vector3(1,0,0));
        camTopRight = cam.ViewportToWorldPoint(new Vector3(1,1,0));
        minSpawnX = camBottomLeft.x;
        maxSpawnX = camTopRight.x;
    }

    public void ManualStart()
    {
        enemiesSpawned = 0;
        allEnemiesDead = false;
        spawnedEnemies = new List<GameObject>();
        deadEnemies = new List<GameObject>();
        InvokeRepeating("SpawnEnemy", 2.0f, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        SetMaxAndMin();
        //Start polling to see when all enemies are dead, but we only need to 
        //check after all of the enemies have been spawned
        if(enemiesSpawned == totalEnemies){
            if(deadEnemies.Count < totalEnemies){
                foreach(GameObject enemyObj in spawnedEnemies){
                    if(enemyObj.GetComponent<Enemy>().isDead()){
                        deadEnemies.Add(enemyObj);
                    }
                }
                foreach(GameObject deadEnemy in deadEnemies){
                    //Remove each dead enemy from the spawned enemies list so we don't keep re-looping through the dead ones
                    spawnedEnemies.Remove(deadEnemy);
                }
            }
            else{
                allEnemiesDead = true;
            }
        }
    }

    void SetMaxAndMin(){
        camBottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,0));
        camTopRight = cam.ViewportToWorldPoint(new Vector3(1,1,0));
        minSpawnX = camBottomLeft.x;
        maxSpawnX = camTopRight.x;
    }

    void SpawnEnemy(){
        if(spawnedEnemies.Count < maxEnemies && enemiesSpawned < totalEnemies){
            int index = Random.Range (0, enemies.Length);
            GameObject enemy = enemies[index];
            float xPos = Random.Range(minSpawnX, maxSpawnX);
            GameObject newEnemy = Instantiate(enemy);
            if(newEnemy.GetComponent<FlyingEnemy>()){
                newEnemy.transform.position = new Vector3(xPos, newEnemy.transform.position.y, gameObject.transform.position.z);
            }
            else{
               newEnemy.transform.position = new Vector3(xPos, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            spawnedEnemies.Add(newEnemy);
            enemiesSpawned++;
        }
    }
}
