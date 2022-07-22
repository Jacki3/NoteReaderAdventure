using System;
using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IShopCustomer
{
    [Header("Player Settings")]
    public Sprite northSprite;

    public Sprite southSprite;

    public Sprite eastSprite;

    public float walkSpeed = 3;

    public float

            sprintSpeed_1 = 4.5f,
            sprintSpeed_2 = 4.5f,
            sprintSpeed_3 = 4.5f;

    [Header("Player Components")]
    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    public AudioClip moveSound;

    private float moveSpeed = 3;

    private float sprintSpeed;

    private Animator animator;

    private Vector2 dir;

    public static NoteReadingRPGAdventure inputActions;

    public static bool readingMode;

    public delegate void NotationCircleSwitch();

    public static event NotationCircleSwitch notationCircleActivated;

    public static event NotationCircleSwitch notationCircleDeactivated;

    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        inputActions = new NoteReadingRPGAdventure();

        inputActions.Player.Smash.performed += ctx => SpawnCircle();
        inputActions.Player.ReadModeSwitch.performed += ctx => SetReadingMode();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        PlayerSkills.onSkillUnlocked += SkillUnlocked;
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        var move = inputActions.Player.Move.ReadValue<Vector2>();
        var sprint = inputActions.Player.Sprint.ReadValue<float>();

        if (
            !readingMode &&
            GameStateController.state == GameStateController.States.Play ||
            GameStateController.state == GameStateController.States.Tutorial &&
            !readingMode
        )
        {
            dir = move;
            if (inputActions.Player.Up.WasPressedThisFrame())
            {
                spriteRenderer.sprite = southSprite;
                animator.SetInteger("Direction", 1);
            }
            else if (inputActions.Player.Down.WasPressedThisFrame())
            {
                spriteRenderer.sprite = northSprite;
                animator.SetInteger("Direction", 0);
            }
            else if (inputActions.Player.Left.WasPressedThisFrame())
            {
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = eastSprite;
                animator.SetInteger("Direction", 3);
            }
            else if (inputActions.Player.Right.WasPressedThisFrame())
            {
                spriteRenderer.flipX = true;
                spriteRenderer.sprite = eastSprite;
                animator.SetInteger("Direction", 2);
            }
        }
        else
        {
            dir = Vector2.zero;
        }

        if (sprint >= .1f && CanSprint())
        {
            if (dir.magnitude > 0) SprintBar.StartSprinting();
            if (SprintBar.canSprint)
                moveSpeed = sprintSpeed;
            else
                moveSpeed = walkSpeed;
        }
        else
        {
            SprintBar.StopSprinting();
            moveSpeed = walkSpeed;
        }
        if (dir.magnitude < 0) SprintBar.StopSprinting();
        dir.Normalize();
        if (dir.magnitude > 0)
        {
            if (GameStateController.state == GameStateController.States.Tutorial
            )
            {
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Move);
            }
            audioSource.clip = moveSound;
        }
        else
        {
            audioSource.clip = null;
            SprintBar.StopSprinting();
        }
        if (!audioSource.isPlaying) audioSource.Play();
        GetComponent<Rigidbody2D>().velocity = moveSpeed * dir; //using get component?
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

    private void SkillUnlocked(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.sprint_1:
                sprintSpeed = sprintSpeed_1;
                SprintBar.ShowStaminaBar();
                break;
            case PlayerSkills.SkillType.sprint_2:
                sprintSpeed = sprintSpeed_2;
                break;
            case PlayerSkills.SkillType.sprint_3:
                sprintSpeed = sprintSpeed_3;
                break;
        }
    }

    private bool CanSprint() =>
        PlayerSkills.IsSkillUnlocked(PlayerSkills.SkillType.sprint_1);

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
