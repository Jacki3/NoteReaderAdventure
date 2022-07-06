using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonEnum : MonoBehaviour
{
    public TextureController.customItem customItemType;

    public TextureController textureController;

    private TextureController.CustomisationItem customItem;

    private RigidPlayerController player;

    void Start()
    {
        customItem = textureController.GetCustomItem(customItemType);
        player = textureController.player;
    }

    public void SetSprite()
    {
        if (customItem.isUnlocked)
        {
            switch (customItem.orientation)
            {
                case TextureController.Orientation.front:
                    player.northSprite = customItem.customSprite;
                    break;
                case TextureController.Orientation.back:
                    player.southSprite = customItem.customSprite;
                    break;
                case TextureController.Orientation.side:
                    player.eastSprite = customItem.customSprite;
                    break;
            }
        }
    }
}
