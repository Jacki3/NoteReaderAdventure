using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCube : MonoBehaviour
{
    public StoneCube cube;

    public bool playerInRange;

    private Vector3 cubeOrigin;

    void Start()
    {
        cubeOrigin = cube.transform.position;
        RigidPlayerController.inputActions.UI.Submit.performed += ctx =>
            ReturnCube();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") playerInRange = false;
    }

    private void ReturnCube()
    {
        if (playerInRange)
        {
            cube.GetComponent<BoxCollider2D>().enabled = false;
            cube.enabled = false;
            Invoke("WaitToReturn", .25f);
        }
    }

    private void WaitToReturn()
    {
        cube.transform.position = cubeOrigin;
        cube.GetComponent<BoxCollider2D>().enabled = true;
        cube.enabled = true;
    }
}
