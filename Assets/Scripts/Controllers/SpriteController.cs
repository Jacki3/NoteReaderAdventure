using UnityEngine;

public static class SpriteController
{
    public enum Sprites
    {
        coolSunGlasses,
        nerdGlasses,
        shield,
        protectiveShield,
        noShield,
        hair1,
        hair2
        //etc.
    }

    public static void SetSprite(Sprites sprite)
    {
        Sprite newSprite = GetSprite(sprite).sprite;
        SpriteRenderer newRenderer = GetSprite(sprite).renderer;

        newRenderer.sprite = newSprite;
    }

    private static CoreSpriteElements.SpriteElements GetSprite(Sprites sprites)
    {
        foreach (CoreSpriteElements.SpriteElements
            spriteElement
            in
            CoreSpriteElements.i.spriteElements
        )
        {
            if (spriteElement.spriteType == sprites) return spriteElement;
        }
        Debug.LogError("Sprite " + sprites + "missing!");
        return null;
    }
}
