using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxMachine{
        public string LayerName;
        public GameObject[] objectsInLayer;
        public float parallaxSpeed;
    }
    public ParallaxMachine[] machines;  

    private GameObject mainCamera;
    private float cameraX, cameraY;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraX = mainCamera.transform.position.x;
        cameraY = mainCamera.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mySpeed;
        float deltaX = mainCamera.transform.position.x - cameraX;
        float deltaY = mainCamera.transform.position.y - cameraY;
        for(int i = 0; i < machines.Length; i++)
        {
            mySpeed = machines[i].parallaxSpeed;
            for(int j = 0; j<machines[i].objectsInLayer.Length; j++){
                machines[i].objectsInLayer[j].transform.position = new Vector3 ((machines[i].objectsInLayer[j].transform.position.x + (machines[i].parallaxSpeed * deltaX)), (machines[i].objectsInLayer[j].transform.position.y + (machines[i].parallaxSpeed * deltaY)), machines[i].objectsInLayer[j].transform.position.z);
            }
        }
        cameraX = mainCamera.transform.position.x;
        cameraY = mainCamera.transform.position.y;
    }
}
