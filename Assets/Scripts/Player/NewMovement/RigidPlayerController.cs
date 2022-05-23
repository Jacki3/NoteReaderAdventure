using System.Collections;
using UnityEngine;

public class RigidPlayerController : MovingObject
{
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public static NoteReadingRPGAdventure inputActions;

    public static bool readingMode;

    public delegate void NotationCircleSwitch();

    public static event NotationCircleSwitch notationCircleActivated;

    public static event NotationCircleSwitch notationCircleDeactivated;

    private void Awake()
    {
        inputActions = new NoteReadingRPGAdventure();

        // inputActions.Player.Smash.performed += ctx => SpawnCircle();
        // inputActions.Player.Escape.performed += ctx => ShowMenu();
        // inputActions.Player.ReadModeSwitch.performed += ctx => SetReadingMode();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // PlayerSkills.onSkillUnlocked += SkillUnlocked;
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

        if (!readingMode)
        {
            if (PlayerController.inputActions.Player.Up.WasPressedThisFrame())
                AttemptMove<RhythmEnemy>(0, 1);
            else if (
                PlayerController.inputActions.Player.Down.WasPressedThisFrame()
            )
                AttemptMove<RhythmEnemy>(0, -1);
            else if (
                PlayerController.inputActions.Player.Left.WasPressedThisFrame()
            )
                AttemptMove<RhythmEnemy>(-1, 0);
            else if (
                PlayerController.inputActions.Player.Right.WasPressedThisFrame()
            ) AttemptMove<RhythmEnemy>(1, 0);
        }

        //applying cosmetic items - can be a lot better!
        // if (dir.y == -1)
        //     glasses.sortingOrder = mSpriteRenderer.sortingOrder + 1;
        // if (dir.y == 1 || dir.x == 1 || dir.x == -1) glasses.sortingOrder = 0;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if (!GameStateController.gamePaused)
        {
            if (Move(xDir, yDir, out hit))
            {
                //animate movement here
                //check tutorial for movement?
                if (AudioController.canPlay)
                {
                    SoundController
                        .PlaySound(SoundController.Sound.ButtonClick);
                    ScoreController.AddStreak_Static();
                }
                else
                {
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    ScoreController.ResetStreak_Static(true);
                }
            }
        }
    }

    protected override void OnCantMove<T>(T Component)
    {
    }

    public void LoseHealth(int healthLost)
    {
        HealthController.RemoveHealth (healthLost);
    }
}
