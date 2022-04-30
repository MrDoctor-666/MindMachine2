using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthUpgrade : MonoBehaviour
{
    [SerializeField] Roboarm roboarm;
    [SerializeField] float newLenght;

    private void OnEnable()
    {
        roboarm.LenghtUpgrade(newLenght);
    }
}
