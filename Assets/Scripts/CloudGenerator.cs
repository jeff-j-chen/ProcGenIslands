using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour {
    public GameObject cloudPrefab;
    // the prefab to instantiate
    public Sprite cloudLarge;
    public Sprite cloudMedium;
    public Sprite cloudSmall;
    // sprites for large, medium, and small clouds
    public int cloudLimit;
    // the maximum number of clouds the screen can contain
    private Color temp = Color.white;
    // cached color, used to save memory

    private void Start() {
        for (int i = 0; i < cloudLimit; i++) {
            // on start create the number of clouds the player wants
            CreateCloud();
        }
    }
    
    private void CreateCloud() {
        GameObject createdCloud = Instantiate(cloudPrefab, new Vector2(Random.Range(-15, 15), Random.Range(-12, 12)), Quaternion.identity);
        createdCloud.layer = 6;
        // create a gameobject from the prefab at a random location
        int cloudRand = Random.Range(1, 4);
        // a random from 1-3, used for choosing the cloud's size
        if (cloudRand == 1) {
            temp.a = 0.4f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudLarge;
        }
        else if (cloudRand == 2) {
            temp.a = 0.3f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudMedium;
        }
        else {
            temp.a = 0.2f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudSmall;
        }
        // assign alpha values and sprites based on the random number
        createdCloud.GetComponent<SpriteRenderer>().color = temp;
        // actually apply the alpha value to the sprite
        createdCloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.value * 2f, Random.value * 2f);
        // give it a velocity (make this based on the wind later on)
    }
}
