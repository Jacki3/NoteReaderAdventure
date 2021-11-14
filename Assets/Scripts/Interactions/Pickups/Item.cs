using UnityEngine;

public class Item : MonoBehaviour, IPickup
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PickUp();
        }
    }

    protected virtual void PickUp()
    {
        //disable collision?
        print("picked up!");
        Increment();
    }

    public void Increment()
    {
    }
}
