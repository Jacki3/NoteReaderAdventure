using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public Seed seed;

    public PauseMenu pauseMenu;

    public RigidPlayerController player;

    private static string DATA_PATH;

    private TimeSpan startTime;

    private TimeSpan endTime;

    private DateTime startDate;

    private DateTime endDate;

    void Awake()
    {
        startDate = DateTime.Now;
        startTime = DateTime.Now.TimeOfDay;
    }

    void Start()
    {
        DATA_PATH = Application.dataPath + "/save.txt";
        CoreGameElements.i.saveDeleted = false;
        Load();
    }

    private void Save()
    {
        int savedRhythmStreak = CoreGameElements.i.gameSave.rhythmStreak;
        int currentRhythmStreak = CoreGameElements.i.currentRhythmStreak;
        if (currentRhythmStreak > savedRhythmStreak)
            CoreGameElements.i.gameSave.rhythmStreak = currentRhythmStreak;
        int savedNoteStreak = CoreGameElements.i.gameSave.noteStreak;
        int currentNoteStreak = CoreGameElements.i.currentNoteStreak;
        if (currentNoteStreak > savedNoteStreak)
            CoreGameElements.i.gameSave.noteStreak = currentNoteStreak;

        TextureController.SaveItemsStatic();
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
            string saveString = File.ReadAllText(DATA_PATH);

            byte[] decodedBytes = Convert.FromBase64String(saveString);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);

            save = JsonUtility.FromJson<SaveFile>(decodedText);
            CoreGameElements.i.gameSave = save;

            //Load the game elements in (probably better to use for loop, find objects or elements that match and set them somehow?)
            // seed.boardController.SetDanceFloor();
            pauseMenu.SetAllSliders();
            pauseMenu.SetColour(false);
            HealthController.SetHealth();
            PlayerSkills.LoadAllSkills();
            UIController.LoadUIHearts();
            TextureController.LoadItemsStatic();
            TextureController.CreateItemButtonsFirst();
            TextureController
                .SetSpriteStatic(TextureController.Orientation.front,
                save.frontSprite,
                save.backSprite,
                save.sideSprite);
            UIController.UpdateIndexField(save.userIndex);
            InventoryController.LoadItemsStatic();

            int noteStreak = save.noteStreak;
            int rhythmStreak = save.rhythmStreak;
            UIController
                .UpdateTextUI(UIController.UITextComponents.noteStreak,
                noteStreak.ToString());
            UIController
                .UpdateTextUI(UIController.UITextComponents.rhythmStreak,
                rhythmStreak.ToString());
            LivesController.UpdateLivesUI();
        }
        else
        {
            //first time loading game, create the savegame object to be saved n loadedz
            save = new SaveFile();
            CoreGameElements.i.gameSave = save;
            ExperienceController.SetInitialLevel();
            HealthController.SetHealth();
            TextureController.CreateItemButtonsFirst();

            seed.SetLevels();
        }
        StartMenu.SetStartTextStatic(save.firstRun);
        StartMenu.UpdateButtonsStatic();
    }

    void OnApplicationQuit()
    {
        if (!CoreGameElements.i.saveDeleted) Save();

        if (CoreGameElements.i.gameSave != null)
        {
            if (CoreGameElements.i.gameSave.userIndex.Length > 0)
            {
                //how many coins, what XP level and what is the current level they are at?
                int coins = CurrencyController.GetTotalCoins();
                int playerLevel = ExperienceController.GetLevel();
                int levelReached = CoreGameElements.i.latetstLevel;

                string stats =
                    "Player Coins: " +
                    coins +
                    "," +
                    "Current XP Lvl: " +
                    playerLevel +
                    "," +
                    "Level Reached: " +
                    levelReached;

                endTime = DateTime.Now.TimeOfDay;
                endDate = DateTime.Now;

                TimeSpan diff = endTime - startTime;

                LogFile
                    .WriteCSV(startDate +
                    "," +
                    endDate +
                    "," +
                    "Time Played: " +
                    diff.ToString() +
                    "," +
                    stats +
                    System.Environment.NewLine);
            }
        }
    }
}
