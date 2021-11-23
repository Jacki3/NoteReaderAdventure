using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject resumeButton;

    public void ShowMenu()
    {
        EventSystem.current.SetSelectedGameObject (resumeButton);
        GameStateController.PauseGame();
        if (GameStateController.gamePaused)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
