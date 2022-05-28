using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemShopController : MonoBehaviour
{
    public List<CoreItems> shopItems = new List<CoreItems>();

    public ShopButton itemTemplateButton;

    public int buttonDistanceY = 120;

    private IShopCustomer shopCustomer;

    private Transform container;

    private List<ShopButton> shopButtons = new List<ShopButton>();

    private void Awake()
    {
        // this is kinda yuck ?
        container = transform.Find("Container");
    }

    private void Start()
    {
        HideShop();

        int yDiff = 0;

        foreach (CoreItems item in shopItems)
        {
            CreateButtons(item.item,
            item.itemSprite,
            item.itemName,
            item.cost,
            yDiff);
            yDiff += buttonDistanceY;
        }
    }

    private void CreateButtons(
        CoreItems.ItemType itemType,
        Sprite itemSprite,
        string itemName,
        int itemCost,
        int yDiff
    )
    {
        ShopButton shopItemTransform =
            Instantiate(itemTemplateButton, container);

        shopItemTransform.transform.localPosition =
            new Vector2(itemTemplateButton.transform.localPosition.x,
                itemTemplateButton.transform.localPosition.y - yDiff);

        ShopButton newButton = shopItemTransform.GetComponent<ShopButton>();
        newButton.itemText.text.text = itemName;
        newButton.costText.text.text = itemCost.ToString();
        newButton.itemImage.image.sprite = itemSprite;

        //better way to do this?
        newButton
            .GetComponent<UnityEngine.UI.Button>()
            .onClick
            .AddListener(delegate ()
            {
                TryBuyItem (itemType);
            });

        shopButtons.Add (shopItemTransform);
    }

    //too many ifs and also add box shaker!
    public void TryBuyItem(CoreItems.ItemType itemType)
    {
        CoreItems item = GetItem(itemType);
        ShopButton currentShopButton = GetShopButton(item);

        int cost = GetItemCost(itemType).cost;

        //if itemType is already got (for cosmetics and sounds)
        if (shopCustomer.TrySpendCoinAmount(cost))
        {
            if (item.itemName.Contains("Health"))
            {
                if (HealthController.NotMaxHealth())
                {
                    shopCustomer.BoughtItem (itemType);
                    MissionHolder
                        .i
                        .CheckValidMission(Mission.Object.ShopPurchase);
                    CurrencyController.AddRemoveCoins(cost, false);
                }
                else
                {
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    Tooltip
                        .SetToolTip_Static("health already full!",
                        currentShopButton.transform.localPosition,
                        currentShopButton.transform.root);
                }
            }
            else if (itemType == CoreItems.ItemType.shield)
            {
                if (
                    HealthController.HasShield() ||
                    HealthController.HasProtectiveShield()
                )
                {
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    Tooltip
                        .SetToolTip_Static("Already got a shield!",
                        currentShopButton.transform.localPosition,
                        currentShopButton.transform.root);
                }
                else
                {
                    shopCustomer.BoughtItem (itemType);
                    MissionHolder
                        .i
                        .CheckValidMission(Mission.Object.ShopPurchase);
                    CurrencyController.AddRemoveCoins(cost, false);
                }
            }
            else if (itemType == CoreItems.ItemType.protectiveShield)
            {
                if (HealthController.HasProtectiveShield())
                {
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    Tooltip
                        .SetToolTip_Static("Already got a shield!",
                        currentShopButton.transform.localPosition,
                        currentShopButton.transform.root);
                }
                else
                {
                    shopCustomer.BoughtItem (itemType);
                    MissionHolder
                        .i
                        .CheckValidMission(Mission.Object.ShopPurchase);
                    CurrencyController.AddRemoveCoins(cost, false);
                }
            }
            else
            {
                shopCustomer.BoughtItem (itemType);
                MissionHolder.i.CheckValidMission(Mission.Object.ShopPurchase);
                CurrencyController.AddRemoveCoins(cost, false);
            }
        }
        else
        {
            SoundController.PlaySound(SoundController.Sound.IncorectNote);
            Tooltip
                .SetToolTip_Static("not enough coins!",
                currentShopButton.transform.localPosition,
                currentShopButton.transform.root);
        }
    }

    public void ShowShop(IShopCustomer customer)
    {
        shopCustomer = customer;
        gameObject.SetActive(true);
        GameObject firstButton = shopButtons[0].gameObject;
        EventSystem.current.SetSelectedGameObject (firstButton);
    }

    public void HideShop()
    {
        shopCustomer = null;
        gameObject.SetActive(false);
    }

    private CoreItems GetItemCost(CoreItems.ItemType itemType)
    {
        foreach (CoreItems item in shopItems)
        {
            if (item.item == itemType)
            {
                return item;
            }
        }
        Debug.LogError("NO ITEM");
        return null;
    }

    private ShopButton GetShopButton(CoreItems itemType)
    {
        foreach (ShopButton shopButton in shopButtons)
        {
            if (shopButton.itemText.text.text.Contains(itemType.itemName))
                return shopButton;
        }
        Debug.LogError("no shop button!");
        return null;
    }

    private CoreItems GetItem(CoreItems.ItemType itemType)
    {
        foreach (CoreItems item in shopItems)
        {
            if (item.item == itemType)
            {
                return item;
            }
        }
        Debug.LogError("no item!");
        return null;
    }
}
