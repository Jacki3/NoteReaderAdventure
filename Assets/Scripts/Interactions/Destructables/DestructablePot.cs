public class DestructablePot : DestructableObject
{
    public override void DestroyObject()
    {
        MissionHolder.i.CheckValidMission(Mission.Object.Pots);
        base.DestroyObject();
    }
}
