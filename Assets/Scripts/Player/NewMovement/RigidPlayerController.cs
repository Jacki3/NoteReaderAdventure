using System.Collections;
using EZCameraShake;
using UnityEngine;

public class RigidPlayerController : MovingObject, IShopCustomer
{
    private Animator animator;

    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    private SpriteRenderer spriteRenderer;

    public static NoteReadingRPGAdventure inputActions;

    public static bool readingMode;

    public delegate void NotationCircleSwitch();

    public static event NotationCircleSwitch notationCircleActivated;

    public static event NotationCircleSwitch notationCircleDeactivated;

    private AudioSource audioSource;

    private void Awake()
    {
        inputActions = new NoteReadingRPGAdventure();

        inputActions.Player.Smash.performed += ctx => SpawnCircle();
        inputActions.Player.ReadModeSwitch.performed += ctx => SetReadingMode();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        inverseMoveTime = 1f / moveTime;

        if (
            !readingMode &&
            GameStateController.state == GameStateController.States.Play
        )
        {
            if (inputActions.Player.Up.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(0, 1);
                animator.SetInteger("Direction", 1);
            }
            else if (inputActions.Player.Down.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(0, -1);
                animator.SetInteger("Direction", 0);
            }
            else if (inputActions.Player.Left.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(-1, 0);
                animator.SetInteger("Direction", 3);
            }
            else if (inputActions.Player.Right.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(1, 0);
                animator.SetInteger("Direction", 2);
            }
        }

        //applying cosmetic items - can be a lot better!
        // if (dir.y == -1)
        //     glasses.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        // if (dir.y == 1 || dir.x == 1 || dir.x == -1) glasses.sortingOrder = 0;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            var camShake = CoreGameElements.i.cameraShakes.movementShake;
            CameraShaker
                .Instance
                .ShakeOnce(camShake.magnitude,
                camShake.roughness,
                camShake.fadeInTime,
                camShake.fadeOutTime);

            //animate movement here
            if (AudioController.canPlay)
            {
                SoundController.PlaySound(SoundController.Sound.ButtonClick);
            }
            else
            {
                SoundController.PlaySound(SoundController.Sound.IncorectNote);
            }
        }
        base.AttemptMove<T>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T Component)
    {
        DestructableObject destructableObject = Component as DestructableObject;
        RhythmEnemy rhythmEnemy = Component as RhythmEnemy;

        if (Component == rhythmEnemy)
        {
            LoseHealth(rhythmEnemy.playerDmg);
        }
        else if (Component == destructableObject)
        {
            destructableObject.DestroyObject();
        }
    }

    private void SpawnCircle()
    {
        if (
            !readingMode &&
            GameStateController.state == GameStateController.States.Play ||
            GameStateController.state == GameStateController.States.Tutorial &&
            !readingMode
        )
        {
            SoundController.PlaySound(SoundController.Sound.SmashCircle);
            var newCircle = Instantiate(smashCircle);
            newCircle.SetLayerAndPosition (
                smashCircleSpawnPoint,
                spriteRenderer
            );
        }
    }

    public void SetReadingMode()
    {
        if (
            GameStateController.state == GameStateController.States.Play ||
            GameStateController.state == GameStateController.States.Tutorial
        )
        {
            if (!GameStateController.gamePaused)
            {
                if (readingMode)
                {
                    readingMode = false;
                    notationCircleDeactivated();
                }
                else
                {
                    readingMode = true;
                    notationCircleActivated();
                }
            }
        }
    }

    public void BoughtItem(CoreItems.ItemType itemType)
    {
        SoundController.PlaySound(SoundController.Sound.Purchase);
        switch (itemType)
        {
            case CoreItems.ItemType.smallHealthRefill:
                HealthController.AddHealth(1);
                break;
            case CoreItems.ItemType.healthRefill:
                HealthController.AddHealth(6);
                break;
            case CoreItems.ItemType.largeHealthRefill:
                HealthController.AddHealth(12);
                break;
            case CoreItems.ItemType.shield:
                HealthController.AddShield(false);
                SpriteController.SetSprite(SpriteController.Sprites.shield);
                break;
            case CoreItems.ItemType.protectiveShield:
                HealthController.AddShield(true);
                SpriteController
                    .SetSprite(SpriteController.Sprites.protectiveShield);
                break;
            case CoreItems.ItemType.coolSunGlasses:
                SpriteController
                    .SetSprite(SpriteController.Sprites.coolSunGlasses);
                break;
            case CoreItems.ItemType.nerdGlasses:
                SpriteController
                    .SetSprite(SpriteController.Sprites.nerdGlasses);
                break;
            case CoreItems.ItemType.life:
                LivesController.AddLife();
                break;
        }
    }

    public bool TrySpendCoinAmount(int coinAmount)
    {
        if (CurrencyController.GetTotalCoins() >= coinAmount)
        {
            return true;
        }
        else
            return false;
    }

    public void LoseHealth(int healthLost)
    {
        HealthController.RemoveHealth(healthLost, true);
    }
}
