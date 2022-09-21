using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCSpeech
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            if (interactText.activeSelf == false) interactText.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("Player is in");
            if (
                RigidPlayerController
                    .inputActions
                    .UI
                    .Submit
                    .WasPressedThisFrame() ||
                Input.GetMouseButtonUp(0)
            )
            {
                print("player clicked");
                if (!GameStateController.gamePaused) StartSpeech();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopTalking();
            RestartText(true);
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
