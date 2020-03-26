using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour {
    public GameObject cloudPrefab;
    public Sprite cloudLarge;
    public Sprite cloudMedium;
    public Sprite cloudSmall;
    public int cloudLimit;
    private Color temp = Color.white;

    private void Start() {
        for (int i = 0; i < cloudLimit; i++) {
            CreateCloud();
        }
    }
    
    private void CreateCloud() {
        GameObject createdCloud;
        createdCloud = Instantiate(cloudPrefab, new Vector2(Random.Range(-15, 15), Random.Range(-12, 12)), Quaternion.identity);
        int cloudRand = Random.Range(1, 4);
        if (cloudRand == 1) {
            temp.a = 0.3f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudLarge;
        }
        else if (cloudRand == 2) {
            temp.a = 0.2f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudMedium;
        }
        else {
            temp.a = 0.2f;
            createdCloud.GetComponent<SpriteRenderer>().sprite = cloudSmall;
        }
        createdCloud.GetComponent<SpriteRenderer>().color = temp;
        createdCloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.value * 2f, Random.value * 2f);
    }
}
