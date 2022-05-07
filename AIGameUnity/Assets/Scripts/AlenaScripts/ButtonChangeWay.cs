using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeWay : InteractableBase
{
    [SerializeField] private Roboarm roboarm;
    [SerializeField] private Texture sprite1;
    [SerializeField] private Texture sprite2;
    private Renderer renderer;
    private int num = 1;
    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        roboarm.buttonChangeWay = true;
        changeImage();
    }

    private void changeImage()
    {
        if (roboarm.onTheBorder)
        {
            switch (num)
            {
                case 1:
                    renderer.material.SetTexture("_EmissionMap", sprite2);
                    num++;
                    return;
                case 2:
                    renderer.material.SetTexture("_EmissionMap", sprite1);
                    num--;
                    return;
            }
        }
    }
}