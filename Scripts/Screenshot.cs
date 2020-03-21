using System.Collections;
using UnityEngine;
using System;

public class Screenshot : MonoBehaviour {
    public GameObject screenshotPrefab;
    public GameObject borderPrefab;
    public Camera myCamera;
    private bool takeScreenShotOnNextFrame;
    int width;
    int height;
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    public bool lockScreenshot = false;
    private void Start() {
        myCamera = GetComponent<Camera>();
        // set the camera
    }

    private void OnPostRender() {
        // called after a frame has been rendered
        if (takeScreenShotOnNextFrame) {
            // if taking a screenshot next frame
            takeScreenShotOnNextFrame = false;
            // set to false the next frame
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // generate a new texture
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            // read the pixels of the texture
            byte[] byteArray = texture.EncodeToPNG();
            DateTime dateTime = DateTime.Now;
            System.IO.File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + $"/{dateTime.Year}-{dateTime.Month}-{dateTime.Day}_{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}.png", byteArray);
            // encode the texture to png and save it with the datetime
            // now create sprite
            texture.Apply();
            // apply texture
            texture.filterMode = FilterMode.Point;
            // point filter (not bilinear)
            texture.wrapMode = TextureWrapMode.Clamp;
            // clamp (not wrap)
            GameObject takenScreenshot = Instantiate(screenshotPrefab, new Vector2(0f, 0f), Quaternion.identity);
            // instantiate the taken screenshot
            takenScreenshot.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1f, 0u, SpriteMeshType.FullRect);
            // make the texture in to a sprite, and give it to the screenshot object
            takenScreenshot.transform.parent = transform;
            // parent the screenshot to the camera (so it follows it around)
            takenScreenshot.transform.localPosition = new Vector3(0f, 0f, 10f);
            // localposition with z of 10, so it's not right on the camera
            GameObject screenshotBorder = Instantiate(borderPrefab, new Vector2(0f, 0f), Quaternion.identity);
            // create the border 
            screenshotBorder.transform.parent = transform;
            screenshotBorder.transform.localPosition = new Vector3(0f, 0f, 10f);
            // same deal
            StartCoroutine(ScreenshotAnimation(takenScreenshot, screenshotBorder));
            // start coroutine for a simple animation
        }
    }

    public IEnumerator ScreenshotAnimation(GameObject takenScreenshot, GameObject screenshotBorder) {
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        // get various shaders
        print("screen capture noise");
        takenScreenshot.GetComponent<SpriteRenderer>().color = Color.black;
        // set to black
        yield return new WaitForSeconds(0.1f);
        // wait tiny bit
        takenScreenshot.GetComponent<SpriteRenderer>().color = Color.white;
        takenScreenshot.GetComponent<SpriteRenderer>().material.shader = shaderGUItext;
        // set to white
        yield return new WaitForSeconds(0.05f);
        // wait tiny bit
        takenScreenshot.GetComponent<SpriteRenderer>().material.shader = shaderSpritesDefault;
        // change to normal, so it flashes black-white-normal 
        yield return new WaitForSeconds(0.75f);
        // show picture on screen for a time
        for (int i = 0; i < 20; i++) {
            takenScreenshot.transform.localScale = new Vector3(takenScreenshot.transform.localScale.x - 0.003333f / 2f, takenScreenshot.transform.localScale.y  - 0.003333f / 2f, takenScreenshot.transform.localScale.z);
            screenshotBorder.transform.localScale = new Vector3(screenshotBorder.transform.localScale.x - 0.134f / 2f, screenshotBorder.transform.localScale.y - 0.134f / 2f, screenshotBorder.transform.localScale.z);
            yield return new WaitForSeconds(0.025f / 2f);
        }
        // scale the screenshot and border down over the course of 0.25s
        Destroy(takenScreenshot);
        Destroy(screenshotBorder);
        // destroy both gameobjects
    }

    public void TakeScreenShot(int passedWidth, int passedHeight) {
        width = passedWidth;
        height = passedHeight;
        takeScreenShotOnNextFrame = true;
        // specify height and width (maybe used later), let the camera know to take a screenshot next frame
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // take screenshot when space is pressed
            if (!lockScreenshot) { 
                lockScreenshot = true;
                StartCoroutine(UnlockScreenshot());
                TakeScreenShot(800, 600); 
            }
        }
    }

    public IEnumerator UnlockScreenshot() {
        yield return new WaitForSeconds(1.5f);
        lockScreenshot = false;
    }
}