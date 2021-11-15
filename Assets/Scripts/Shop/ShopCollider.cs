using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    [SerializeField]
    private ItemShopController itemShop;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IShopCustomer customer = other.GetComponent<IShopCustomer>();
        if (customer != null)
        {
            itemShop.ShowShop (customer);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        itemShop.HideShop();
    }
}
