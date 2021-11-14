using UnityEngine;

public class CoreItemElements : MonoBehaviour
{
    private static CoreItemElements _i;

    public static CoreItemElements i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;
    }

    public ItemType[] itemTypes;

    [System.Serializable]
    public class ItemType
    {
        public ItemSpawner.ItemType item;

        public Item itemType;
    }
}
