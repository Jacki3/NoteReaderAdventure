﻿using System;
using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IShopCustomer, IDamagable
{
    [Header("Player Settings")]
    public float walkSpeed = 3;

    public float

            sprintSpeed_1 = 4.5f,
            sprintSpeed_2 = 4.5f,
            sprintSpeed_3 = 4.5f;

    [Header("Player Components")]
    public SpriteRenderer glasses;

    public Transform shieldSpawn;

    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    public PauseMenu pauseMenu;

    public AudioClip moveSound;

    private float moveSpeed = 3;

    private float sprintSpeed;

    private Animator animator;

    private Vector2 dir;

    private SpriteRenderer mSpriteRenderer;

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
        inputActions.Player.Escape.performed += ctx => ShowMenu();
        inputActions.Player.ReadModeSwitch.performed += ctx => SetReadingMode();

        mSpriteRenderer = GetComponent<SpriteRenderer>();
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
        }
        else
        {
            dir = Vector2.zero;
        }

        //this can be done better also leads to stuck movement if cursor clicks off screen etc.
        switch (dir)
        {
            case Vector2 v:
                if (v == Vector2.left)
                {
                    animator.SetInteger("Direction", 3);
                    break;
                }
                else if (v == Vector2.right)
                {
                    animator.SetInteger("Direction", 2);
                    break;
                }
                if (v == Vector2.up)
                {
                    animator.SetInteger("Direction", 1);
                    break;
                }
                else if (v == Vector2.down) animator.SetInteger("Direction", 0);
                break;
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
        animator.SetBool("IsMoving", dir.magnitude > 0);

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

        // GetComponent<Rigidbody2D>().velocity = moveSpeed * dir; //using get component?
        //lulw -- set for other items in one method
        if (dir.y == -1)
            glasses.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        if (dir.y == 1 || dir.x == 1 || dir.x == -1) glasses.sortingOrder = 0;
    }

    //again bad practice? doing something it should not really do -- should be listening for events not calling methods
    private void ShowMenu()
    {
        if (GameStateController.state != GameStateController.States.Shopping)
        {
            // pauseMenu.ShowMenu();
        }
    }

    //this is bad practice right? -- try to copy from tooltip stuff
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
                mSpriteRenderer
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

    public void Damage(int damageToInflict)
    {
        HealthController.RemoveHealth(damageToInflict, true);
    }
}
