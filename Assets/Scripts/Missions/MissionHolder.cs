using System.Collections;
using System.Collections.Generic;
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
            mission.missionObject;

        missionPlaceholders.Add (_missionPlaceholder);
    }

    public void IncrementCurrentAmount(Mission mission)
    {
        if (mission.isActiveMission)
        {
            mission.currentAmount++;
            MissionPlaceholder placeHolder = FindMissionPlaceholder(mission);

            string placeHolderText = placeHolder.missionText.text.text;

            //this is clunky and can be included with MissionComplete()?
            if (mission.currentAmount <= mission.requiredAmount)
            {
                placeHolderText =
                    placeHolderText
                        .Replace((mission.currentAmount - 1).ToString(),
                        mission.currentAmount.ToString());

                placeHolder.missionText.text.text = placeHolderText;
            }

            if (mission.missionComplete())
            {
                mission.isActiveMission = false;
                placeHolderText = "<s>" + placeHolderText;
                placeHolder.missionText.text.text = placeHolderText;
                ExperienceController.AddXP(mission.XPReward);
                CurrencyController.AddRemoveCoins(mission.coinReward, true);
                placeHolder
                    .GetComponent<Animator>()
                    .SetTrigger("MissionComplete");
                SoundController
                    .PlaySound(SoundController.Sound.MissionComplete);
            }
        }
    }

    public void CheckValidMission(Mission.Object missionObject)
    {
        foreach (Mission mission in currentMissions)
        {
            if (mission.missionObject == missionObject)
            {
                IncrementCurrentAmount (mission);
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
                    .Contains(mission.missionObject.ToString())
            ) return placeHolder;
        }
        return null;
    }
}
