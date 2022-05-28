using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BoxCollider2D[] blockers;

    public SpriteRenderer hintRenderer;

    public NPCSpeech tutorialNPC;

    public List<Tutorial> tutorials = new List<Tutorial>();

    private int tutorialIndex = 0;

    private static TutorialManager instance;

    private NPCSpeech spawnedNPC; //instance of tutorial npc prefab

    private void Awake()
    {
        instance = this;
    }

    public static void CheckTutorialStatic(
        Tutorial.TutorialValidation validation
    )
    {
        instance.CheckTutorialComplete (validation);
    }

    private void CheckTutorialComplete(
        Tutorial.TutorialValidation tutorialValidation
    )
    {
        if (tutorialIndex < tutorials.Count)
        {
            if (tutorials[tutorialIndex].validation == tutorialValidation)
            {
                ShowSuccessText();
                if (tutorialIndex < tutorials.Count)
                {
                    tutorialIndex++;
                }
            }
        }
    }

    private void ShowSuccessText()
    {
        spawnedNPC.speech = tutorials[tutorialIndex].successText;
        spawnedNPC.speechIndex = 0;
        spawnedNPC.StartSpeech();
    }

    public static void ShowNextTutorialStatic()
    {
        instance.ShowNextTutorial();
    }

    private void ShowNextTutorial()
    {
        hintRenderer.enabled = false;

        if (tutorialIndex >= tutorials.Count)
        {
            StartGame();
        }
        else
        {
            spawnedNPC = Instantiate(tutorialNPC, Camera.main.transform);
            spawnedNPC.speech = tutorials[tutorialIndex].tutorialText;
            spawnedNPC.StartSpeech();
            spawnedNPC.state = GameStateController.States.Tutorial;

            var hintSprite = tutorials[tutorialIndex].hintSprite;
            if (hintSprite != null)
            {
                SetTutorialSprite();
            }
            else
                hintRenderer.enabled = false;
        }
    }

    private void SetTutorialSprite()
    {
        hintRenderer.enabled = true;
        hintRenderer.sprite = tutorials[tutorialIndex].hintSprite;
    }

    public static void StartGameStatic()
    {
        instance.StartGame();
    }

    private void StartGame()
    {
        GameStateController.state = GameStateController.States.Play;
        foreach (BoxCollider2D blocker in blockers) blocker.enabled = false;
    }
}
