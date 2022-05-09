using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Latern : NotationItem
{
    public Light2D laternLight;

    public float brightness;

    protected override void AllNotationsComplete()
    {
        if (GameStateController.state == GameStateController.States.Tutorial)
        {
            TutorialManager
                .CheckTutorialStatic(Tutorial.TutorialValidation.ReadMode);
        }
        base.AllNotationsComplete();
        LightUp();
    }

    private void LightUp()
    {
        StartCoroutine(FadeLight(brightness, 1));
    }

    IEnumerator FadeLight(float desiredIntensity, float duration)
    {
        float timeElapsed = 0;
        float currentBrightness = laternLight.intensity;

        while (timeElapsed < duration)
        {
            laternLight.intensity =
                Mathf
                    .Lerp(currentBrightness,
                    desiredIntensity,
                    timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        laternLight.intensity = desiredIntensity;
    }
}
