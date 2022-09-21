using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AreaTrigger : MonoBehaviour
{
    public GameObject defaultArea;

    public GameObject newArea;

    public Light2D mainLight;

    public bool newAreaShowing;

    public void ShowArea(bool effectLights)
    {
        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.LevelFader,
            "BasicFade");
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        if (newAreaShowing)
        {
            newAreaShowing = false;
            newArea.SetActive(false);
            defaultArea.SetActive(true);
            if (effectLights) mainLight.enabled = true;
        }
        else
        {
            newAreaShowing = true;
            newArea.SetActive(true);
            defaultArea.SetActive(false);
            if (effectLights) mainLight.enabled = false;
        }
    }
}
