using UnityEngine;

//renamed to pickups
public class Item : MonoBehaviour
{
    public delegate void Increment();

    public static event Increment increment;

    public SoundController.Sound soundType;

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
        //disable collision etc.
        SoundController.PlaySound (soundType);
    }
}
