using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TextureController : MonoBehaviour
{
    public RigidPlayerController player;

    [SerializeField]
    private UnityEngine.UI.Image playerImage;

    public Sprite frontFacingSprite;

    public Sprite backFacingSprite;

    public Sprite sideFacingSprite;

    [SerializeField]
    private CustomisationItem[] customItems;

    [SerializeField]
    private ItemButtonEnum itemButton;

    [SerializeField]
    private Transform frontScrollRect;

    [SerializeField]
    private Transform backScrollRect;

    [SerializeField]
    private Transform sideScrollRect;

    [SerializeField]
    private Transform frontScrollRectContent;

    [SerializeField]
    private Transform backScrollRectContent;

    [SerializeField]
    private Transform sideScrollRectContent;

    private bool leftFacing;

    private bool rightFacing;

    private bool backFacing;

    private bool frontFacing = true;

    private List<ItemButtonEnum> itemButtons = new List<ItemButtonEnum>();

    private static TextureController i;

    public enum Orientation
    {
        front,
        back,
        side
    }

    void Awake()
    {
        i = this;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            CurrencyController.AddRemoveCoins(100, true);
    }

    public static void CreateItemButtonsFirst()
    {
        i.CreateItemButtons();
    }

    private void CreateItemButtons()
    {
        Transform parent = null;
        foreach (CustomisationItem item in customItems)
        {
            ItemButtonEnum newButton = Instantiate(itemButton);
            switch (item.orientation)
            {
                case Orientation.front:
                    parent = frontScrollRectContent;
                    break;
                case Orientation.back:
                    parent = backScrollRectContent;
                    break;
                case Orientation.side:
                    parent = sideScrollRectContent;
                    break;
            }
            newButton.transform.SetParent (parent);
            newButton.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            newButton.customItemType = item.itemType;
            newButton.SetButtonUI(item.itemName, item.UISprite);
            newButton.player = player;
            newButton.textureController = this;
            if (item.isUnlocked) newButton.UnlockSpirte();

            itemButtons.Add (newButton);
        }
    }

    public void SwitchMenu(bool right)
    {
        var rotation = playerImage.GetComponent<RectTransform>().localRotation;
        if (right)
        {
            if (!backFacing && !leftFacing && !rightFacing && frontFacing)
            {
                //Player is front facing and right arrow has been pressed and player is now showing right side
                rightFacing = true;
                frontFacing = false;
                playerImage.sprite = sideFacingSprite;
                rotation.y = 180;
                playerImage.GetComponent<RectTransform>().localRotation =
                    rotation;
                sideScrollRect.gameObject.SetActive(true);
                frontScrollRect.gameObject.SetActive(false);
            }
            else if (!backFacing && !leftFacing && !frontFacing && rightFacing)
            {
                //Player is facing right and right arrow is pressed now player is back facing
                backFacing = true;
                rightFacing = false;
                playerImage.sprite = backFacingSprite;
                sideScrollRect.gameObject.SetActive(false);
                backScrollRect.gameObject.SetActive(true);
            }
            else if (!leftFacing && !rightFacing && !frontFacing && backFacing)
            {
                //player is facing back and right is pressed now player is facing left
                leftFacing = true;
                backFacing = false;
                rotation.y = 0;
                playerImage.GetComponent<RectTransform>().localRotation =
                    rotation;
                playerImage.sprite = sideFacingSprite;
                sideScrollRect.gameObject.SetActive(true);
                backScrollRect.gameObject.SetActive(false);
            }
            else
            {
                //player is facing left and right is pressed now player faces front
                frontFacing = true;
                leftFacing = false;
                playerImage.sprite = frontFacingSprite;
                sideScrollRect.gameObject.SetActive(false);
                frontScrollRect.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!backFacing && !leftFacing && !rightFacing && frontFacing)
            {
                //Player is front facing and right arrow has been pressed and player is now showing right side
                leftFacing = true;
                frontFacing = false;
                playerImage.sprite = sideFacingSprite;
                rotation.y = 0;
                playerImage.GetComponent<RectTransform>().localRotation =
                    rotation;
                sideScrollRect.gameObject.SetActive(true);
                frontScrollRect.gameObject.SetActive(false);
            }
            else if (!backFacing && !rightFacing && !frontFacing && leftFacing)
            {
                //Player is facing right and right arrow is pressed now player is back facing
                backFacing = true;
                rightFacing = false;
                playerImage.sprite = backFacingSprite;
                sideScrollRect.gameObject.SetActive(false);
                backScrollRect.gameObject.SetActive(true);
            }
            else if (!leftFacing && !rightFacing && !frontFacing && backFacing)
            {
                //player is facing back and right is pressed now player is facing left
                rightFacing = true;
                backFacing = false;
                rotation.y = 180;
                playerImage.GetComponent<RectTransform>().localRotation =
                    rotation;
                playerImage.sprite = sideFacingSprite;
                sideScrollRect.gameObject.SetActive(true);
                backScrollRect.gameObject.SetActive(false);
            }
            else
            {
                //player is facing left and right is pressed now player faces front
                frontFacing = true;
                leftFacing = false;
                playerImage.sprite = frontFacingSprite;
                sideScrollRect.gameObject.SetActive(false);
                frontScrollRect.gameObject.SetActive(true);
            }
        }
        playerImage.type = UnityEngine.UI.Image.Type.Simple;
    }

    public static void SetSpriteStatic(
        Orientation orientation,
        Sprite front,
        Sprite back,
        Sprite side
    )
    {
        i.frontFacingSprite = front;
        i.sideFacingSprite = side;
        i.backFacingSprite = back;
        i.SetSprite(Orientation.front);
    }

    public void SetSprite(Orientation orientation)
    {
        switch (orientation)
        {
            case TextureController.Orientation.front:
                playerImage.sprite = frontFacingSprite;
                break;
            case TextureController.Orientation.back:
                playerImage.sprite = backFacingSprite;
                break;
            case TextureController.Orientation.side:
                playerImage.sprite = sideFacingSprite;
                break;
        }
        player.SetSprites (
            frontFacingSprite,
            backFacingSprite,
            sideFacingSprite
        );
    }

    public static bool CheckBoughtItemStatic(CoreItems.ItemType item)
    {
        if (i.CheckBoughtItem(item))
            return true;
        else
            return false;
    }

    private bool CheckBoughtItem(CoreItems.ItemType itemBought)
    {
        foreach (CustomisationItem item in customItems)
        {
            if (itemBought == item.itemType && !item.isUnlocked)
            {
                item.isUnlocked = true;
                ItemButtonEnum itemButton = GetItemButton(itemBought);
                itemButton.UnlockSpirte();
                SaveItems();
                return true;
            }
            else
            {
                print("item already purchased");
                return false;
            }
        }
        return false;
    }

    [Serializable]
    public class CustomisationItem
    {
        public string itemName;

        public Sprite UISprite;

        public CoreItems.ItemType itemType;

        public Orientation orientation;

        public Sprite customSprite;

        public bool isUnlocked;
    }

    public CustomisationItem GetCustomItem(CoreItems.ItemType type)
    {
        foreach (CustomisationItem item in customItems)
        {
            if (item.itemType == type) return item;
        }
        Debug.LogError("No item!");
        return null;
    }

    private ItemButtonEnum GetItemButton(CoreItems.ItemType type)
    {
        foreach (ItemButtonEnum itemButton in itemButtons)
        {
            if (itemButton.customItemType == type) return itemButton;
        }
        Debug.Log("No matching item button");
        return null;
    }

    public static void SaveItemsStatic()
    {
        i.SaveItems();
    }

    private void SaveItems()
    {
        var save = CoreGameElements.i.gameSave;
        save.frontSprite = frontFacingSprite;
        save.backSprite = backFacingSprite;
        save.sideSprite = sideFacingSprite;
        save.savedCustomItems = customItems.ToArray();
        for (int i = 0; i < customItems.Length; i++)
        {
            save.savedCustomItems[i].isUnlocked = customItems[i].isUnlocked;
        }
    }

    public static void LoadItemsStatic()
    {
        i.LoadItems();
    }

    private void LoadItems()
    {
        CustomisationItem[] savedItems =
            CoreGameElements.i.gameSave.savedCustomItems;
        for (int i = 0; i < customItems.Length; i++)
        {
            customItems[i].isUnlocked = savedItems[i].isUnlocked;
        }
    }
}
