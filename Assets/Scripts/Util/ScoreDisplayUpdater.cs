using System.Collections;
using System.Management.Instrumentation;
using UnityEngine;

public class ScoreDisplayUpdater : MonoBehaviour
{
    private static ScoreDisplayUpdater instance;

    private bool stopRoutine;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator
    ScoreUpdater(int addedAmount, UIController.UITextComponents textComponent)
    {
        var text = CoreUIElements.i.GetTextComponent(textComponent);
        var displayScore = int.Parse(text.text);

        while (true)
        {
            if (instance.stopRoutine)
            {
                yield break;
            }

            if (displayScore < addedAmount)
            {
                displayScore++;
                text.text = displayScore.ToString();
            }

            //this should reflect how big the gap is
            yield return new WaitForSeconds(0.025f);
        }
    }

    private IEnumerator
    ScoreUpdaterDown(
        int addedAmount,
        UIController.UITextComponents textComponent
    )
    {
        var text = CoreUIElements.i.GetTextComponent(textComponent);
        var displayScore = int.Parse(text.text);

        while (true)
        {
            if (instance.stopRoutine)
            {
                yield break;
            }
            if (displayScore > addedAmount)
            {
                displayScore--;
                text.text = displayScore.ToString();
            }

            //this should reflect how big the gap is
            yield return new WaitForSeconds(0.025f);
        }
    }

    public static void StartRoutine(
        int addedAmount,
        UIController.UITextComponents textComponent
    )
    {
        instance.stopRoutine = false;
        instance
            .StartCoroutine(instance.ScoreUpdater(addedAmount, textComponent));
    }

    public static void StartRoutineDown(
        int addedAmount,
        UIController.UITextComponents textComponent
    )
    {
        instance
            .StartCoroutine(instance
                .ScoreUpdaterDown(addedAmount, textComponent));
    }

    public static void StopRoutine()
    {
        instance.stopRoutine = true;
        UIController.UpdateTextUI(UIController.UITextComponents.scoreText, "0");
    }
}
