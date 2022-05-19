using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    // Indicator icon
    public Image img;
    // The target (location, enemy, etc..)
    public Transform target;
    // To adjust the position of the icon
    public Vector3 offset;
    private Camera _camera;

    private void Awake()
    {
        EventAggregator.changeMissionWaypoint.Subscribe(RefreshTransformWaypoint);
        if (GameInfo.isUsingMarker == true) img.gameObject.SetActive(true);
        else img.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target.Equals(null))
        {
            img.enabled = false;
            return;
        }
        img.enabled = true;

       _camera = GameInfo.currentDevice.GetComponentInChildren<Camera>();
        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width
        float minX = img.GetPixelAdjustedRect().width / 2;
        
        // Maximum X position: screen width - half of the icon width
        float maxX = Screen.width - minX;
        // Minimum Y position: half of the height
        float minY = img.GetPixelAdjustedRect().height / 2;
        // Maximum Y position: screen height - half of the icon height
        float maxY = Screen.height - minY;
        

        // Temporary variable to store the converted position from 3D world point to 2D screen point
        Vector2 pos = _camera.WorldToScreenPoint(target.position + offset);

        
            if(Vector3.Dot((target.position - _camera.transform.position), _camera.transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if(pos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    pos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    pos.x = minX;
                }
            }
        
       
        // Check if the target is behind us, to only show the icon once the target is in front
       

        // Limit the X and Y positions
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        img.transform.position = pos;
        
    }

    private void RefreshTransformWaypoint(Transform transform, Vector3 offset)
    {
        target = transform;
        this.offset = offset;
    }

}