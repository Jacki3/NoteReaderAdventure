using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    public enum Effects
    {
        PlayerLevelUp
    }

    public enum Animations
    {
        PlayerAnimator,
        MissionAnimator,
        BeatAnimator,
        BeatFlash,
        LevelFader,
        NONE
    }

    public FX[] CoreEffects;

    [System.Serializable]
    public class FX
    {
        public Effects effectType;

        public Transform effectSpawn;

        public GameObject effectObject;
    }

    public AnimatorControllers[] animatorControllers;

    [System.Serializable]
    public class AnimatorControllers
    {
        public Animations animationType;

        public Animator animator;
    }

    private static FXController instance;

    private void Awake()
    {
        instance = this;
    }

    private void SpawnEffect(Effects effect)
    {
        FX currentFX = GetEffect(effect);

        var newFX = Instantiate(currentFX.effectObject, currentFX.effectSpawn);
        newFX.transform.position = currentFX.effectSpawn.position;
    }

    private FX GetEffect(Effects effect)
    {
        foreach (FX _effect in CoreEffects)
        {
            if (effect == _effect.effectType) return _effect;
        }
        return null;
    }

    public static void SpawnEffect_Static(Effects effects)
    {
        instance.SpawnEffect (effects);
    }

    private void SetAnimatorTrigger(Animations animations, string triggerName)
    {
        GetAnimator(animations).animator.SetTrigger(triggerName);
    }

    public static void SetAnimatorTrigger_Static(
        Animations animations,
        string triggerName
    )
    {
        instance.SetAnimatorTrigger (animations, triggerName);
    }

    private AnimatorControllers GetAnimator(Animations animations)
    {
        foreach (AnimatorControllers animatorControllers in animatorControllers)
        {
            if (animations == animatorControllers.animationType)
                return animatorControllers;
        }
        return null;
    }

    private IEnumerator
    LerpSlider(
        float endValue,
        float valueToChange,
        float duration,
        float sliderMax,
        UIController.UIImageComponents slider
    )
    {
        float time = 0;
        float startValue = valueToChange;

        while (time < duration)
        {
            valueToChange = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            UIController.UpdateSliderAmount (slider, sliderMax, valueToChange);
            yield return null;
        }
        valueToChange = endValue;
    }

    public static void LerpSlider_Static(
        float end,
        float change,
        float duration,
        float max,
        UIController.UIImageComponents slider
    )
    {
        instance
            .StartCoroutine(instance
                .LerpSlider(end, change, duration, max, slider));
    }

    private IEnumerator
    ExpandSliderBar(
        float endValue,
        float duration,
        UIController.UIImageComponents slider
    )
    {
        float time = 0;
        var tempSlider =
            CoreUIElements.i.GetSliderComponent(slider).transform.localScale;
        float valueToChange = tempSlider.x;

        float startValue = valueToChange;

        while (time < duration)
        {
            valueToChange = Mathf.Lerp(startValue, endValue, time / duration);
            tempSlider.x = valueToChange;
            CoreUIElements.i.GetSliderComponent(slider).transform.localScale =
                tempSlider;
            time += Time.deltaTime;
            yield return null;
        }
        valueToChange = endValue;
    }

    public static void ExpandSlider(
        float end,
        float duration,
        UIController.UIImageComponents slider
    )
    {
        instance
            .StartCoroutine(instance.ExpandSliderBar(end, duration, slider));
    }

    private IEnumerator
    ZoomCameraRoutine(float desiredCameraSize, float zoomDuration)
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(.75f, 2f, .5f, 1f);

        float timeElapsed = 0;
        float currentSize = Camera.main.orthographicSize;

        while (timeElapsed < zoomDuration)
        {
            Camera.main.orthographicSize =
                Mathf
                    .Lerp(currentSize,
                    desiredCameraSize,
                    timeElapsed / zoomDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.orthographicSize = desiredCameraSize;
    }

    public static void ZoomCamera(float cameraSize, float duration)
    {
        instance
            .StartCoroutine(instance.ZoomCameraRoutine(cameraSize, duration));
    }
}
