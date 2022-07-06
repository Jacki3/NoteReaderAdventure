using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureController : MonoBehaviour
{
    public RigidPlayerController player;

    [SerializeField]
    private CustomisationItem[] customItems;

    public enum customItem
    {
        coolGlasses,
        nerdGlasses
    }

    public enum Orientation
    {
        front,
        back,
        side
    }

    [Serializable]
    public class CustomisationItem
    {
        public customItem itemType;

        public Orientation orientation;

        public Sprite customSprite;

        public bool isUnlocked;
    }

    public CustomisationItem GetCustomItem(customItem type)
    {
        foreach (CustomisationItem item in customItems)
        {
            if (item.itemType == type) return item;
        }
        Debug.LogError("No item!");
        return null;
    }
}
