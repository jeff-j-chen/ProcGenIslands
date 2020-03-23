using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour {
    public TextMeshProUGUI dateText;
    // the textmeshpro text for indicating the datetime
    public float _changeTimer;
    // how fast the time should change
    private WaitForSeconds changeTimer;
    // the waitforseconds of the change timer
    public float hour = 1;
    // current hour
    public int day = 1;
    // current day
    public List<Color> dayColors;
    // the list of colors ot use for the day night cycle
    // DO NOT RENAME

    void Start() {
        changeTimer = new WaitForSeconds(_changeTimer);
        // generate the waitforseconds 
        StartCoroutine(MoveThroughTime());
        // begin the coroutine
        for (int i = 0; i < 6; i++) {
            dayColors.Insert(0, dayColors[23]);
        }
        // rearrange the colors array, because i messed it up and manually changing everything is super tedious
    }

    private IEnumerator MoveThroughTime() {
        while (true) {
            // repeat forever
            int flooredHour = Mathf.FloorToInt(hour);
            // get the floored hour (1.56 -> 1)
            if (hour > 25) { hour = 1f; }
            // reset hour if necessary
            if (flooredHour < 25) {
                // if the floored hour is between 1 and 24
                GetComponent<SpriteRenderer>().color = Color.Lerp(dayColors[flooredHour], dayColors[flooredHour + 1], hour - flooredHour);
                // lerp the color between the time periods
            }
            else if (flooredHour == 25) {
                // if at the end hour
                GetComponent<SpriteRenderer>().color = Color.Lerp(dayColors[flooredHour], dayColors[0], hour - flooredHour);
                // lerp between start and end colors
            }
            hour += _changeTimer;
            // increment the hour by the amount we want to change by
            if (Mathf.Abs(flooredHour - hour) <= 0.1) {
                // if the hour just passed
                if (flooredHour <= 12) { dateText.text = $"Day {day}, {flooredHour} AM"; } 
                else { dateText.text = $"Day {day}, {flooredHour - 12} PM"; }
                // display the day and time 
            }
            yield return changeTimer;
            // wait changetimer seconds
        }
    }
} 

