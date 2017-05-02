using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//*******************************************************************
// Turn Timer Script
// Chris
// 5/1/2017
// Deals with functionality of player motion, time, and spell mechanics
// also buys me a hat
//*******************************************************************



public class TurnTimer : MonoBehaviour
{

    public GameObject sliderObj;

    public GameObject player01;

    [Header("Where the player moves towards at all times.")]
    public GameObject mousePointObj;

    [Header("Fire Spell")]
    public GameObject fireSpell;

    public GameObject fireBallObj;

    private Slider slider;

    bool fireSelected;

    bool shotFire = false;


    public enum gameState { Planning, Running };

    [Header("player Starting State")]
    public gameState state;

    public float moveSpeed = 5;

    //queue for holding mouse click points. vestigial
    private Queue<GameObject> pointObjQueue = new Queue<GameObject>();
    private Queue<GameObject> pointEffectQueue = new Queue<GameObject>();

    //particle effect on clicks
    [Header("Effect that spawns on click")]
    public GameObject clickEffect;

    private Vector3 aimPos;

    // Use this for initialization
    void Start()
    {

        fireSelected = false;

        slider = sliderObj.GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {

        //old slider stuff
        
        if (slider.value == 0)
        {
            state = gameState.Running;
            transform.FindChild("Slider").FindChild("Fill Area").GetComponentInChildren<Image>().color = Color.gray;
        }


        if (state == gameState.Planning)
        {
            slider.value -= Time.deltaTime;
            transform.FindChild("Slider").FindChild("Fill Area").GetComponentInChildren<Image>().color = Color.red;
        }

        else
        {
            slider.value += Time.deltaTime;
        }
       
        //determine if the slider should go up
        if (slider.value == 0)
        {
            state = gameState.Running;
            StartCoroutine(startTurn());
        }

        //determine if the slider should go down
        if (slider.value == 5)
        {
            state = gameState.Planning;
            StartCoroutine(startTurn());
        }
        

        //on mouse click
        //mouse click stuff
        if (state == gameState.Planning)
        {
            //create a ray from camera and to point clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && fireSelected == false)
            {
                //get a screen point from where you clicked
                Vector3 screenPoint = Input.mousePosition;

                //distance of the plane from the camera
                screenPoint.z = 10.0f;

                //getting mouse point based on camera drama
                screenPoint = Camera.main.ScreenToWorldPoint(screenPoint);                   

               
                //if we click and our ray hits the playing field or anything physical
                if (Physics.Raycast(ray, out hit))
                {
                    // Debug.DrawLine(ray.direction, ray.origin, Color.green, 15.0f);

                    //moe target position
                    mousePointObj.transform.position = hit.point;


                    //move particle effect to click point
                    clickEffect.transform.position = hit.point;


                    //burst the effect with 30 particles
                    clickEffect.GetComponent<ParticleSystem>().Play();

                    //player01.transform.LookAt(mousePointObj.transform);
                    
                }


            }

            if (Input.GetButtonDown("Fire Spell") && fireSelected == false)
            {
                fireSpell.GetComponent<Image>().color = Color.red;

                fireSelected = true;

                player01.GetComponentInChildren<ParticleSystem>().Play();

            }
            else if (Input.GetButtonDown("Fire Spell") && fireSelected == true)
            {
                fireSpell.GetComponent<Image>().color = Color.white;

                fireSelected = false;
                player01.transform.LookAt(player01.transform.forward);
                player01.GetComponentInChildren<ParticleSystem>().Stop();
            }

            if (fireSelected)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    // Debug.DrawRay(player01.transform.position, hit.point, Color.red, 10.0f);
                    if(!shotFire)
                    player01.transform.LookAt(hit.point);
                    
                }

                if (Input.GetMouseButtonDown(0))
                {
                    shotFire = true;
                    aimPos = hit.point;
                }
            }
           

           
        }

        if (state == gameState.Running)
        {

            //if havent reached our click point
            if (Vector3.Distance(player01.transform.position, mousePointObj.transform.position) < 1)
            {
                clickEffect.GetComponent<ParticleSystem>().Stop();
            }
            else
            {
                //move towards target
                player01.transform.position = Vector3.MoveTowards(player01.transform.position, mousePointObj.transform.position, moveSpeed * Time.deltaTime);

            }

            

            if(shotFire)
            {
                Vector3 frontPlayer = new Vector3(player01.transform.position.x, player01.transform.position.y, player01.transform.position.z + 1.2f);

                GameObject fireBall = Instantiate(fireBallObj, frontPlayer, player01.transform.rotation) as GameObject;

                fireBall.GetComponent<Rigidbody>().AddForce(fireBallObj.transform.forward * 100, ForceMode.Impulse);

                shotFire = false;
  
            }

            //fireBallObj.transform.position = Vector3.MoveTowards(fireBallObj.transform.position, aimPos, 8.0f * Time.deltaTime);

           
        }

    }




IEnumerator startTurn()
{

    yield return new WaitForSeconds(2.0f);


}

}


