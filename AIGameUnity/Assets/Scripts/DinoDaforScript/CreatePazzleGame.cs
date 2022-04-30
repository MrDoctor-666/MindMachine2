using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePazzleGame : MonoBehaviour
{
    //public Camera mainCamera;
    [SerializeField] GameObject whereFrom;
    public Transform pazzleGamePrefab;
    public Camera pazzleCamera;
    public Vector3 constVector = new Vector3(-3.25f, -1.066755f, 8.32f);

    private void Start()
    {
        //mainCamera.enabled = true;
        pazzleCamera.enabled = false;
    }

    private void OnMouseDown()
    {
        whereFrom.GetComponent<SwitchCamera>().WaitForPuzzleSolve();
        //mainCamera.enabled = false;
        pazzleCamera.enabled = true;
        Transform createdPuzzle = Instantiate(pazzleGamePrefab, pazzleCamera.transform.position + constVector, Quaternion.identity);
        createdPuzzle.GetComponentInChildren<PuzzleManager>().puzzleCreate = this;
    }

    public void End()
    {
        whereFrom.GetComponent<SwitchCamera>().goToThisCamera();
    }
}