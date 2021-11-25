public class LifePickup : Item
{
    protected override void PickUp()
    {
        base.PickUp();
        LivesController.AddLife();
        Destroy (gameObject);
    }
}
