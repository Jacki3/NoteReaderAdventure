using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemButtonEnum : MonoBehaviour
{
    public CoreItems.ItemType customItemType;

    public TextureController textureController;

    public RigidPlayerController player;

    public TMPro.TextMeshProUGUI itemText;

    public UnityEngine.UI.Image image;

    public UnityEngine.UI.Button itemButton;

    public TextureController.CustomisationItem customItem;

    public void SetButtonUI(string text, Sprite itemSprite)
    {
        itemText.text = text;
        image.sprite = itemSprite;
    }

    public void SetSprite()
    {
        customItem = textureController.GetCustomItem(customItemType);
        TextureController.Orientation newOrient =
            new TextureController.Orientation();

        if (customItem.isUnlocked)
        {
            switch (customItem.orientation)
            {
                case TextureController.Orientation.front:
                    newOrient = TextureController.Orientation.front;
                    player.northSprite = customItem.customSprite;
                    textureController.frontFacingSprite =
                        customItem.customSprite;
                    CoreGameElements.i.gameSave.frontSprite =
                        customItem.customSprite;
                    break;
                case TextureController.Orientation.back:
                    newOrient = TextureController.Orientation.back;
                    player.southSprite = customItem.customSprite;
                    textureController.backFacingSprite =
                        customItem.customSprite;
                    CoreGameElements.i.gameSave.backSprite =
                        customItem.customSprite;
                    break;
                case TextureController.Orientation.side:
                    newOrient = TextureController.Orientation.side;
                    player.eastSprite = customItem.customSprite;
                    textureController.sideFacingSprite =
                        customItem.customSprite;
                    CoreGameElements.i.gameSave.sideSprite =
                        customItem.customSprite;
                    break;
            }
            textureController.SetSprite (newOrient);
        }
        else
        {
            //create a tooltip that says you have not unlocked the item
        }
    }

    public void UnlockSpirte()
    {
        image.color = Color.white;
        itemButton.interactable = true;
    }
}
