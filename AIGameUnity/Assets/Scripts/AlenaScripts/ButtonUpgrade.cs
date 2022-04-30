using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpgrade : MonoBehaviour
{
    Roboarm roboarm;

    private void Awake()
    {
        roboarm = transform.parent.GetComponent<Roboarm>();
        roboarm.ButtonUpgrade();
    }
}
