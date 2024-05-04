using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField]
    private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    bool playerInRange = false;

    private void Awake() {
        visualCue.SetActive(false);
    }

    private void Start() {
        PlayerController.Interacted += OnInteraction;
    }

    private void OnDisable() {
        PlayerController.Interacted -= OnInteraction;
    }


    private void Update() {
        if(playerInRange)
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") playerInRange = false;
    }

    private void OnInteraction()
    {
        if(playerInRange && DialogueManager.GetInstance().canStartDialogue)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}
