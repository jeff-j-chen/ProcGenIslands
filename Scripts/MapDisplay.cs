using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {
    public Renderer textureRenderer;
    public void DrawTexture(Texture2D texture) {
        textureRenderer.sharedMaterial.mainTexture = texture;
        // set the shared material (not normal material, which allows for viewing in scene) to be the texture
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        // set the local scale
    }
}