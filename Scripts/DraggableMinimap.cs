using UnityEngine;
using System.Collections.Generic;

public class DraggableMinimap : MonoBehaviour {
    public Camera minimapCamera;
    // the camera used for viewing the minimap
    Vector3 downPoint;
    Vector3 curPoint;
    // points used to determine how much the player dragged their mouse

    private void OnMouseDown() {
        if (minimapCamera.enabled) { 
            // if the minimap is on
            downPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // when the mouse is pressed down, get the current position
        }
    }

    private void OnMouseDrag() {
        if (minimapCamera.enabled) { 
            // if the minimap is on
            curPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // get the current position of the mouse
            minimapCamera.transform.position = new Vector3((minimapCamera.transform.position.x + (downPoint.x - curPoint.x) / ((1f / minimapCamera.orthographicSize) * 500f)), (minimapCamera.transform.position.y + (downPoint.y - curPoint.y) / ((1f / minimapCamera.orthographicSize) * 500f)), -10f);
            // move the minimap based on player's mouse drag distance relative to the camera's orthographic size
            downPoint = curPoint;
            // reassign the downpoint
        }
    }
}
