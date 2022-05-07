using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public bool isInMonologue { get; private set; }
    PlayerInput playerInput;
    [SerializeField] GameObject panel;
    [SerializeField] Image cutsceneImage;
    [SerializeField] float typingSpeed = 0.01f;
    [SerializeField] string folderRoute = "Cutscenes";

    Coroutine typing;
    GameObject dialogue;
    TextAsset inkFile;
    Story story;
    Text message;
    string characterName, characterNameColored, currentSentence;
    List<string> tags;
    CharacterInfo[] characterInfos;
    CharacterInfo curCharacterAnimation;
    private InputAction nextLineAction;
    bool isFullText = false;
    string cutsceneFolder = "";

    #region unityFunctions
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        EventAggregator.DialogueStarted.Subscribe(StartDialogue);

        nextLineAction = playerInput.currentActionMap.FindAction("NextDialogueLine");
        nextLineAction.performed += OnNextPressed;
    }

    void Start()
    {
        isInMonologue = false;
        message = panel.transform.GetComponentInChildren<Text>();
        if (cutsceneImage == null)
        {
            foreach (Image child in panel.GetComponentsInChildren<Image>(true))
                if (child.gameObject.name == "CutscenePanel") cutsceneImage = child;
        }
    }
    #endregion

    #region StartDialogue
    public void StartDialogue(GameObject dialogue)
    {
        isInMonologue = true;
        this.dialogue = dialogue;
        Debug.Log("Starting Conversation");
        EventAggregator.PanelOpened.Publish(panel);
        characterNameColored = "";
        characterName = "";
        if (!LoadDocument()) return;


        AdvanceDialogue();
    }

    public bool LoadDocument()
    {
        if (dialogue.GetComponent<ITrigger>() != null)
        {
            inkFile = dialogue.GetComponent<ITrigger>().inkFile;
            cutsceneFolder = dialogue.GetComponent<ITrigger>().cutsceneFolderName;
        }
        else inkFile = null;

        if (inkFile == null) { EndDialogue(); Debug.Log("Somethnig went wrong"); return false; }
        story = new Story(inkFile.text);
        tags = new List<string>();
        characterInfos = dialogue.GetComponentsInChildren<CharacterInfo>();
        //Debug.Log(characterInfos[1].character + "    " + characterInfos.Length);
        return true;
    }
    #endregion

    #region continueDialogue

    public void OnNextPressed(InputAction.CallbackContext context)
    {
        if (!isInMonologue) return;
        if (!isFullText)
        {
            StopCoroutine(typing);
            message.text = "";
            if (characterNameColored != "") message.text = characterNameColored + ":";
            message.text += currentSentence;
            isFullText = true;
        }
        else if (!story.canContinue) EndDialogue();
        else if (curCharacterAnimation == null || !curCharacterAnimation.isInAnimation) AdvanceDialogue();
    }

    void AdvanceDialogue()
    {
        currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        isFullText = false;
        //message.text = currentSentence;
        typing = StartCoroutine(TypeSentence(currentSentence));
    }

     IEnumerator TypeSentence(string sentence)
     {
        message.text = "";
        if (characterNameColored != "") message.text = characterNameColored + ":";
         //if (sentence[0] == '!') { message.fontStyle = FontStyle.Italic; sentence = sentence.Remove(0, 1); }
         foreach (char letter in sentence.ToCharArray())
         {
            message.text += letter;
            yield return new WaitForSeconds(typingSpeed);
         }
        isFullText = true;
         //yield return new WaitForSeconds(4);
         //DiaplayNextSentence();
     }
    #endregion

    #region tags
    void ParseTags()
    {
        tags = story.currentTags;
        if (story.currentText.Contains(":"))
        {
            characterName = story.currentText.Split(':')[0];
            characterNameColored = characterName;
            currentSentence = story.currentText.Split(':')[1];
        }
        else characterName = "";
        foreach (string t in tags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];

            switch (prefix.ToLower())
            {
                case "anim":
                    Debug.Log("Animation " + param);
                    SetAnimation(param);
                    break;
                case "color":
                    Debug.Log("Color " + param);
                    SetTextColor(param);
                    break;
                case "image":
                    Debug.Log("Image " + param);
                    SetNewCutsceneImage(param);
                    break;
            }
        }
    }

    void SetAnimation(string animName)
    {
        Characters character;
        if (!GameInfo.people.TryGetValue(characterName.ToLower(), out character)) return;
        Debug.Log(character);
        foreach (CharacterInfo ch in characterInfos)
        {
            if (ch.character == character) { 
                ch.PlayAnimation(animName);
                curCharacterAnimation = ch;
                break; 
            }
        }
    }

    void SetTextColor(string param)
    {
        characterNameColored = string.Format("<color={0}>{1}</color>", param, characterName);
    }

    void SetNewCutsceneImage(string param)
    {
        if (cutsceneFolder == null || cutsceneFolder == "") return;
        Debug.Log("Change Image In Cutscene");
        var sprite = Resources.Load<Sprite>(folderRoute + "/" + cutsceneFolder + "/" + param);
        if (sprite == null) return;
        cutsceneImage.sprite = sprite;
        cutsceneImage.gameObject.SetActive(true);
    }

    #endregion

    #region dialogueEnd
    void EndDialogue()
    {
        StopAllCoroutines();
        Debug.Log("End Dialogue");
        EventAggregator.PanelClosed.Publish();
        EventAggregator.DialogueEnded.Publish();
        Reset();
        isInMonologue = false;
    }

    private void Reset()
    {
        inkFile = null;
        story = null;
        tags = null;
        isFullText = false;
        typing = null;
        cutsceneImage.sprite = null;
    }
    #endregion
}
