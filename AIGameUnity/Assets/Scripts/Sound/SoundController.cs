using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    //there are events for each field in comments

    [Header("AudioSources (SoundController's childrens)")]
    [SerializeField] private AudioSource livingRoomBackground;
    [SerializeField] private AudioSource engineRoomBackground;
    [SerializeField] private AudioSource puzzleBackground;
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private List<GameObject> audioChannels;

    [Header("AudioMixerGroups")]
    [SerializeField] private AudioMixerGroup devices;
    [SerializeField] private AudioMixerGroup effects;
    [SerializeField] private AudioMixerGroup voices;

    [Header("Background")]
    [SerializeField] private List<AudioClip> livingRoom;
    [SerializeField] private List<AudioClip> engineRoom;
    [SerializeField] private AudioClip puzzleScene;

    [Header("Devices")]
    [SerializeField] private AudioClip capture; //deviceBought event  
    [SerializeField] private AudioClip deviceSwitched; //DeviceSwitched event 
    [SerializeField] private AudioClip getItem; //getObjectEvent
    [SerializeField] private AudioClip putItem; //putObjectEvent
    [SerializeField] private AudioClip buyError; //DeviceBuyError

    [SerializeField] private AudioClip cockroachMoving; //startMoving, endMoving
    [SerializeField] private AudioClip cockroachJumping; //cockroachJump

    [SerializeField] private AudioClip deliveryMoving; //startMoving, endMoving

    [SerializeField] private AudioClip roboarmMoving;  //startMoving, endMoving
    [SerializeField] private AudioClip roboarmChangeWay; //changeWayEvent

    [Header("Other")]
    [SerializeField] private AudioClip toaster; //toasterEvent
    [SerializeField] private AudioClip elevator; //liftMovingEvent
    [SerializeField] private AudioClip buttonOnScene; //sceneButtonEvent
    [SerializeField] private AudioClip doorsOnScene; //sceneDoorsEvent

    [Header("Puzzle")]
    [SerializeField] private AudioClip startPuzzle; //puzzleStarted
    [SerializeField] private AudioClip snakeMoving; //snakeStartMoving,snakeEndMoving 
    [SerializeField] private AudioClip pushButton; //pushButton
    [SerializeField] private AudioClip getLight; //getLight
    [SerializeField] private AudioClip puzzleDoorOpened; //doorOpened
    [SerializeField] private AudioClip algorithmEnd; //puzzleEnded
    [SerializeField] private AudioClip sabotageEnd; //puzzleEnded

    [Header("UI")]
    [SerializeField] private float timeBetwChangeSnaps;
    [SerializeField] private AudioClip panelOpened; //PanelOpened
    [SerializeField] private AudioClip panelClosed; //PanelClosed
    [SerializeField] private AudioClip clickButtonUI; //clickButtonUI

    private SoundSource livingRoomSource;
    private SoundSource engineRoomSource;
    private List<SoundSource> soundSources;
    private List<AudioSource> audioSources;

    //Snapshots
    private AudioMixerSnapshot main;
    private AudioMixerSnapshot inMenu;


    private void Awake()
    {
        main = devices.audioMixer.FindSnapshot("Main");
        inMenu = devices.audioMixer.FindSnapshot("In Menu");
        moveSound.loop = true;

        livingRoomSource = livingRoomBackground.gameObject.GetComponent<SoundSource>();
        engineRoomSource = engineRoomBackground.gameObject.GetComponent<SoundSource>();

        EventSubsciption();
        soundSources = new List<SoundSource>();
        audioSources = new List<AudioSource>();
        for (int i = 0; i < audioChannels.Count; i++)
        {
            soundSources.Add(audioChannels[i].GetComponent<SoundSource>());
            audioSources.Add(audioChannels[i].GetComponent<AudioSource>());
            Debug.Log(soundSources[i]);
            Debug.Log(audioSources[i]);
        }
    }

    private void EventSubsciption()
    {
        EventAggregator.deviceBought.Subscribe(OnDeviceBought);
        EventAggregator.DeviceSwitched.Subscribe(OnDeviceSwitched);
        EventAggregator.getObjectEvent.Subscribe(OnGetObject);
        EventAggregator.putObjectEvent.Subscribe(OnPutObject);

        EventAggregator.startMoving.Subscribe(OnStartMoving);
        EventAggregator.endMoving.Subscribe(OnEndMoving);
        EventAggregator.cockroachJump.Subscribe(OnCockroachJump);
        EventAggregator.DeviceBuyError.Subscribe(OnBuyError);

        EventAggregator.changeWayEvent.Subscribe(OnChangeWay);
        EventAggregator.toasterEvent.Subscribe(OnToasterSwitch);
        EventAggregator.liftMovingEvent.Subscribe(OnElevator);
        EventAggregator.sceneDoorsEvent.Subscribe(OnDoors);
        EventAggregator.sceneButtonEvent.Subscribe(OnButton);


        EventAggregator.puzzleStarted.Subscribe(OnPuzzleStarted);
        EventAggregator.puzzleEnded.Subscribe(OnPuzzleEnded);
        EventAggregator.snakeMoving.Subscribe(OnSnakeMoving);
        EventAggregator.pushButton.Subscribe(OnPushButton);
        EventAggregator.getLight.Subscribe(OnGetLight);
        EventAggregator.doorOpened.Subscribe(OnDoorOpened);

        EventAggregator.PanelOpened.Subscribe(OnPanelOpened);
        EventAggregator.PanelClosed.Subscribe(OnPanelClosed);
        EventAggregator.clickButtonUI.Subscribe(OnClickButtonUI);
        EventAggregator.DeviceBuyTried.Subscribe(OnPanelOpened);
    }

    private void Update()
    {
        Background(livingRoomSource, livingRoom, livingRoomBackground);
        Background(engineRoomSource, engineRoom, engineRoomBackground);
    }

    private void Background(SoundSource soundSource, List<AudioClip> list, AudioSource audioSource)
    {
        if (soundSource.isFree)
        {
            if (list.Count != 0)
            { 
                int range = Random.Range(0, list.Count);
                audioSource.clip = list[range];
                audioSource.Play();
                soundSource.PlayCounterTime();
            }
        }
    }



    /*private void Background()
    {
        if (livingRoomSource.isFree)
        {
            int rangeLivingRoom = Random.Range(0, livingRoom.Count);
            livingRoomBackground.clip = livingRoom[rangeLivingRoom];
        }

        if (engineRoomSource.isFree)
        {
            int rangeEngineRoom = Random.Range(0, engineRoom.Count);
            engineRoomBackground.clip = engineRoom[rangeEngineRoom];
        }

        if (puzzleSceneSource.isFree)
        {
            int rangePuzzleScene = Random.Range(0, puzzleScene.Count);
            puzzleBackground.clip = puzzleScene[rangePuzzleScene];
        }



        switch (currentRoom)
            {
                case ("livingRoom"):
                    int rangeLivingRoom = Random.Range(0, livingRoom.Count);
                    roomSound.clip = livingRoom[rangeLivingRoom];
                    break;

                case ("engineRoom"):
                    int rangeEngineRoom = Random.Range(0, engineRoom.Count);
                    roomSound.clip = engineRoom[rangeEngineRoom];
                    break;

                case ("puzzleScene"):
                    roomSound.clip = puzzleScene;
                    break;
            }
            roomSound.Play();
            backgroundSoundSource.PlayCounterTime();

        }
    }*/

    private void Revise(AudioClip nameSound, AudioMixerGroup nameMixerGroup)
    {
        if (nameSound != null)
        {
            if (soundSources[0].isFree)
            {
                audioSources[0].clip = nameSound;
                audioSources[0].outputAudioMixerGroup = nameMixerGroup;
                audioSources[0].Play();
                soundSources[0].PlayCounterTime();
            }
            else if (soundSources[1].isFree)
            {
                audioSources[1].clip = nameSound;
                audioSources[1].outputAudioMixerGroup = nameMixerGroup;
                audioSources[1].Play();
                soundSources[1].PlayCounterTime();
            }
            else
            {
                Debug.Log("Don't have any sources");
            }
        }
    }

    private void OnDeviceBought()
    {
        Revise(capture, effects);
    }

    private void OnDeviceSwitched(GameObject device)
    {
        Revise(deviceSwitched, effects);
    }

    private void OnGetObject()
    {
        Revise(getItem, effects);
    }

    private void OnPutObject()
    {
        Revise(putItem, effects);
    }

    private void OnSnakeMoving()
    {
        moveSound.loop = false;
        moveSound.clip = snakeMoving;
        moveSound.Play();
    }

    private void OnStartMoving(GameObject device)
    {
        moveSound.loop = true;
        moveSound.outputAudioMixerGroup = devices;
        if (device.GetComponent<CockroachMove>() != null)
        {
            moveSound.clip = cockroachMoving;
            moveSound.Play();
        }
        else if (device.GetComponent<DeliveryMove>() != null)
        {
            Debug.Log("deliveryMove START");
            moveSound.clip = deliveryMoving;
            moveSound.Play();
        }
        else if (device.GetComponent<Roboarm>() != null)
        {
            moveSound.clip = roboarmMoving;
            moveSound.Play();
        }
        else
        {
            Debug.Log("Check OnStartMoving in SoundController");
        }
    }

    private void OnEndMoving(GameObject device)
    {
        moveSound.Stop();
    }

    private void OnCockroachJump()
    {
        Revise(cockroachJumping, devices);
        Debug.Log("jump sound");
    }

    private void OnBuyError(GameObject device)
    {
        Revise(buyError, effects);
    }

    private void OnChangeWay()
    {
        Revise(roboarmChangeWay, devices);
    }

    private void OnToasterSwitch()
    {
        Revise(toaster, effects);
    }

    private void OnElevator()
    {
        Revise(elevator, effects);
    }

    private void OnDoors()
    {
        Revise(doorsOnScene, effects);
    }

    private void OnButton()
    {
        Revise(buttonOnScene, effects);
    }


    private void OnPuzzleStarted(GameObject device)
    {

        main.TransitionTo(0);

        puzzleBackground.clip = puzzleScene;
        puzzleBackground.Play();
        livingRoomBackground.Stop();
        engineRoomBackground.Stop();
        Revise(startPuzzle, effects);
    }


    private void OnPuzzleEnded(PuzzleEnd end)
    {
        switch (end)
        {
            case PuzzleEnd.Algorithm:
                Revise(algorithmEnd, effects);
                break;

            case PuzzleEnd.Sabotage:
                Revise(sabotageEnd, effects);
                break;
        }

        puzzleBackground.Stop();
        livingRoomBackground.Play();
        engineRoomBackground.Play();

    }

    private void OnPushButton()
    {
        Revise(pushButton, effects);
    }

    private void OnGetLight()
    {
        Revise(getLight, effects);
    }

    private void OnDoorOpened()
    {
        Revise(puzzleDoorOpened, effects);
    }

    private void OnPanelOpened(GameObject panel)
    {
            Revise(panelOpened, effects);
            inMenu.TransitionTo(timeBetwChangeSnaps);
    }

    private void OnPanelClosed()
    {
            Revise(panelClosed, effects);
            main.TransitionTo(timeBetwChangeSnaps);        
    }

    private void OnClickButtonUI()
    {
        Revise(clickButtonUI, effects);
    }
}
