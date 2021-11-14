using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int

            fowardNote,
            backwardNote,
            leftNote,
            rightNote,
            readModeNote;

    [Header("Player Settings")]
    public float walkSpeed = 3;

    public float sprintSpeed = 4.5f;

    [Header("Player Components")]
    public SmashCircle smashCircle;

    public Transform smashCircleSpawnPoint;

    private float moveSpeed = 3;

    private Animator animator;

    private Vector2 dir;

    private SpriteRenderer mSpriteRenderer;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

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
    }
}
