using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeech : MonoBehaviour
{
    public string[] speech;

    public TMPro.TextMeshPro text;

    public float timePerChar = .05f;

    public int speechIndex = 0;

    public AudioSource talkingSource;

    public GameStateController.States state;

    private TextWriter.TextWriterSingle textWriterSingle;

    private bool firstPress = true;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (
            PlayerController.inputActions.UI.Submit.WasPressedThisFrame() ||
            Input.GetMouseButtonUp(0)
        )
        {
            StartSpeech();

            if (
                firstPress &&
                GameStateController.state == GameStateController.States.Tutorial
            )
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Play);
        }
    }

    public void StartSpeech()
    {
        if (speechIndex < speech.Length)
        {
            {
                if (textWriterSingle != null && textWriterSingle.IsActive())
                {
                    textWriterSingle.WriteAllDestroy();
                }
                else
                {
                    string message = speech[speechIndex];
                    if (!GameStateController.gamePaused) StartTalking();
                    textWriterSingle =
                        TextWriter
                            .AddWriter_Static(text,
                            message,
                            timePerChar,
                            true,
                            true,
                            StopTalking);
                }
            }
        }
        else
        {
            if (CoreGameElements.i.useTutorial)
            {
                GameStateController.state = GameStateController.States.Tutorial;
                TutorialManager.ShowNextTutorialStatic();
            }
            else
            {
                TutorialManager.StartGameStatic();
            }

            animator.SetTrigger("FInished");
            this.enabled = false;
        }
    }

    private void StartTalking()
    {
        talkingSource.Play();
    }

    private void StopTalking()
    {
        if (talkingSource != null) talkingSource.Stop();
        speechIndex++;
    }
}
