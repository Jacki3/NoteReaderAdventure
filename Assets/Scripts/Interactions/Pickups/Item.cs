using UnityEngine;

//renamed to pickups
public class Item : MonoBehaviour
{
    public delegate void Increment();

    public static event Increment increment;

    public SoundController.Sound soundType;

    public Mission.Object missionObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            PickUp();
        }
    }

    protected virtual void PickUp()
    {
        // increment(); -- alternative solution could be events driven from parent classes?
        //disable collision etc.
        SoundController.PlaySound (soundType);
        MissionHolder.i.CheckValidMission (missionObject);
    }
}
