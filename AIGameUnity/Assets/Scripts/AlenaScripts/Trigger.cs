using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private Roboarm roboarm;
    [SerializeField] private BezierSpline rail0;
    [SerializeField] private BezierSpline rail1;
    [SerializeField] private float progRail0;
    [SerializeField] private float progRail1;

    private void OnTriggerEnter(Collider other)
    {
        roboarm.onTheBorder = true;
        roboarm.rail0To1 = progRail1;
        roboarm.rail1To0 = progRail0;
        roboarm.splinesOfRails[0] = rail0;
        roboarm.splinesOfRails[1] = rail1;
    }

    private void OnTriggerExit(Collider other)
    {
        roboarm.onTheBorder = false;
        roboarm.splinesOfRails[0] = null;
        roboarm.splinesOfRails[1] = null;
    }




}
