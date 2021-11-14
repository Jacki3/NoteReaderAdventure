using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : IUpdateMission
{
    public string name;

    public string decription;

    public int XPReward;

    public int coinReward;

    public int requiredAmount;

    public int currentAmount;

    public void Increment()
    {
        currentAmount++;
        missionComplete();
        Debug.Log("Collected");
        Debug.Log (currentAmount);
    }

    public bool missionComplete() => currentAmount >= requiredAmount;
}

public interface IUpdateMission
{
    void Increment();
}

public interface IPickup : IUpdateMission { }
