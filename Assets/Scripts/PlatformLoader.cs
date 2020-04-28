using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLoader : MonoBehaviour
{
    public bool turnedOff; //Is the platform loader turned off?
    public bool genEnemies; //True if we should generate enemies with the platforms
    public GameObject[] platforms; //All different platform prefab game objects are stored here. make sure they have edgecollider2d components on them
    public GameObject homeBase; //The final destination of the player aka the "win" platform (prefab object)
    public GameObject platformParent; //The game object to nest all platform objects generated for neat tidiness. Just a rando empty game object is fine.
    public GameObject enemyParent; //The game object to nest all enemy objects generated for neat tidiness. Just a rando empty game object is fine.
    public float maxVerticalDist; //Maximum vertical distance between two platforms (platform x can be this far above platform y)
    public float minVerticalDist; ////Minimum vertical distance between two platforms (platform x can be this far below platform y)
    public float maxHorizontalDist; //Maximum horizontal distance betweent two platforms
    public float ultraMaxY; //Maximum y value of any/all platforms (cannot go higher than this in the map)
    public float ultraMinY; //Minimum y value of any/all platforms (cannot go lower than this in the map)
    public int levelSize; //Total number of platforms to generate
    public float chanceOfEnemy = 0.3f; //Percentage chance of enemy generating on a platform (as a fraction) default 30%
    private List<Vector2> platformPositions; //Stores all positions of all platforms in list of 2d vectors i guess

    public GameObject[] enemyObjs; //All generated enemies i think


    void Update(){
        //check if the player is within a certain distance of the end of the level
        //call generate level and pass in the last platform
        //that's what it would do here if we had an endless sidescroller
    }

    public void Run(){
        if(!turnedOff){ //only generat if it's turned on
            platformPositions = new List<Vector2>();
            //make first platform
            GameObject firstPlatform = Instantiate(platforms[0]);
            //first platform is the anchor for the whole level
            //generateLevel builds the whole thing and populates the platformPositions list with all the platforms
            generateLevel(firstPlatform, 1);

            // good job julia, really useful code here.
            Vector2 lastPlatform = platformPositions[platformPositions.Count-1];
            Vector2 secondLastPlatform = platformPositions[platformPositions.Count-2];
        }
    }

    //recursive function that generates one plaform and positions it based on the location of the previous plaform (passed in as the variable firstPlaform)
    //recurses until total number of platforms created (numberOfPlatforms) is equal to levelSize
    void generateLevel(GameObject firstPlatform, int numberOfPlatforms){
        //only run if we have not reached the level size
        if(numberOfPlatforms <= levelSize){
            //get the top right x,y coordinates of the platform
            Vector2 firstPlatformTopRight = getPlatformCornerWorld("right", firstPlatform); 
            //nice lil hard coded string to put as the tag for all platforms. 
            string platformTag = "Platform";
            //minimum x of new platform is the maximum x value of the previous platform.
            float minX = firstPlatformTopRight.x;
            //maximum x of new platform is the minimum x + the maximum horizontal distance the two platforms can be apart
            float maxX = firstPlatformTopRight.x + maxHorizontalDist;
            //minimum y value is the y value of the previous platform + minimum vertical distance
            float minY = firstPlatform.transform.position.y + minVerticalDist;
            //double check that we won't go below the game mimimum
            minY = minY < ultraMinY ? ultraMinY : minY;
            //maximum y value is the y value of the previous platform + maximum vertical distance
            float maxY = firstPlatform.transform.position.y + maxVerticalDist;
            //double check that we won't go above the game maximum
            maxY = maxY > ultraMaxY ? ultraMaxY : maxY;
            //get random values from the minimum and maximums we just set
            float setX = Random.Range (minX, maxX);
            float setY = Random.Range (minY, maxY);
            //same z value across the board because we 2D yo
            float setZ = firstPlatform.transform.position.z; 
            GameObject platformRef;
            //if we're at the end of the level then we are going to use the homebase prefab
            if(numberOfPlatforms == levelSize){
                platformRef = homeBase;
                platformTag = "Home";
            }
            else{
                //get a random platform prefab  from the array
                platformRef = platforms[Random.Range(0, platforms.Length)];
            }  
            //finally, we instantiate the platform game object
            GameObject platformObj = Instantiate(platformRef);
            //set position to the x, y, and z values we just determined
            platformObj.transform.position = new Vector3(setX, setY, setZ);
            //set the platform tag
            platformObj.tag = platformTag;
            //nest the platform object under the parent object
            platformObj.transform.parent = platformParent.transform;

            //add the position of the newly created platform to our master list of platform positions
            platformPositions.Add(new Vector2(platformObj.transform.position.x, platformObj.transform.position.y));

            //only run this if genEnemies is turned on
            //it will generate an enemy above the newly created platform based on the chanceOfEnemy
            if(genEnemies){
                //we pick a random mumber from 0 to 100 and if it's within the percentage then we generate an enemy
                float enemyGen = Random.Range(0,100);
                //don't put "squiggly" in the name of your platforms or else it won't generate an enemy there.
                //I did this because my squiggly platform was giving me problems, ok?
                if(enemyGen<=(chanceOfEnemy*100) && !platformObj.name.Contains("squiggly")){
                    //pass the current platform object into the genEnemy function so we know where to position the new enemy
                    generateEnemy(platformObj);
                }
            }
            // hashtag recursion
            // increase the number of platforms we created by 1 so we can keep track
            generateLevel(platformObj, numberOfPlatforms+1);
        }
    }

    // get the position of the corner of the platform sprite in world co-ordinates
    //this requires your platforms to have an edgecollider2D component on them. what if it's a box collider, you may ask? well, you're screwed.
    Vector2 getPlatformCornerWorld(string side, GameObject platform){
        //you can change this to be whatever type of collider component you are using on your platforms.
        EdgeCollider2D platCollider = platform.GetComponent<EdgeCollider2D>();
        if(platCollider == null){
            platCollider = platform.GetComponentInChildren<EdgeCollider2D>();
        }
        //it goes through all the points in your edge collider and gets the co-ordinates of the point
        //if the method is asking for the "left" side of the platform then we grab the first point
        //if the method is asking for the "right side" then we grab the last point.
        Transform platColliderTransform = platCollider.transform;
        if(side == "left"){
            Vector2 firstPoint = platCollider.points[0];
            Vector2 firstPointWorld = new Vector2(firstPoint.x + platColliderTransform.position.x, firstPoint.y + platColliderTransform.position.y);
            return firstPoint;
        }
        if(side == "right"){
            Vector2 lastPoint = platCollider.points[platCollider.pointCount-1];
            Vector2 lastPointWorld = new Vector2(lastPoint.x + platColliderTransform.position.x, lastPoint.y + platColliderTransform.position.y);
            return lastPointWorld;
        }
        //random stand in null return value... helpful?
        else return new Vector2(0,0);
    }

    // generates an enemy prefab ontop of the supplied platform
    void generateEnemy(GameObject platform){
        // get the edges of the current platform so we can spawn the enemy in the middle of it
        Vector2 platLeftCorner = new Vector2(platform.transform.position.x, platform.transform.position.y); //origin is at the left side
        Vector2 platRightCorner = getPlatformCornerWorld("right", platform);
        
        //get the higher value of the platform's y and add a nice hard coded 15 units to it. that's the y value of the enemy to be spawned.
        float genY = (platLeftCorner.y > platRightCorner.y) ? platLeftCorner.y + 15 : platRightCorner.y + 15;
        //get the middle value of x
        float genX = (platRightCorner.x + platLeftCorner.x)/2;
        
        //which enemy object should we instantiate?
        int k = Random.Range (0, enemyObjs.Length);
        //this one!
        GameObject enemyToInst = enemyObjs[k];
        //instantiate new object
        GameObject newEnemy = Instantiate(enemyToInst);
        //if it's a flying enemy... oh this might break your code if you don't have a flying enemy class. yep. it will. 
        //so comment this out if you don't have a FlyingEnemy class. don't say I didn't warn you!
        if(newEnemy.GetComponent<FlyingEnemy>() ){
            //what am i doing here??? what is this for??? 
            //i think i set the general y position in the prefab nd then just add the new genY to it..
            //cuz flying enemies need to be higher up? gosh i don't know. i'm sorry.
            newEnemy.transform.position = new Vector3(genX, newEnemy.transform.position.y, 10);
        }
        //set the position of the new enemy
        newEnemy.transform.position = new Vector3(genX, genY, 10);
        //nest the object in the supplied parent object to be nice and tidy and maintain the impression that this is not spaghetti
        newEnemy.transform.parent = enemyParent.transform;
        return;
    }

    //this is not used but it's pretty cool to have right???
    //you can make a line of platforms that are perfectly connected based on their colliders
    //i don't want to comment this
    void MakeTwoPlatformsRightNextToEachother(){
            GameObject platform = platforms[0];
            GameObject thisPlatform = Instantiate(platform);
            EdgeCollider2D platCollider = thisPlatform.GetComponent<EdgeCollider2D>();
            Transform platColliderTransform = platCollider.transform;
            // Debug.Log("Platform collider transform right = "+platColliderTransform.right.x+", "+platColliderTransform.right.y+", "+platColliderTransform.right.z);
            // Debug.Log("Platform collider number of points = "+platCollider.pointCount);
            Vector2 firstPoint = platCollider.points[0];
            Vector2 lastPoint = platCollider.points[platCollider.pointCount-1];
            // Debug.Log("First point = "+firstPoint.x+", "+firstPoint.y);
            // Debug.Log("Last point = "+lastPoint.x+", "+lastPoint.y);
            Vector3 center = thisPlatform.transform.position;
            Vector3 firstPointWorld = new Vector3(center.x + firstPoint.x, center.y+firstPoint.y, center.z);
            Vector3 lastPointWorld = new Vector3(center.x + lastPoint.x, center.y+lastPoint.y, center.z);
            // Debug.Log("First point world = "+firstPointWorld.x+", "+firstPointWorld.y);
            // Debug.Log("Last point world = "+lastPointWorld.x+", "+lastPointWorld.y);



            GameObject platform1 = platforms[1];
            GameObject thisPlatform1= Instantiate(platform1);
            EdgeCollider2D platCollider1 = thisPlatform1.GetComponent<EdgeCollider2D>();
            Transform platColliderTransform1 = platCollider1.transform;
            // Debug.Log("Platform collider transform right = "+platColliderTransform1.right.x+", "+platColliderTransform1.right.y+", "+platColliderTransform1.right.z);
            // Debug.Log("Platform collider number of points = "+platCollider1.pointCount);
            Vector2 firstPoint1 = platCollider1.points[0];
            Vector2 lastPoint1 = platCollider1.points[platCollider1.pointCount-1];
            Transform thisPlatform1Transform = thisPlatform1.transform;
            float setX = lastPointWorld.x + Mathf.Abs(firstPoint1.x);
            float setY = lastPointWorld.y - Mathf.Abs(firstPoint1.y);
            thisPlatform1Transform.position = new Vector3(setX, setY, thisPlatform1Transform.position.z);
            
    }

    //background renderer uses this function.
    public List<Vector2> GetPlatformPositions(){
        List<Vector2> platformPosTemp = platformPositions;
        return platformPosTemp;
    }

}
