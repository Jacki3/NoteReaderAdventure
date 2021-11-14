using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionGoal
{
    public GoalType goalType;

    public int requiredAmount;

    public int currentAmount;

    public bool goalComplete()
    {
        return (currentAmount >= requiredAmount);
    }
}

public enum GoalType
{
    Collect,
    Eliminate,
    Activate
}
