using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField]
    private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Store Gameobject")]
    [SerializeField] GameObject store;

    private Story currentStory;
    public bool dialogueIsPlaying {get;private set;}
    public bool canStartDialogue {get;private set;}
    private static DialogueManager instance;

    public static event Action onSellingItems;

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

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void OnDisable() {
        PlayerController.Interacted -= ContinueStory;
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
        currentStory.BindExternalFunction("openBuyMenu",OpenBuyMenu);
        currentStory.BindExternalFunction("openSellMenu",OpenSellMenu);
        
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
            DisplayChoices();
        }
        else if(currentStory.currentChoices.Count == 0)
        {
            ExtiDialogueMode();
        }

    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than the UI can support");
        }

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        
    }

    private void OpenBuyMenu()
    {
        store.SetActive(true);
        ExtiDialogueMode();
    }

    private void OpenSellMenu()
    {
        onSellingItems?.Invoke();
    }
}
