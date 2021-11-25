using System;
using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IShopCustomer
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

    private float moveSpeed = 3;

    private float sprintSpeed;

    private Animator animator;

    private Vector2 dir;

    private SpriteRenderer mSpriteRenderer;

    private NoteReadingRPGAdventure inputActions;

    private void Awake()
    {
        inputActions = new NoteReadingRPGAdventure();

        inputActions.Player.Smash.performed += ctx => SpawnCircle();
        inputActions.Player.Escape.performed += ctx => ShowMenu();

        mSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        dir = move;

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
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = moveSpeed * dir;

        //lulw
        if (dir.y == -1)
            glasses.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        if (dir.y == 1 || dir.x == 1 || dir.x == -1) glasses.sortingOrder = 0;
    }

    //again bad practice? doing something it should not really do -- should be listening for events not calling methods
    private void ShowMenu()
    {
        pauseMenu.ShowMenu();
    }

    //this is bad practice right? -- try to copy from tooltip stuff
    private void SpawnCircle()
    {
        var newCircle = Instantiate(smashCircle);
        newCircle.SetLayerAndPosition (smashCircleSpawnPoint, mSpriteRenderer);
    }

    private void SkillUnlocked(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.sprint_1:
                sprintSpeed = sprintSpeed_1;
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
        print("Bought item " + itemType);
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
                HealthController.AddShield();
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
}
