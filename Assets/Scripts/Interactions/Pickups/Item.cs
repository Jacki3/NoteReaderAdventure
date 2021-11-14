using UnityEngine;

//renamed to pickups
public class Item : MonoBehaviour
{
    public delegate void Increment();

    public static event Increment increment;

    public Mission.Object missionObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PickUp();
        }
    }

    protected virtual void PickUp()
    {
        // increment(); -- alternative solution could be events driven from parent classes?
        MissionHolder.i.CheckValidMission (missionObject); //everything which can be a mission just add this?
        //disable collision etc.
    }
}
