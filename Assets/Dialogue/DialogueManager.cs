using System;
using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsPlaying {get;private set;}
    public bool canStartDialogue {get;private set;}
    private static DialogueManager instance;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        dialogueIsPlaying = false;
        canStartDialogue = true;
        dialoguePanel.SetActive(false);

        PlayerController.Interacted += ContinueStory;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Update() {
        if(!dialogueIsPlaying) return;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        canStartDialogue = false;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExtiDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        StartCoroutine(DialogueCoolDown());
    }

    private IEnumerator DialogueCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        canStartDialogue = true;
    }

    private void ContinueStory()
    {
        if(!dialogueIsPlaying) return;


        if(currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExtiDialogueMode();
        }
    }
}
