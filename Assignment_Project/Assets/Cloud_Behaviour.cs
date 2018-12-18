using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Behaviour : MonoBehaviour
{

    public float cloudSize;
    public float cloudMaxSize;
    public float cloudMinSize;


    public float growRate; 

    public float shrinkRate;
    public bool shrink;

    public bool snow;

    public ParticleSystem snowS;

    public ParticleSystem rainS;

    ParticleSystem.EmissionModule snowE;
    ParticleSystem.EmissionModule rainE;
    
    
    public float hoverDistance;
    
    
    
    public bool rise;
    public float riseSpeed;

    public Texture2D texture;

    public Color col;
    public Color water;

    MapGenerator mapGen;

    Ray ray;

    public LayerMask ground;

    public GameObject model;


    


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
    }

    void Update()
    {
        model.transform.localScale = new Vector3(cloudSize, cloudSize, cloudSize);
        AvoidGround();
        SetInactive();
        SampleColourUnder();

        if(col.b == water.b && shrink == false){
            cloudSize += growRate * Time.deltaTime;
        }
        
        if(cloudSize >= cloudMaxSize){
            shrink = true;
        }

        if(shrink){
            cloudSize -= shrinkRate *Time.deltaTime;

            if(snow){
                snowE.rateOverTime = 100f;
                rainE.rateOverTime = 0f;

            }else{
                rainE.rateOverTime = 100f;
                snowE.rateOverTime = 0f;
            }

            if(cloudSize <= cloudMinSize){
                shrink = false; 
            }
        }else{
            cloudSize += (growRate/15f) * Time.deltaTime;
            snowE.rateOverTime = 0;
            rainE.rateOverTime = 0;
        }

        if(transform.position.y > 160f){
            snow = true;
        }else{
            snow = false;
        }

    }

    
    
    
    
    
    
    
    
    
    
    void SetInactive(){
        if(transform.position.y < 0){
            this.gameObject.SetActive(false);
        }
    }













    void AvoidGround(){
        if(rise){
            float _aboveCloud = transform.position.y + (1f * riseSpeed);

            float newYPos = Mathf.Lerp(transform.position.y, _aboveCloud, Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        }else{
            float _belowCloud = transform.position.y - (1f * (riseSpeed/4));

            float newYPos = Mathf.Lerp(transform.position.y, _belowCloud, Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        }
    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    void SampleColourUnder(){
        Debug.DrawRay(transform.position, Vector3.down *300f, Color.red);

        ray = new Ray(transform.position, Vector3.down);


        if(Physics.Raycast(ray, out hit, 300f, ground)){
            //gets the hits exact pixel colour
            if(hit.collider.tag.Equals("Ground")){
                texture = hit.transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
                Vector2 pixelHit = hit.textureCoord;
                pixelHit.x *= texture.width;
                pixelHit.y *= texture.height;

                col = texture.GetPixel((int)pixelHit.x, (int)pixelHit.y);


            }
        }

    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    void OnTriggerStay(Collider other){
        if(other.tag == "Ground"){
            rise = true;
        }
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    void OnTriggerExit(Collider other){
        if(other.tag =="Ground"){
            rise = false;
        }
    }
}
