using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InternetChecker : MonoBehaviour
{
    public float checkTime = 1;

    public GameObject warningScreen;

    public GameObject mainGame;

    public static bool hasInternet;

    private void Start()
    {
        StartCoroutine(CheckConnection());
    }

    IEnumerator CheckConnection()
    {
        while (true)
        {
            UnityWebRequest newRequest =
                new UnityWebRequest("https://www.google.com/"); //replace with your own server
            yield return newRequest.SendWebRequest();

            if (newRequest.error == null)
            {
                warningScreen.SetActive(false);
                mainGame.SetActive(true);
                hasInternet = true;
            }
            else
            {
                warningScreen.SetActive(true);
                mainGame.SetActive(false);
                hasInternet = false;
            }

            yield return new WaitForSeconds(checkTime);
        }
    }

    public void Quit()
    {
        GameStateController.Quit();
    }
}
