using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeech : MonoBehaviour
{
    public string[] speech;

    public GameObject speechBubble;

    public GameObject interactText;

    public TMPro.TextMeshPro text;

    public float timePerChar = .05f;

    public int speechIndex = 0;

    public AudioSource talkingSource;

    public GameStateController.States state;

    private TextWriter.TextWriterSingle textWriterSingle;

    private Animator animator;

    public bool isSpeaking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
    }

    void Update()
    {
        //ensure this is sets to true if you are loading tutorial otherwise it is false
        // if (CoreGameElements.i.useTutorial)
        // {
        //     if (
        //         RigidPlayerController
        //             .inputActions
        //             .UI
        //             .Submit
        //             .WasPressedThisFrame() ||
        //         Input.GetMouseButtonUp(0)
        //     )
        //     {
        //         if (!GameStateController.gamePaused) StartSpeech();
        //         if (
        //             firstPress &&
        //             GameStateController.state ==
        //             GameStateController.States.Tutorial
        //         )
        //             TutorialManager
        //                 .CheckTutorialStatic(Tutorial.TutorialValidation.Play);
        //     }
        // }
    }

    public virtual void StartSpeech()
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
                    isSpeaking = true;
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
    }

    public virtual void StartTalking()
    {
        talkingSource.Play();
    }

    public virtual void StopTalking()
    {
        isSpeaking = false;
        if (talkingSource != null) talkingSource.Stop();
        speechIndex++;
    }
}
