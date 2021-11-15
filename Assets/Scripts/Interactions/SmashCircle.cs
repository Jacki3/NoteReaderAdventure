using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashCircle : MonoBehaviour
{
    public static float sizeBonus; //if the player upgradges the smash ability then the cirlce size increases?

    private SpriteRenderer mSpriteRenderer;

    private Transform playerPos;

    private string sortingLayer;

    private int layerOrder;

    private CircleCollider2D mCircleCollider;

    private Animator mAnimator;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mAnimator = GetComponent<Animator>();
        mCircleCollider = gameObject.AddComponent<CircleCollider2D>();
        mCircleCollider.isTrigger = true;
        mCircleCollider.radius = 0;
    }

    private void Start()
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(2f, 1f, .5f, 1f);
    }

    private void Update()
    {
        transform.position = playerPos.position;
        mSpriteRenderer.sortingLayerName = sortingLayer;
        mSpriteRenderer.sortingOrder = layerOrder;
        mCircleCollider.radius += mAnimator.speed * 5 * Time.deltaTime; //hard coded number - how can we multiply this properly?
    }

    public void SetLayerAndPosition(
        Transform spawnPos,
        SpriteRenderer playerRenderer
    )
    {
        playerPos = spawnPos;
        sortingLayer = playerRenderer.sortingLayerName;
        layerOrder = playerRenderer.sortingOrder - 1;
    }
}
