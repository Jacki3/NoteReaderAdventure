using System.Collections;
using EZCameraShake;
using UnityEngine;

public class RigidPlayerController : MovingObject, IShopCustomer
{
    public Sprite northSprite;

    public Sprite southSprite;

    public Sprite eastSprite;

    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    public static NoteReadingRPGAdventure inputActions;

    public static bool readingMode;

    public delegate void NotationCircleSwitch();

    public static event NotationCircleSwitch notationCircleActivated;

    public static event NotationCircleSwitch notationCircleDeactivated;

    private AudioSource audioSource;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

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
                spriteRenderer.sprite = southSprite;
                animator.SetInteger("Direction", 1);
            }
            else if (inputActions.Player.Down.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(0, -1);
                spriteRenderer.sprite = northSprite;
                animator.SetInteger("Direction", 0);
            }
            else if (inputActions.Player.Left.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(-1, 0);
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = eastSprite;
                animator.SetInteger("Direction", 3);
            }
            else if (inputActions.Player.Right.WasPressedThisFrame())
            {
                AttemptMove<MonoBehaviour>(1, 0);
                spriteRenderer.flipX = true;
                spriteRenderer.sprite = eastSprite;
                animator.SetInteger("Direction", 2);
            }
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (GameStateController.state == GameStateController.States.Play)
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
                    print("ADDING RHYTHM");
                    SoundController
                        .PlaySound(SoundController.Sound.ButtonClick);
                    CoreGameElements.i.currentRhythmStreak++;
                    UIController
                        .UpdateTextUI(UIController
                            .UITextComponents
                            .currentRhythmStreak,
                        CoreGameElements.i.currentRhythmStreak.ToString());

                    int savedRhythmStreak =
                        CoreGameElements.i.gameSave.rhythmStreak;
                    int currentRhythmStreak =
                        CoreGameElements.i.currentRhythmStreak;
                    if (currentRhythmStreak > savedRhythmStreak)
                    {
                        CoreGameElements.i.gameSave.rhythmStreak =
                            currentRhythmStreak;
                        UIController
                            .UpdateTextUI(UIController
                                .UITextComponents
                                .rhythmStreak,
                            currentRhythmStreak.ToString());
                    }
                }
                else
                {
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    CoreGameElements.i.currentRhythmStreak = 0;
                    UIController
                        .UpdateTextUI(UIController
                            .UITextComponents
                            .currentRhythmStreak,
                        CoreGameElements.i.currentRhythmStreak.ToString());
                }
            }
            base.AttemptMove<T>(xDir, yDir);
        }
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
        if (TextureController.CheckBoughtItemStatic(itemType) == false)
        {
            switch (itemType)
            {
                case CoreItems.ItemType.smallHealthRefill:
                    HealthController.AddHealth(1);
                    HealthController.SaveHealth();
                    break;
                case CoreItems.ItemType.healthRefill:
                    HealthController.AddHealth(6);
                    HealthController.SaveHealth();
                    break;
                case CoreItems.ItemType.largeHealthRefill:
                    HealthController.AddHealth(12);
                    HealthController.SaveHealth();
                    break;
                case CoreItems.ItemType.shield:
                    HealthController.AddShield(false);
                    SpriteController.SetSprite(SpriteController.Sprites.shield);
                    HealthController.SaveHealth();

                    break;
                case CoreItems.ItemType.protectiveShield:
                    HealthController.AddShield(true);
                    SpriteController
                        .SetSprite(SpriteController.Sprites.protectiveShield);
                    HealthController.SaveHealth();
                    break;
                case CoreItems.ItemType.life:
                    LivesController.AddLife();
                    break;
            }
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

    public void SetSprites(Sprite front, Sprite back, Sprite side)
    {
        northSprite = front;
        southSprite = back;
        eastSprite = side;
        spriteRenderer.sprite = northSprite;
    }
}
