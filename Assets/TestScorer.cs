using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScorer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.pKey.wasPressedThisFrame)
        {
            ScoreController.AddScore_Static(10);
            ExperienceController.AddXP(10);
        }
    }
}
