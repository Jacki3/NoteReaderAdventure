using System;
using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;

public class PlayerController : MonoBehaviour, IShopCustomer
{
    public int

            fowardNote,
            backwardNote,
            leftNote,
            rightNote,
            readModeNote;

    [Header("Player Settings")]
    public float walkSpeed = 3;

    public float

            sprintSpeed_1 = 4.5f,
            sprintSpeed_2 = 4.5f,
            sprintSpeed_3 = 4.5f;

    [Header("Player Components")]
    public SpriteRenderer glasses;

    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    public PauseMenu pauseMenu;

    private float moveSpeed = 3;

    private float sprintSpeed;

    private Animator animator;

    private Vector2 dir;

    private SpriteRenderer mSpriteRenderer;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        PlayerSkills.onSkillUnlocked += SkillUnlocked;
    }

    private void Update()
    {
        dir = Vector2.zero;
        if (
            Input.GetKey(KeyCode.A) ||
            MidiMaster.GetKey(leftNote + MidiMaster.startMIDINumber) > 0
        )
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (
            Input.GetKey(KeyCode.D) ||
            MidiMaster.GetKey(rightNote + MidiMaster.startMIDINumber) > 0
        )
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (
            Input.GetKey(KeyCode.W) ||
            MidiMaster.GetKey(fowardNote + MidiMaster.startMIDINumber) > 0
        )
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (
            Input.GetKey(KeyCode.S) ||
            MidiMaster.GetKey(backwardNote + MidiMaster.startMIDINumber) > 0
        )
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        if (Input.GetKey(KeyCode.LeftShift) && CanSprint())
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

        //is this bad practice?
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var newCircle = Instantiate(smashCircle);
            newCircle.SetLayerAndPosition (
                smashCircleSpawnPoint,
                mSpriteRenderer
            );
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = moveSpeed * dir;

        //lulw
        if (dir.y == -1)
            glasses.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        if (dir.y == 1 || dir.x == 1 || dir.x == -1) glasses.sortingOrder = 0;

        if (Input.GetKeyDown(KeyCode.Escape)) pauseMenu.ShowMenu();
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
            case CoreItems.ItemType.healthRefill:
                HealthController.AddHealth(5);
                break;
            case CoreItems.ItemType.shield:
                HealthController.AddShield();
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
