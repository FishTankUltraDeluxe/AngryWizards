using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour
{

    public GameObject sliderObj, player01, mousePointObj;


    private Slider slider;

    bool decreaseSlider;

    public float moveSpeed = 5;

    private Queue<GameObject> pointObjQueue = new Queue<GameObject>();
    private Queue<GameObject> pointEffectQueue = new Queue<GameObject>();

    public GameObject clickEffect;

    private GameObject waypoint;
    private GameObject effect;

    // Use this for initialization
    void Start()
    {

        decreaseSlider = true;

        slider = sliderObj.GetComponent<Slider>();

        waypoint = new GameObject();
        effect = new GameObject();

        waypoint.transform.position = player01.transform.position;   
    }

    // Update is called once per frame
    void Update()
    {

        //camera constantly looks at player
        //Camera.main.transform.LookAt(player01.transform);

        if (slider.value == 0)
        {
            decreaseSlider = false;
        }


        if (decreaseSlider)
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
            decreaseSlider = false;
            StartCoroutine(startTurn());
        }

        //determine if the slider should go down
        if (slider.value == 5)
        {
            decreaseSlider = true;
            StartCoroutine(startTurn());
        }



        if (Input.GetMouseButton(0))
        {

            //mousePos
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
            //getting mouse point based on camera drama
            screenPoint = Camera.main.ScreenToWorldPoint(screenPoint);


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               // Debug.DrawLine(ray.direction, ray.origin, Color.green, 15.0f);

                waypoint =  Instantiate(mousePointObj, hit.point, transform.rotation);

                effect = Instantiate(clickEffect, hit.point, transform.rotation);

                effect.GetComponent<ParticleSystem>().Emit(30);

            }


        }

        if (Vector3.Distance(player01.transform.position, waypoint.transform.position) > 1)
        {
            player01.transform.LookAt(waypoint.transform.position);

            player01.transform.position = Vector3.MoveTowards(player01.transform.position, waypoint.transform.position, moveSpeed * Time.deltaTime);
            effect.GetComponent<ParticleSystem>().Stop();
        }




    }


    IEnumerator startTurn()
    {

        yield return new WaitForSeconds(2.0f);


    }
}


