using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameHandler : MonoBehaviour
{
    void Start()
    {
        Load();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) HealthController.RemoveHealth(1, true);
        if (Input.GetKeyDown(KeyCode.P)) Save();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(CoreGameElements.i.gameSave);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    private void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            print("save file found");
            string saveString =
                File.ReadAllText(Application.dataPath + "/save.txt");

            SaveFile save = JsonUtility.FromJson<SaveFile>(saveString);
            CoreGameElements.i.gameSave = save;

            //Load the game elements in (probably better to use for loop, find objects or elements that match and set them somehow?)
            CurrencyController.AddRemoveCoins(save.playerCoins, true);
            HealthController.SetHealth(save.playerHealth);
        }
        else
        {
            //first time loading game, create the savegame object to be saved n loadedz
            SaveFile save = new SaveFile();
            CoreGameElements.i.gameSave = save;
        }
    }
}
