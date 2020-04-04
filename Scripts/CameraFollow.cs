using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Player player;
    // use to link the player with this script
    private void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        // constantly follow the player's position
    }
}
