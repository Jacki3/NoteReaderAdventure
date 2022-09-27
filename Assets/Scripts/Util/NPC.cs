using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCSpeech
{
    private bool playerInRange;

    private void Start()
    {
        RigidPlayerController.inputActions.UI.Submit.performed += ctx =>
            ShowNextSpeech();
    }

    private void ShowNextSpeech()
    {
        if (playerInRange) if (!GameStateController.gamePaused) StartSpeech();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (interactText.activeSelf == false) interactText.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopTalking();
            RestartText(true);
            playerInRange = false;
        }
    }

    public override void StartSpeech()
    {
        if (speechIndex == speech.Length)
        {
            RestartText(false);
        }
        else
        {
            base.StartSpeech();
            interactText.SetActive(false);
            speechBubble.SetActive(true);
        }
    }

    private void RestartText(bool isLeaving)
    {
        if (!isLeaving)
            interactText.SetActive(true);
        else
            interactText.SetActive(false);
        speechBubble.SetActive(false);
        speechIndex = 0;
    }
}
