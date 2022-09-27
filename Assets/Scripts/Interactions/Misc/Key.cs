using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    private KeyType keyType;

    public enum KeyType
    {
        Gold,
        Iron
    }

    public Sprite keySprite;

    protected override void PickUp()
    {
        base.PickUp();
        UIController
            .UpdateImageSprite(UIController.UIImageComponents.goldKey,
            keySprite,
            true);
        KeyHolder.AddKey (keyType);
        Destroy (gameObject);

        Transform parent = CoreGameElements.i.mainCanvas.transform;

        Tooltip
            .SetToolTip_Static("picked up " + keyType + " key!",
            Vector3.zero,
            parent);
    }
}
