using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigSliderControl : MonoBehaviour
{
    // slider for the dig
    private GameObject DigSliderObj;
    // object for the player
    private GameObject PlayerObj;
    // tempary object for a grave
    private GameObject TempGraveObj;
    // slider for that shows the dig progress
    private GameObject DigProgressBar;

    // change per update for slider
    private float DigSlideDelta = 2f;
    // boolean based on if button to dig was pressed
    private bool Slide = false;
    // min/max of range of sucessful dig
    private float TargetMax = 0.75f;
    private float TargetMin = 0.35f;
    // key to press to dig
    private KeyCode DigKey = KeyCode.Space;
    // the changes per dig, cann be changed based on soil hardness
    private float DigProgressChange = 0.2f;
    //dig CD
    private bool DigCooldown = false;
    //dig CD time
    public float DigCooldownTime = .5f;


    // Start is called before the first frame update
    void Start()
    {
        DigSliderObj = GameObject.Find("DigSlider");
        PlayerObj = GameObject.Find("Player");
        TempGraveObj = GameObject.Find("Grave");
        DigProgressBar = GameObject.Find("DigProgressSlider");
    }

    // Update is called once per frame
    void Update()
    {

        // changes the direction when hits edge of slider
        if (DigSliderObj.GetComponent<Slider>().value == DigSliderObj.GetComponent<Slider>().maxValue)
        {
            DigSlideDelta *= -1;
        } else if (DigSliderObj.GetComponent<Slider>().value == DigSliderObj.GetComponent<Slider>().minValue)
        {
            DigSlideDelta *= -1;
        }
        // Checks if if not pressed than adds the change
        if (!Slide)
        {
            DigSliderObj.GetComponent<Slider>().value += DigSlideDelta * Time.deltaTime;

         
        }
        // Checks for the DigKey to be pressed
        if (Input.GetKeyDown(DigKey) && DigCooldown == false)
        {
            Slide = true;
            Debug.Log("I dug");
            DigCooldown = true;
            StartCoroutine("Physics");

        }
        // if DigKey was pressde then checks if within range
        if (Slide)
        {
            if (DigSliderObj.GetComponent<Slider>().value > TargetMin && DigSliderObj.GetComponent<Slider>().value < TargetMax)
            {
                //Debug.Log("Dig " + DigSliderObj.GetComponent<Slider>().value);
                DigProgressBar.GetComponent<Slider>().value += DigProgressChange * 2;
                Debug.Log("Dig " + DigProgressBar.GetComponent<Slider>().value);
            } else
            {
                //Debug.Log("Weak" + DigSliderObj.GetComponent<Slider>().value);
                DigProgressBar.GetComponent<Slider>().value += DigProgressChange;
                Debug.Log("Weak" + DigProgressBar.GetComponent<Slider>().value);
            }
            Slide = false;
        }  
    }

    private IEnumerator Physics()
    {

        Debug.Log("bro you cant dig that fast");
        

        yield return new WaitForSeconds(.75f);
        DigCooldown = false;
    }
}
