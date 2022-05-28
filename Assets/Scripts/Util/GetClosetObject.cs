using System.Collections.Generic;
using UnityEngine;

public static class GetClosetObject
{
    public static Notation
    GetClosetObj(List<Notation> objects, Transform fromThis)
    {
        Notation bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Notation potentialTarget in objects)
        {
            Vector3 directionToTarget =
                potentialTarget.notation.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
