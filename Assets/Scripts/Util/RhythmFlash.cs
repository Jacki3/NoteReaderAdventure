using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmFlash : MonoBehaviour
{
    public Renderer[] tileMapsL;

    public Renderer[] tileMapsR;

    private float secPerBeat;

    void Start()
    {
        StartCoroutine(FlashTiles());
    }

    void Update()
    {
        secPerBeat = AudioController.secPerBeat;
    }

    private IEnumerator FlashTiles()
    {
        while (true)
        {
            for (int i = 0; i < tileMapsL.Length; i++)
            {
                tileMapsL[i].enabled = false;
                tileMapsR[i].enabled = true;
                FXController
                    .SetAnimatorTrigger_Static(FXController
                        .Animations
                        .BeatAnimator,
                    "Beat");
            }
            yield return new WaitForSeconds(secPerBeat);
            for (int i = 0; i < tileMapsL.Length; i++)
            {
                tileMapsL[i].enabled = true;
                tileMapsR[i].enabled = false;
                FXController
                    .SetAnimatorTrigger_Static(FXController
                        .Animations
                        .BeatAnimator,
                    "Beat");
            }
            yield return new WaitForSeconds(secPerBeat);
        }
    }
}
