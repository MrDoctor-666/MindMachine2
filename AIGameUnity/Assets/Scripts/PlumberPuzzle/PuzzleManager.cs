using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject pipeMainParrent;
    public GameObject pipesParrentBranch1;
    public GameObject pipesParrentBranch2;
    public Pipe[] pipesBranch1;
    public Pipe[] pipesBranch2;

    private int numPipesBranch1 = 0;
    private int numPipesBranch2 = 0;

    private Camera mainCamera;
    private Camera pazzleCamera;
    [HideInInspector] public CreatePazzleGame puzzleCreate;

    private void Start()
    {
        numPipesBranch1 = pipesParrentBranch1.transform.childCount;
        numPipesBranch2 = pipesParrentBranch2.transform.childCount;

        pipesBranch1 = new Pipe[numPipesBranch1];
        pipesBranch2 = new Pipe[numPipesBranch2];

        for (int i = 0; i < pipesBranch1.Length; i++)
        {
            GameObject gameObjectPipe1 = pipesParrentBranch1.transform.GetChild(i).gameObject;
            Pipe pipe1 = gameObjectPipe1.GetComponent<Pipe>();
            if (pipe1 == null)
            {
                continue;
            }
            pipesBranch1[i] = pipe1;
        }

        for (int j = 0; j < pipesBranch2.Length; j++)
        {
            GameObject gameObjectPipe2 = pipesParrentBranch2.transform.GetChild(j).gameObject;
            Pipe pipe2 = gameObjectPipe2.GetComponent<Pipe>();
            if (pipe2 == null)
            {
                continue;
            }
            pipesBranch2[j] = pipe2;
        }
    }

    public void Win1()
    {
        for (int i = 0; i < pipesBranch1.Length; i++)
        {
            if (pipesBranch1[i].isPlaced == false)
            {
                return;
            }
        }
        Debug.Log("Win1");
        FinishPazzleGame();
    }

    public void Win2()
    {
        for (int j = 0; j < pipesBranch2.Length; j++)
        {
            if (pipesBranch2[j].isPlaced == false)
            {
                return;
            }
        }
        
        StartCoroutine(WaitForSec());
        
    }

    public void FinishPazzleGame()
    {
        //pazzleCamera = GameObject.FindGameObjectWithTag("PazzleCamera").GetComponent<Camera>();
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //mainCamera = puzzleCreate.mainCamera;
        pazzleCamera = puzzleCreate.pazzleCamera;
        Destroy(transform.parent.gameObject);
        pazzleCamera.enabled = false;
        //mainCamera.enabled = true;
        puzzleCreate.End();
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Win2");
        FinishPazzleGame();
    }
}
