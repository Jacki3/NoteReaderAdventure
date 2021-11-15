public interface IShopCustomer
{
    void BoughtItem(CoreItems.ItemType itemType);

    bool TrySpendCoinAmount(int coinAmount);
}
