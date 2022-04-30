using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    private GameObject curDevice;
    [SerializeField] Camera puzzleCamera;
    [SerializeField] List<GameObject> puzzleLevels = new List<GameObject>();

    int curPuzzleLevel = 0;
    GameObject puz;
    private void Awake()
    {
        EventAggregator.puzzleStarted.Subscribe(OnPuzzleStarted);
        EventAggregator.puzzleEnded.Subscribe(OnPuzzleEnded);
        puzzleCamera.enabled = false;
        //puzzleCamera.GetComponent<PlayerInput>().enabled = false;
        puzzleCamera.gameObject.SetActive(false);
    }

    public void OnPuzzleStarted(GameObject device)
    {
        curDevice = device;
        //start puzzle
        puzzleCamera.gameObject.GetComponent<AudioListener>().enabled = true;

        if (curPuzzleLevel >= puzzleLevels.Count) curPuzzleLevel = puzzleLevels.Count - 1;
        puz = Instantiate(puzzleLevels[curPuzzleLevel]);
        
        curPuzzleLevel++;
        puz.transform.position = puzzleCamera.transform.position + new Vector3(0, 0, 10);
        puzzleCamera.GetComponent<SnakeManagerNew>().Start();
        //Camera curCamera = device.GetComponentInChildren<Camera>();
        //curCamera.enabled = false;
        GameInfo.currentDevice.GetComponentInChildren<Camera>().enabled = false;
        puzzleCamera.gameObject.SetActive(true);
        puzzleCamera.enabled = true;
        Cursor.visible = true;


        //puzzleCamera.GetComponent<PlayerInput>().enabled = true;
        //puzzleCamera.GetComponent<SnakeManagerNew>().NewStart();
    }

    public void OnPuzzleEnded(PuzzleEnd puzzleEnd)
    {

        //puzzleCamera.GetComponent<PlayerInput>().enabled = false;
        puzzleCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        puzzleCamera.GetComponent<Camera>().enabled = false;
        GameInfo.currentDevice.GetComponentInChildren<Camera>().enabled = true;
        //curDevice.GetComponentInChildren<Camera>().enabled = true;
        puzzleCamera.gameObject.SetActive(false);

        //add suspicion and/or computing power
        Debug.Log("Puzzle ended with " + puzzleEnd);
        Debug.Log(curDevice + " ");
        switch (puzzleEnd)
        {
            case PuzzleEnd.Algorithm:
                GameInfo.DecreaseSuspicion(curDevice.GetComponent<DeviceInfo>().minusSuspitionAlgorithm);
                break;
            case PuzzleEnd.Sabotage:
                GameInfo.IncreaseSuspicion(curDevice.GetComponent<DeviceInfo>().suspicionSabotage);
                GameInfo.IncreaseCompPower(curDevice.GetComponent<DeviceInfo>().prizeSabotge);
                break;
        }
        //open some panel with info 
        //EventAggregator.PanelOpened.Publish(GetComponentInChildren<Image>(true).gameObject);

        Destroy(puz);
        Debug.Log(GameInfo.computingPower + "  " + GameInfo.suspicion);
        curDevice = null;
    }

}
