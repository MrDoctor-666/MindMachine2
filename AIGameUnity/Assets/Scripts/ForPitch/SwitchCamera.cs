using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    private Canvas canvas;
    private Camera switchTo;
    void Start()
    {
        
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        //если у нас будет гейм контроллер, в которм будут как-то хранится камеры,
        // то будет брать mainCam оттуда
        switchTo = gameObject.transform.GetComponentInChildren<Camera>();
        switchTo.enabled = false;
    }

    private void OnMouseDown()
    {
        goToThisCamera();
    }

    public void goToThisCamera()
    {
        if (mainCam == null) mainCam = Camera.main;
        canvas.gameObject.SetActive(true);
        switchTo.enabled = true;
        mainCam.enabled = false;
        gameObject.GetComponent<Movement>().StartMoving();
    }

    public void BackToCamera()
    {
        gameObject.GetComponent<Movement>().EndMoving();
        canvas.gameObject.SetActive(false);
        switchTo.enabled = false;
        mainCam.enabled = true;
    }

    public void WaitForPuzzleSolve()
    {
        gameObject.GetComponent<Movement>().EndMoving();
        canvas.gameObject.SetActive(false);
        switchTo.enabled = false;
    }
}
