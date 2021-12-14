using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Latern : NotationItem, INotation
{
    public Light2D laternLight;

    public float brightness;

    public override void NotationComplete()
    {
        notationsCompleted++;

        if (notationsCompleted >= notationsToComplete)
        {
            base.NotationComplete();
            LightUp();
        }
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
