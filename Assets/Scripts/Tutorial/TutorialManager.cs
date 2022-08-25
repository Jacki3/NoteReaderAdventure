using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BoxCollider2D[] blockers;

    public SpriteRenderer hintRenderer;

    public NPCSpeech tutorialNPC;

    public List<Tutorial> tutorials = new List<Tutorial>();

    public int tutorialIndex = 0;

    private static TutorialManager instance;

    private NPCSpeech spawnedNPC; //instance of tutorial npc prefab

    public static bool tutorialSuccess;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public static void LoadTutorial()
    {
        instance.StartTutorial();
    }

    //this gets called if you and when you load the tutorial level
    public void StartTutorial()
    {
        tutorialNPC.gameObject.SetActive(true);
        ShowNextTutorial();
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
        tutorialSuccess = true;
        tutorialNPC.speech = tutorials[tutorialIndex].successText;
        tutorialNPC.speechIndex = 0;
        tutorialNPC.StartSpeech();
    }

    public static void ShowNextTutorialStatic()
    {
        instance.ShowNextTutorial();
    }

    private void ShowNextTutorial()
    {
        tutorialSuccess = false;
        hintRenderer.enabled = false;

        if (tutorialIndex >= tutorials.Count)
        {
            StartGame();
        }
        else
        {
            tutorialNPC.speech = tutorials[tutorialIndex].tutorialText;
            tutorialNPC.speechIndex = 0;
            tutorialNPC.StartSpeech();

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
        Destroy(tutorialNPC.gameObject);
        hintRenderer.enabled = false;
    }

    public static bool TutorialComplete() =>
        instance.tutorialIndex >= instance.tutorials.Count;
}
