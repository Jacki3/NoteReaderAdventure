using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionHolder : MonoBehaviour
{
    private static MissionHolder _i;

    public static MissionHolder i
    {
        get
        {
            return _i;
        }
    }

    public List<Mission> currentMissions = new List<Mission>();

    private int totalMissions; //can track how many missions player has done and reward for completing all/badges etc.

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;

        // Item.increment += IncrementCurrentAmount; -- alternative solution could be events driven from parent classes?

        //UI:
        //loops through allMissions and sets text? (if amount = 1 then put an 'a')
    }

    public void IncrementCurrentAmount(Mission mission)
    {
        mission.currentAmount++;
        if (mission.missionComplete())
        {
            print("mission done!");
            mission.isActiveMission = false;
            //remove from list, cross out (update UI) etc.
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
                print("wrong item?");
        }
    }
}
