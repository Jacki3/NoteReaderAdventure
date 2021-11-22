using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TryBuyItem(CoreItems.ItemType itemType)
    {
        CoreItems item = GetItem(itemType);
        ShopButton currentShopButton = GetShopButton(item);

        int cost = GetItemCost(itemType).cost;
        if (shopCustomer.TrySpendCoinAmount(cost))
        {
            //add one for shield check -- too many if though maybe use case?
            if (itemType != CoreItems.ItemType.healthRefill)
            {
                shopCustomer.BoughtItem (itemType);
                CurrencyController.AddRemoveCoins(cost, false);
            }
            else
            {
                if (HealthController.NotMaxHealth())
                {
                    shopCustomer.BoughtItem (itemType);
                    CurrencyController.AddRemoveCoins(cost, false);
                }
                else
                {
                    //shake screen or box/sound
                    Tooltip
                        .SetToolTip_Static("health already full!",
                        currentShopButton.transform.localPosition,
                        currentShopButton.transform.root);
                }
            }
        }
        else
        {
            //shake screen or box/sound
            if (currentShopButton != null)
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
        print("NO ITEM");
        return null;
    }

    private ShopButton GetShopButton(CoreItems itemType)
    {
        foreach (ShopButton shopButton in shopButtons)
        {
            if (shopButton.itemText.text.text.Contains(itemType.itemName))
                return shopButton;
        }
        print("no shop button!");
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
        print("no item!");
        return null;
    }
}
