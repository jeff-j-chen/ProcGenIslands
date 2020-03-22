using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour {
    // Start is called before the first frame update
    public TextMeshProUGUI dateText;
    public float _changeTimer;
    private WaitForSeconds changeTimer;
    public float hour = 7;
    public int day = 1;
    public List<Color> dayColors;

    void Start() {
        changeTimer = new WaitForSeconds(_changeTimer);
        StartCoroutine(MoveThroughTime());
        for (int i = 0; i < 6; i++) {
            dayColors.Insert(0, dayColors[23]);
        }
    }

    private IEnumerator MoveThroughTime() {
        while (true) {
            int flooredHour = Mathf.FloorToInt(hour);
            if (hour > dayColors.Count) { hour = 0f; }
            if (flooredHour < dayColors.Count - 1) {
                GetComponent<SpriteRenderer>().color = Color.Lerp(dayColors[flooredHour], dayColors[flooredHour + 1], hour - flooredHour);
            }
            else if (flooredHour == dayColors.Count - 1) {
                GetComponent<SpriteRenderer>().color = Color.Lerp(dayColors[flooredHour], dayColors[0], hour - flooredHour);
            }
            hour += _changeTimer;
            if (Mathf.Abs(flooredHour - hour) <= 0.1) {
                if (flooredHour + 1 <= 12) { dateText.text = $"Day {day}, {flooredHour} AM"; } 
                else { dateText.text = $"Day {day}, {flooredHour - 12} PM"; } 
            }
            yield return changeTimer;
        }
    }
} 

