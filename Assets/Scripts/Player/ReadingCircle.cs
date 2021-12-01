using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingCircle : MonoBehaviour
{
    public float cameraZoomSize;

    public float cameraZoomSpeed;

    private SpriteRenderer spriteRenderer;

    private float cameraDefaultSize;

    private void Awake()
    {
        PlayerController.notationCircleActivated += ZoomCameraOut;
        PlayerController.notationCircleDeactivated += ZoomCameraIn;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraDefaultSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (PlayerController.readingMode)
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
        FXController.ZoomCamera (cameraZoomSize, cameraZoomSpeed);
        SoundController.PlaySound(SoundController.Sound.CameraZoom);
    }

    private void ZoomCameraIn()
    {
        FXController.ZoomCamera (cameraDefaultSize, cameraZoomSpeed);
        SoundController.PlaySound(SoundController.Sound.CameraZoom);
    }
}
