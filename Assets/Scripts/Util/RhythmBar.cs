using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBar : MonoBehaviour
{
    private float speed;

    private float distanceToNoteTarget;

    public void Start()
    {
        distanceToNoteTarget = transform.localPosition.x;
        if (distanceToNoteTarget < 0) distanceToNoteTarget *= -1;
    }

    void Update()
    {
        speed = distanceToNoteTarget / AudioController.secPerBeat;
        speed /= 3f;

        transform.localPosition =
            Vector2
                .MoveTowards(transform.localPosition,
                new Vector2(0, transform.localPosition.y),
                speed * Time.deltaTime);

        if (transform.localPosition.x == 0)
        {
            FXController
                .SetAnimatorTrigger_Static(FXController.Animations.BeatAnimator,
                "Beat");
            Destroy(this.gameObject);
        }
    }
}
