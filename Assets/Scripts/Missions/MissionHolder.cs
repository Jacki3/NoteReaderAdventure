using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MissionHolder : MonoBehaviour
{
    public List<Mission> currentMissions = new List<Mission>();

    [Header("Placholder Settings")]
    public MissionPlaceholder missionPlaceholder;

    public Transform placeholderContainer;

    public int placeholderDistanceY = 30;

    private List<MissionPlaceholder>
        missionPlaceholders = new List<MissionPlaceholder>();

    private static MissionHolder _i;

    public static MissionHolder i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;

        // Item.increment += IncrementCurrentAmount; -- alternative solution could be events driven from parent classes?
    }

    private void Start()
    {
        int yDiff = 0;

        //loops through allMissions and sets text? (if amount = 1 then put an 'a')
        foreach (Mission mission in currentMissions)
        {
            CreateMissionPlaceholder (mission, yDiff);
            yDiff += placeholderDistanceY;
        }
    }

    private void CreateMissionPlaceholder(Mission mission, int yDiff)
    {
        MissionPlaceholder _missionPlaceholder =
            Instantiate(missionPlaceholder, placeholderContainer);

        _missionPlaceholder.transform.localPosition =
            new Vector2(missionPlaceholder.transform.localPosition.x,
                missionPlaceholder.transform.localPosition.y - yDiff);

        _missionPlaceholder.missionText.text.text =
            mission.missionType +
            " " +
            mission.currentAmount +
            "/" +
            mission.requiredAmount +
            " " +
            mission.missionObjectString;

        missionPlaceholders.Add (_missionPlaceholder);
    }

    public void LoadAllMissionsFromSave()
    {
        if (CoreGameElements.i.gameSave.allMissions.Count > 0)
        {
            currentMissions.Clear();
            currentMissions = DeepCopy(CoreGameElements.i.gameSave.allMissions);

            foreach (Mission savedMission in currentMissions)
            {
                MissionPlaceholder placeHolder =
                    FindMissionPlaceholder(savedMission);

                placeHolder.missionText.text.text =
                    savedMission.missionType +
                    " " +
                    savedMission.currentAmount +
                    "/" +
                    savedMission.requiredAmount +
                    " " +
                    savedMission.missionObjectString;

                if (savedMission.currentAmount > 0)
                {
                    if (
                        savedMission.currentAmount >=
                        savedMission.requiredAmount
                    )
                    {
                        string placeHolderText =
                            placeHolder.missionText.text.text;
                        placeHolderText = "<s>" + placeHolderText;
                        placeHolder.missionText.text.text = placeHolderText;
                        placeHolder.missionText.text.color = Color.white;
                        placeHolder
                            .GetComponent<Animator>()
                            .SetTrigger("MissionComplete");
                    }
                    else
                    {
                        placeHolder
                            .GetComponent<Animator>()
                            .SetTrigger("MissionIncrement");
                    }
                }
                else
                {
                    placeHolder.GetComponent<Animator>().SetTrigger("Reset");
                }
            }
        }
        else
        {
            foreach (Mission mission in currentMissions)
            {
                MissionPlaceholder placeHolder =
                    FindMissionPlaceholder(mission);

                string currentAmountString = (mission.currentAmount).ToString();
                string placeHolderText = placeHolder.missionText.text.text;

                placeHolderText =
                    placeHolderText.Replace(currentAmountString, "0");

                placeHolder.missionText.text.text = placeHolderText;

                mission.currentAmount = 0;
                mission.isActiveMission = true;

                placeHolder.GetComponent<Animator>().SetTrigger("Reset");
            }
        }
    }

    public void IncrementCurrentAmount(Mission mission, bool increment)
    {
        if (mission.isActiveMission)
        {
            string currentAmountString = "0";

            if (increment) mission.currentAmount++;
            MissionPlaceholder placeHolder = FindMissionPlaceholder(mission);

            string placeHolderText = placeHolder.missionText.text.text;

            //this is clunky and can be included with MissionComplete()?
            if (increment)
            {
                currentAmountString = (mission.currentAmount - 1).ToString();
            }

            if (mission.currentAmount <= mission.requiredAmount)
            {
                placeHolderText =
                    placeHolderText
                        .Replace(currentAmountString,
                        mission.currentAmount.ToString());

                placeHolder.missionText.text.text = placeHolderText;
            }

            if (mission.missionComplete())
            {
                mission.isActiveMission = false;
                placeHolderText = "<s>" + placeHolderText;
                placeHolder.missionText.text.text = placeHolderText;
                placeHolder.missionText.text.color = Color.white;
                ExperienceController.AddXP(mission.XPReward);
                CurrencyController.AddRemoveCoins(mission.coinReward, true);
                placeHolder
                    .GetComponent<Animator>()
                    .SetTrigger("MissionComplete");
                if (increment)
                    SoundController
                        .PlaySound(SoundController.Sound.MissionComplete);
            }
            else
            {
                placeHolder
                    .GetComponent<Animator>()
                    .SetTrigger("MissionIncrement");
            }
        }
    }

    public void CheckValidMission(Mission.Object missionObject)
    {
        foreach (Mission mission in currentMissions)
        {
            if (mission.missionObject == missionObject)
            {
                IncrementCurrentAmount(mission, true);
            }
            else
            {
                //??
            }
        }
    }

    //this is clunky and uses find
    public MissionPlaceholder FindMissionPlaceholder(Mission mission)
    {
        foreach (MissionPlaceholder placeHolder in missionPlaceholders)
        {
            if (
                placeHolder
                    .missionText
                    .text
                    .text
                    .Contains(mission.missionObjectString.ToString())
            ) return placeHolder;
        }
        return null;
    }

    public void SaveAllMissions()
    {
        CoreGameElements.i.gameSave.allMissions = DeepCopy(currentMissions);
    }

    public static T DeepCopy<T>(T item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize (stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        T result = (T) formatter.Deserialize(stream);
        stream.Close();
        return result;
    }
}
