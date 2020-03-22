using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayandNightCycle : MonoBehaviour {
    // Start is called before the first frame update
    public int dayLength;   //in minutes
    public int nightLength;   //also in minutes
    private int currentTime;
    public float cycleSpeed;
    private bool isDay;
    private Color sky;

    // Day and Night Script for 2d,
    // Unity needs one empty GameObject (earth) and one Light (sun)
    // make the sun a child of the earth
    // reset the earth position to 0,0,0 and move the sun to -200,0,0
    // attach script to sun
    // add sun and earth to script publics
    // set sun to directional light and y angle to 90


    void Start()
    {
        isDay = false;
        StartCoroutine(TimeOfDay());
        sky = GetComponent<SpriteRenderer>().color;
    }

    void Update() {
        if(isDay == true) {
            sky = new Color(0f, 0f, 0f, 0f);
        }
        if (isDay == false) {
            sky = new Color(0f, 0f, 0f, 0.7f);
        }
    }

    IEnumerator TimeOfDay() {
        yield return new WaitForSeconds(dayLength);
        isDay = false;
    }
    IEnumerator TimeOfNight() {
        yield return new WaitForSeconds(nightLength);
        isDay = true;
    }
} 

