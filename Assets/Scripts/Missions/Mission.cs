[System.Serializable]
public class Mission
{
    public enum Object
    {
        Coins,
        Statue,
        Rats,
        Pots,
        MusicalLaterns,
        Pearls,
        MusicalBoxes,
        MusicalChests,
        ShopPurchase,
        None,
        Flowers,
        Bushes
        //these should not be so specific but an enum of 'types' e.g. enemies, activations -- should be like multiple dropdown or based on the type of object?
        //what it would actually be is a list of 'items', 'smashables' etc. which are bases of mission objects?
        //they should also have names rather than using the enum names
    }

    public string missionType;

    public Object missionObject;

    public string missionObjectString;

    public int XPReward;

    public int coinReward;

    public int requiredAmount;

    public int currentAmount;

    public bool isActiveMission = true;

    public bool missionComplete() => currentAmount >= requiredAmount;
}
