using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial
{
    public enum TutorialValidation
    {
        Move,
        ReadMode,
        Smash,
        Play,
        Menu
    }

    public TutorialValidation validation;

    public string tutorialName;

    public Sprite hintSprite;

    public string[] tutorialText;

    public string[] successText;
}
