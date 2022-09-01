using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmFlash : MonoBehaviour
{
    public List<Renderer> danceFloorLeft = new List<Renderer>();

    public List<Renderer> danceFloorRight = new List<Renderer>();

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
            FXController
                .SetAnimatorTrigger_Static(FXController.Animations.BeatAnimator,
                "Beat");

            for (int i = 0; i < danceFloorRight.Count; i++)
            {
                danceFloorRight[i].enabled = false;
            }
            for (int i = 0; i < danceFloorLeft.Count; i++)
            {
                danceFloorLeft[i].enabled = true;
            }

            yield return new WaitForSeconds(secPerBeat);

            FXController
                .SetAnimatorTrigger_Static(FXController.Animations.BeatAnimator,
                "Beat");

            for (int i = 0; i < danceFloorLeft.Count; i++)
            {
                danceFloorLeft[i].enabled = false;
            }
            for (int i = 0; i < danceFloorRight.Count; i++)
            {
                danceFloorRight[i].enabled = true;
            }
            yield return new WaitForSeconds(secPerBeat);
        }
    }
}
