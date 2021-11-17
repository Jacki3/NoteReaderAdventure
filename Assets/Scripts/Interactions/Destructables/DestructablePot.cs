public class DestructablePot : DestructableObject
{
    protected override void DestroyObject()
    {
        MissionHolder.i.CheckValidMission(Mission.Object.Pots);
        base.DestroyObject();
    }
}
