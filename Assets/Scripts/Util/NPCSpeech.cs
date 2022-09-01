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
