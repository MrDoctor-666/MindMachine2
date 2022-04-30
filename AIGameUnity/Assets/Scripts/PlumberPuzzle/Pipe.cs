using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pipe : MonoBehaviour
{
    private int numOfPossibleAngles = 1;
    private float[] rotationsPos = { 0, 90, 180, 270 };
    private float[] correctAngles;
    public bool isPlaced;

    private SpriteRenderer spriteR;
    private Sprite sprite;
    private string nameSprite;

    PuzzleManager puzzleManager;

    private void Awake()
    {
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        CheckSprite();
    }
    private void Start()
    {
        int rotationIndex = Random.Range(0, rotationsPos.Length);
        transform.eulerAngles = new Vector3(0, 0, rotationsPos[rotationIndex]);
        numOfPossibleAngles = correctAngles.Length;
        SetCorrectPosition();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void CheckSprite()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        sprite = spriteR.sprite;
        nameSprite = sprite.texture.name;
        if (nameSprite == "straight")
        {
            float a = Math.Abs(transform.eulerAngles.z);
            float b = a + 180;
            float[] massiveCorrectAngles = { a, b };
            correctAngles = massiveCorrectAngles;
        }
        else if (nameSprite == "conor")
        {
            float a = Math.Abs(transform.eulerAngles.z);
            float[] massiveCorrectAngles = { a };
            correctAngles = massiveCorrectAngles;
        }
        
    }
    private void SetCorrectPosition()
    {
        if (numOfPossibleAngles > 1)
        {
            if (Math.Abs(transform.eulerAngles.z - correctAngles[0]) < 1.0 || Math.Abs(transform.eulerAngles.z - correctAngles[1]) < 1.0)
            {
                isPlaced = true;
            }
        }
        else
        {
            if (Math.Abs(transform.eulerAngles.z - correctAngles[0]) < 1.0)
            {
                isPlaced = true;
            }
        }
    }


    private void CheckCorrectPosition()
    {
        if (numOfPossibleAngles > 1)
        {
            if (Math.Abs(transform.eulerAngles.z - correctAngles[0]) < 1.0 || Math.Abs(transform.eulerAngles.z - correctAngles[1]) < 1.0 && isPlaced == false)
            {
                isPlaced = true;
                WinCheck();
            }
            else if (isPlaced)
            {
                isPlaced = false;
            }
        }
        else
        {
            if (Math.Abs(transform.eulerAngles.z - correctAngles[0]) < 1.0 && isPlaced == false)
            {
                isPlaced = true;
                WinCheck();
            }
            else if (isPlaced)
            {
                isPlaced = false;
            }
        }
    }

    private void WinCheck()
    {
        if (this.transform.parent.name == "PipesEnd1")
        {
            puzzleManager.Win1();
        }
        else if (this.transform.parent.name == "PipesEnd2")
        {
            puzzleManager.Win2();
        }
        
    }

    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        CheckCorrectPosition();
    }
}
