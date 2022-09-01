using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPC : NPCSpeech
{
    private bool firstPress = true;

    private void Awake()
    {
        RigidPlayerController.inputActions.UI.Submit.performed += ctx =>
            NextTutorial();
    }

    private void NextTutorial()
    {
        if (CoreGameElements.i.useTutorial)
        {
            if (!GameStateController.gamePaused) StartSpeech();
            if (
                firstPress &&
                GameStateController.state == GameStateController.States.Tutorial
            )
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Play);
        }
    }

    public override void StartSpeech()
    {
        if (speechIndex == speech.Length || speech[0].Length <= 0)
        {
            RestartText();
        }
        else
        {
            base.StartSpeech();
        }
    }

    private void RestartText()
    {
        if (TutorialManager.tutorialSuccess)
            TutorialManager.ShowNextTutorialStatic();
        else
            speechIndex = 0;
    }
}
