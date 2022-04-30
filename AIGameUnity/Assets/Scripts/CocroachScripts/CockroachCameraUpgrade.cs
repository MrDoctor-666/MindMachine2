using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CockroachCameraUpgrade : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Upgrade 1 activated");
        gameObject.transform.parent.GetComponentInChildren<PostProcessLayer>().enabled = false;
    }
}
