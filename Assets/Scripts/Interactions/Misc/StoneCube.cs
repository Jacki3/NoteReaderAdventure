using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCube : MovingObject
{
    protected override void OnCantMove<T>(T Component, int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
