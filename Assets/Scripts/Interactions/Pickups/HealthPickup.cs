public class HealthPickup : Item
{
    public int healthToAdd = 1;

    protected override void PickUp()
    {
        base.PickUp();
        HealthController.AddHealth (healthToAdd);
        Destroy (gameObject);
    }
}
