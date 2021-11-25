using System.Collections;
using UnityEngine;

public class ScoreDisplayUpdater : MonoBehaviour
{
    private static ScoreDisplayUpdater instance;

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
            if (displayScore < addedAmount)
            {
                displayScore++;
                text.text = displayScore.ToString();
            }

            //this should reflect how big the gap is
            yield return new WaitForSeconds(0.05f);
        }
    }

    public static void StartRoutine(
        int addedAmount,
        UIController.UITextComponents textComponent
    )
    {
        instance
            .StartCoroutine(instance.ScoreUpdater(addedAmount, textComponent));
    }
}
