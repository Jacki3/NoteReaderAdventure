using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    [SerializeField]
    private ItemShopController itemShop;

    private bool hasCustomer;

    public static bool isShopping;

    private IShopCustomer customer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        customer = other.GetComponent<IShopCustomer>();
        if (customer != null)
        {
            hasCustomer = true;
        }
    }

    void Update()
    {
        if (
            RigidPlayerController
                .inputActions
                .UI
                .Submit
                .WasPressedThisFrame() &&
            hasCustomer
        )
        {
            itemShop.ShowShop(customer, true);
            GameStateController.state = GameStateController.States.Shopping;
            isShopping = true;
        }

        if (
            isShopping &&
            RigidPlayerController
                .inputActions
                .Player
                .Escape
                .WasPressedThisFrame()
        )
        {
            itemShop.HideShop();
            GameStateController.state = GameStateController.States.Play;
            isShopping = false;
        }

        //if you are shopping then player cannot move until they press the same button again
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        itemShop.HideShop();
        hasCustomer = false;
    }
}
