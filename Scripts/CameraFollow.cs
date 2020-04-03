using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Player player;
    private void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
