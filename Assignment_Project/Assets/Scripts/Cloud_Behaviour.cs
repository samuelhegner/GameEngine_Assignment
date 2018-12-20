using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Behaviour : MonoBehaviour
{
    //The Variable that controls the size of the cloud
    public float cloudSize;
    //The maximun size the cloud is going to grow to before then shrinking
    public float cloudMaxSize;
    //The minimum size the cloud will shrink to before stating to grow again
    public float cloudMinSize;

    //The rate at which the cloud grows
    public float growRate; 
    //The rate at which the cloud shrinks
    public float shrinkRate;
    //The cloud shrinks while the this bool is true
    public bool shrink;
    //While the true it snows, while false the cloud rains
    public bool snow;

    //The two particle systems
    public ParticleSystem snowS;
    public ParticleSystem rainS;

    //The two emision modules that are used to toggle between rain and snow
    ParticleSystem.EmissionModule snowE;
    ParticleSystem.EmissionModule rainE;
    
    //This distance obove the floor the cloud will hover
    public float hoverDistance;

    //The point on the mesh where the Ray cast hits
    Vector3 hitPoint;
    //the desired y position the cloud lerps to
    float desiredYPos;
    
    
    //the speed at which the cloud lerps to the desired y position
    public float riseSpeed;

    //The 2d texture that is used to pinpoint the pixel the cloud is over
    public Texture2D texture;
    //The current colour of the pixel the raycast is hitting
    public Color col;
    //the color of the water region to compare the other colour variable to
    public Color water;

    //used to get the water colour
    MapGenerator mapGen;
    //the ray used to check height and colour
    Ray ray;
    //the layer mask the ray is able to hit
    public LayerMask ground;
    //the clouds model that shrinks
    public GameObject model;
    //used to get the snowfall line
    Spawn_Clouds spawner;


    RaycastHit hit;

    void Start()
    {
        mapGen = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
        water = mapGen.regions[0].colour;
        cloudSize = 1;
        cloudMaxSize = 2;
        cloudMinSize = 0.8f;

        rainE = rainS.emission;
        snowE = snowS.emission;
        snow = false;

        spawner = GameObject.Find("Cloud Generator").GetComponent<Spawn_Clouds>();
    }

    void Update()
    {
        model.transform.localScale = new Vector3(cloudSize, cloudSize, cloudSize);
        AvoidGround();
        SetInactive();
        SampleColourUnder();


        //Checks whether the current pixel color is the same as the water colour
        //if so the the cloud grows more than if it isn't over water
        if(col.b == water.b && shrink == false){
            cloudSize += growRate * Time.deltaTime;
        }
        
        //if the cloud hits max size it will start shrinking
        if(cloudSize >= cloudMaxSize){
            shrink = true;
        }


        //thinks that happen while the cloud is shrinking
        if(shrink){

            //clouds size shrinks
            cloudSize -= shrinkRate *Time.deltaTime;

            //while snowing the snow emision rate is 100 and the rain is 0
            //this is the same for the other way around
            if(snow){
                snowE.rateOverTime = 100f;
                rainE.rateOverTime = 0f;

            }else{
                rainE.rateOverTime = 100f;
                snowE.rateOverTime = 0f;
            }

            //flips shrink when cloud hit minimum size
            if(cloudSize <= cloudMinSize){
                shrink = false; 
            }
        }else{
            //turns of particle and slowly grows cloud while the cloud isn't shrinking
            cloudSize += (growRate/15f) * Time.deltaTime;
            snowE.rateOverTime = 0;
            rainE.rateOverTime = 0;
        }

        //set snow based on y position
        if(transform.position.y > spawner.snowFallLine){
            snow = true;
        }else{
            snow = false;
        }

    }

    
    
    
    
    
    
    
    
    
    //turns off the cloud when it sinks too low
    void SetInactive(){
        if(transform.position.y < 0){
            this.gameObject.SetActive(false);
        }
    }












    //function that lets clouds avoid the floor
    void AvoidGround(){
        if(hitPoint != Vector3.zero){
            Vector3 desiredPos = new Vector3(hitPoint.x, hitPoint.y + hoverDistance, hitPoint.z);

            transform.position = Vector3.Lerp(transform.position, desiredPos, riseSpeed * Time.deltaTime);
        }
        
        /* if(rise){
            float _aboveCloud = transform.position.y + (1f * riseSpeed);

            float newYPos = Mathf.Lerp(transform.position.y, _aboveCloud, Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        }else{
            float _belowCloud = transform.position.y - (1f * (riseSpeed/4));

            float newYPos = Mathf.Lerp(transform.position.y, _belowCloud, Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        }*/
    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    //function that shoot the ray, samples the color and location it hits
    void SampleColourUnder(){
        //Debug.DrawRay(transform.position, Vector3.down *300f, Color.red);

        ray = new Ray(transform.position, Vector3.down);

        //shoots ray straight down-
        if(Physics.Raycast(ray, out hit, 300f, ground)){
            //gets the hits exact pixel colour
            if(hit.collider.tag.Equals("Ground")){
                texture = hit.transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
                Vector2 pixelHit = hit.textureCoord;
                pixelHit.x *= texture.width;
                pixelHit.y *= texture.height;

                col = texture.GetPixel((int)pixelHit.x, (int)pixelHit.y);
                //sets the vector of the hit position
                hitPoint = hit.point;
            }
        }else{
            hitPoint = new Vector3();
        }

    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /* void OnTriggerStay(Collider other){
        if(other.tag == "Ground"){
            rise = true;
        }
    }*/

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /* void OnTriggerExit(Collider other){
        if(other.tag =="Ground"){
            rise = false;
        }
    }*/
}
