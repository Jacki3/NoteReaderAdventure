using UnityEngine;

public class CoreSpriteElements : MonoBehaviour
{
    private static CoreSpriteElements _i;

    public static CoreSpriteElements i
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

    public SpriteElements[] spriteElements;

    [System.Serializable]
    public class SpriteElements
    {
        public SpriteController.Sprites spriteType;

        public SpriteRenderer renderer;

        public Sprite sprite;
    }
}
