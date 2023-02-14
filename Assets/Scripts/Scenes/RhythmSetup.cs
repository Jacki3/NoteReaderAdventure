using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmSetup : MonoBehaviour
{
    public RhythmFlash flashScript;

    public Renderer rhythmTilesR;

    public Renderer rhythmTilesL;

    void Start()
    {
        flashScript.danceFloorRight.Add (rhythmTilesR);
        flashScript.danceFloorLeft.Add (rhythmTilesL);
    }

    private void OnDestroy()
    {
        flashScript.danceFloorRight.Remove (rhythmTilesR);
        flashScript.danceFloorLeft.Remove (rhythmTilesL);
    }
}
