using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameHandler : MonoBehaviour
{
    public Seed seed;

    public PauseMenu pauseMenu;

    private static string DATA_PATH;

    void Start()
    {
        DATA_PATH = Application.dataPath + "/save.txt";
        CoreGameElements.i.saveDeleted = false;
        Load();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(CoreGameElements.i.gameSave);
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(json);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        File.WriteAllText (DATA_PATH, encodedText);
    }

    private void Load()
    {
        SaveFile save = null;
        if (File.Exists(DATA_PATH))
        {
            print("save file found");
            string saveString = File.ReadAllText(DATA_PATH);

            byte[] decodedBytes = Convert.FromBase64String(saveString);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);

            save = JsonUtility.FromJson<SaveFile>(decodedText);
            CoreGameElements.i.gameSave = save;

            //Load the game elements in (probably better to use for loop, find objects or elements that match and set them somehow?)
            CurrencyController.AddRemoveCoins(save.playerCoins, true);
            HealthController.SetHealth(save.playerHealth);
            seed.boardController.SetDanceFloor();
            pauseMenu.SetAllSliders();
            pauseMenu.SetColour(false);
        }
        else
        {
            //first time loading game, create the savegame object to be saved n loadedz
            save = new SaveFile();
            CoreGameElements.i.gameSave = save;

            foreach (Mission mission in MissionHolder.i.currentMissions)
            {
                save.allMissions.Add (mission);
            }

            ExperienceController.SetInitialLevel();

            seed.SetLevels();
        }
        StartMenu.SetStartTextStatic(save.firstRun);
        StartMenu.UpdateButtonsStatic();
    }

    void OnApplicationQuit()
    {
        if (!CoreGameElements.i.saveDeleted) Save();
    }
}
