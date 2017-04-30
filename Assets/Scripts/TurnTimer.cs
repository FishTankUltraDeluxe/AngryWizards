using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour {

    public GameObject sliderObj, player01, mousePointObj;


    private Slider slider;

    bool decreaseSlider;

    public float moveSpeed = 5;

    private Stack<GameObject> pointObjQueue = new Stack<GameObject>();

	// Use this for initialization
	void Start () {

        decreaseSlider = true;

        slider = sliderObj.GetComponent<Slider>();


		
	}
	
	// Update is called once per frame
	void Update () {


        Camera.main.transform.LookAt(player01.transform);

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

        if (slider.value == 0)
        {
            decreaseSlider = false;
            StartCoroutine(startTurn());         
        }

        if(slider.value ==5)
        {
            decreaseSlider = true;
            StartCoroutine(startTurn());
        }

        /*
        if (Input.GetMouseButton(0))
        {
            
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.z, player01.transform.position.y);
            //Debug.Log(Input.mousePosition);
            Vector3 lookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            
            lookPos = lookPos - transform.position;
            mousePos.y = player01.transform.position.y;
            mousePos.z =  Vector3.Distance(Input.mousePosition, Camera.main.transform.position);

            float angle = Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg;

            player01.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down); // Turns Right
            player01.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up); //Turns Left
            player01.transform.LookAt(mousePos);
            player01.transform.position = Vector3.MoveTowards(player01.transform.position, mousePos, moveSpeed * Time.deltaTime);
            Debug.DrawRay(player01.transform.position, mousePos,  Color.red, 1.0f);



        }*/


        if(Input.GetMouseButton(0))
        {
            // Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.z, player01.transform.position.y);
            //Vector3 lookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
            screenPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            //screenPoint.y = 1;

            // mousePos.y = player01.transform.position.y;
            // mousePos.z = Vector3.Distance(Input.mousePosition, Camera.main.transform.position);

            //Instantiate(mousePointObj, mousePos, Quaternion.identity);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
               pointObjQueue.Push(Instantiate(mousePointObj, screenPoint, transform.rotation));


            
            

        }


        player01.transform.position = Vector3.MoveTowards(player01.transform.position, pointObjQueue.Peek().transform.position, moveSpeed * Time.deltaTime);


    }


    IEnumerator startTurn()
    {
        
        yield return new WaitForSeconds(2.0f);
       
        
    }
}


