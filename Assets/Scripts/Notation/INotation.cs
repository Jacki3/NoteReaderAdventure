using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INotation
{
    void NotationComplete();

    void PlayedCorrectNote();

    Transform GetTransform();

    int GetObjectScore();
}
