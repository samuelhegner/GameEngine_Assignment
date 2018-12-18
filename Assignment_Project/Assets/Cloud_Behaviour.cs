using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Behaviour : MonoBehaviour
{

    public float cloudSize;

    public bool shrink;

    public bool snow;
    public bool rise;
    public float riseSpeed;

    public Texture2D texture;

    public Color col;

    Ray ray;

    public LayerMask ground;
    


    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AvoidGround();
        SetInactive();
        SampleColourUnder();
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
            print(hit.collider.tag);
            if(hit.collider.tag.Equals("Ground")){
                print("hurray");
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
