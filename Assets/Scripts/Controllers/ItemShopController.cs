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
    }

    public void TryBuyItem(CoreItems.ItemType itemType)
    {
        int cost = GetItemCost(itemType).cost;
        if (shopCustomer.TrySpendCoinAmount(cost))
        {
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
                    //tooltip warning
                    //shake screen or box/sound
                    print("health already full!");
                }
            }
        }
        else
        {
            //tooltip warning
            //shake screen or box/sound
            print("not enough coins");
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
}
