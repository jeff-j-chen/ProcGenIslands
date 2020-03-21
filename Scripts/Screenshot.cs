using System.Collections;
using UnityEngine;
public class Screenshot : MonoBehaviour {
    private static Screenshot instance;
    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;
    public SpriteRenderer onscreen;
    public string screenShotName = "CameraScreenshot.png";
    int width = Screen.width;   // for Taking Picture
    int height = Screen.height;
    Texture2D screenshot;
    Texture2D LoadScreenshot;

    public void Awake() {
        instance = this;
        myCamera = GetComponent<Camera>();
        onscreen = GetComponentInChildren<SpriteRenderer>();
        onscreen.enabled = true;
    }

    private void OnPostRender() {
        if (takeScreenshotOnNextFrame) {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
            Debug.Log("Save CameraScreenshot.png");
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
            this.LoadImage();
            
        }
    }
    
    private void TakeScreenShot(int width, int height) {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
        
        onscreen.enabled = false;

        StartCoroutine(WaitOnScreen());
    }

    public static void TakeScreenShot_Static(int width, int height) {
        instance.TakeScreenShot(width, height);
    }

    public void LoadImage() {
        string path = Application.persistentDataPath + "/" + screenShotName;
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(path);
        LoadScreenshot = new Texture2D(4, 4);
        LoadScreenshot.LoadImage(bytes);
        Sprite sprite = Sprite.Create(screenshot, new Rect(0, 0, width, height), new Vector2(0.5f, 0.0f), 1.0f);

        GameObject.Find("Picture").GetComponent<SpriteRenderer>().sprite = sprite;

        Debug.Log("LOAD IMAGE");
    }

    IEnumerator WaitOnScreen() {
        yield return new WaitForSeconds(1f);
        onscreen.enabled = true;
    }

}
