using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    private List<TextWriterSingle>
        textWriterSingles = new List<TextWriterSingle>();

    private static TextWriter instance;

    private void Awake()
    {
        instance = this;
    }

    public static TextWriterSingle
    AddWriter_Static(
        TMPro.TextMeshPro text,
        string textToWrite,
        float timePerChar,
        bool stableTextWrite,
        bool canRemoveEarly,
        Action onComplete
    )
    {
        if (canRemoveEarly) instance.RemoveWriter(text);
        return instance
            .AddWriter(text,
            textToWrite,
            timePerChar,
            stableTextWrite,
            onComplete);
    }

    private TextWriterSingle
    AddWriter(
        TMPro.TextMeshPro text,
        string textToWrite,
        float timePerChar,
        bool stableTextWrite,
        Action onComplete
    )
    {
        TextWriterSingle textWriterSingle =
            new TextWriterSingle(text,
                textToWrite,
                timePerChar,
                stableTextWrite,
                onComplete);
        textWriterSingles.Add (textWriterSingle);
        return textWriterSingle;
    }

    private void RemoveWriter(TMPro.TextMeshPro text)
    {
        for (int i = 0; i < textWriterSingles.Count; i++)
        {
            if (textWriterSingles[i].GetTextUI() == text)
            {
                textWriterSingles.RemoveAt (i);
                i--;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < textWriterSingles.Count; i++)
        {
            bool textComplete = textWriterSingles[i].Update();

            if (textComplete)
            {
                textWriterSingles.RemoveAt (i);
                i--;
            }
        }
    }

    public class TextWriterSingle
    {
        private TMPro.TextMeshPro text;

        private string textToWrite;

        private int charIndex;

        private float timePerChar;

        private float timer;

        private bool stableText;

        private Action onComplete;

        public TextWriterSingle(
            TMPro.TextMeshPro text,
            string textToWrite,
            float timePerChar,
            bool stableText,
            Action onComplete
        )
        {
            this.text = text;
            this.textToWrite = textToWrite;
            this.timePerChar = timePerChar;
            this.stableText = stableText;
            this.onComplete = onComplete;
            charIndex = 0;
        }

        public bool Update()
        {
            timer -= Time.deltaTime;
            while (timer < 0f)
            {
                timer += timePerChar;
                charIndex++;
                string moddedText;
                moddedText = textToWrite.Substring(0, charIndex);

                if (stableText)
                    moddedText +=
                        "<color=#00000000>" +
                        textToWrite.Substring(charIndex) +
                        "</color>";

                text.text = moddedText;

                if (charIndex >= textToWrite.Length)
                {
                    if (onComplete != null) onComplete();
                    return true;
                }
            }
            return false;
        }

        public TMPro.TextMeshPro GetTextUI()
        {
            return text;
        }

        public bool IsActive() => charIndex < textToWrite.Length;

        public void WriteAllDestroy()
        {
            text.text = textToWrite;
            charIndex = textToWrite.Length;
            if (onComplete != null) onComplete();
            instance.RemoveWriter (text);
        }
    }
}
