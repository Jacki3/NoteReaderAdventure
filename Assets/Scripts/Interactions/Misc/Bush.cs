using UnityEngine;

[System.Serializable]
public class Bush : MonoBehaviour
{
    public GameObject flowers;

    public int xpToAdd;

    public bool isWatered;

    public void WaterBush(bool makeSound)
    {
        CoreItems.ItemType water = CoreItems.ItemType.water;
        if (InventoryController.CheckItem(water))
        {
            if (!isWatered)
            {
                isWatered = true;

                if (flowers != null) flowers.SetActive(true);

                if (makeSound)
                {
                    SoundController.PlaySound(SoundController.Sound.FlowerUp);
                    ExperienceController.AddXP (xpToAdd);
                    MissionHolder.i.CheckValidMission(Mission.Object.Flowers);
                }
            }
            else
                SoundController.PlaySound(SoundController.Sound.IncorectNote);
        }
    }
}
