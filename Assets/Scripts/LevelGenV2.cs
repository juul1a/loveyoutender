using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenV2 : MonoBehaviour
{

    [SerializeField]
    private float playerGenDist;

    [SerializeField]
    private Player player;
    private Vector3 latestEndPos;

    // Start is called before the first frame update
    void Awake()
    {
        //Generate a good chunk of level first
    }

    // Update is called once per frame
    void Update()
    {
        //if the player omes within Vector3.Distance(playerGenDist and latestEndPos)
    }
}
