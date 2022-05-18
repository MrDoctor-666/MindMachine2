using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow3DLock : MonoBehaviour

{
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Vector3 cameraRotationRelative;
    [SerializeField] private Transform target;
    private float _maxDistance;
    public LayerMask obstacles;
    private Vector3 _position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        _maxDistance = Vector3.Distance(_position, target.position);
    }

    private void LateUpdate()
    {
     
          // transform.position = target.position + target.rotation * cameraOffset;
        //transform.rotation = target.rotation * Quaternion.Euler(cameraRotationRelative);
        ObstaclesReact();
    }

    private void ObstaclesReact()
    {
        var distance = Vector3.Distance(_position, target.position);
        RaycastHit hit;
        RaycastHit hit2;
        // Ray ray = new Ray(target.position, transform.position -target.position);
        // Ray ray2 = new Ray(transform.position, -transform.forward);
        // Debug.DrawRay(ray.origin, ray.direction*_maxDistance, Color.red);
        // Debug.DrawRay(ray2.origin, ray2.direction*0.1f, Color.green);
        if (Physics.Raycast(target.position, transform.position -target.position, out hit, _maxDistance, obstacles))
        {
            if (target.GetComponent<BoxCollider>().bounds.max.y+0.5f > hit.point.y)
            {
                Vector3 vector3 = new Vector3(hit.point.x, _position.y, hit.point.z);
                _position = vector3;
           }
            if (distance < _maxDistance && Physics.Raycast(_position, -transform.forward, out hit2,0.5f, obstacles))
            {
                Vector3 vector3 = new Vector3(hit2.point.x, _position.y, hit2.point.z);
                _position = vector3;
            }
        }
        else if (distance < _maxDistance && !Physics.Raycast(_position, -transform.forward, 0.1f, obstacles))
        {
            transform.position = target.position + target.rotation * cameraOffset;
            transform.rotation = target.rotation * Quaternion.Euler(cameraRotationRelative);
        }
       
        
    }
}
