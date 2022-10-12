using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingCircle : MonoBehaviour
{
    public float cameraZoomSize;

    public float cameraZoomSizeUpgrade1;

    public float cameraZoomSizeUpgrade2;

    public float cameraZoomSpeed;

    private SpriteRenderer spriteRenderer;

    private float cameraDefaultSize;

    private void Awake()
    {
        RigidPlayerController.notationCircleActivated += ZoomCameraOut;
        RigidPlayerController.notationCircleDeactivated += ZoomCameraIn;
        PlayerSkills.onSkillUnlocked += UpgradeReaderCircle;
    }

    private void UpgradeReaderCircle(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.readerRadius_1:
                CoreGameElements.i.gameSave.addedCamZoom =
                    cameraZoomSizeUpgrade1;
                break;
            case PlayerSkills.SkillType.readerRadius_2:
                CoreGameElements.i.gameSave.addedCamZoom =
                    cameraZoomSizeUpgrade2;
                break;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraDefaultSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (RigidPlayerController.readingMode)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void ZoomCameraOut()
    {
        float addedZoom = CoreGameElements.i.gameSave.addedCamZoom;
        FXController.ZoomCamera(cameraZoomSize + addedZoom, cameraZoomSpeed);
        SoundController.PlaySound(SoundController.Sound.CameraZoom);
    }

    private void ZoomCameraIn()
    {
        FXController.ZoomCamera (cameraDefaultSize, cameraZoomSpeed);
        SoundController.PlaySound(SoundController.Sound.CameraZoom);
    }
}
