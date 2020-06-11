using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnObj;
    private EnemySpawner enemySpawner;
    [SerializeField]
    private GameObject bulletBoss;
    private GameObject thisBoss;
    private Enemy thisBossEnemy;
    [SerializeField]
    private Player player;

    void Awake(){
        enemySpawner = enemySpawnObj.GetComponent<EnemySpawner>();
    }
    void Start()
    {
        enemySpawner.ManualStart();

    }

    // Update is called once per frame
    void Update()
    {
        if(enemySpawner.allEnemiesDead && thisBoss == null){
            GameObject boss = Instantiate(bulletBoss);
            bulletBoss.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            thisBoss = boss;
            thisBossEnemy = thisBoss.GetComponent<Enemy>();
        }
        if(thisBoss != null && thisBossEnemy.isDead()){
            player.WinTheGame();
        }
    }
}
