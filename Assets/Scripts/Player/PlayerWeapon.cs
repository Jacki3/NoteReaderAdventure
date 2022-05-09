using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public NoteProjectile projectile;

    public Transform projectileSpawn;

    private static PlayerWeapon instance;

    private void Awake()
    {
        instance = this;
    }

    public static void ShootProjectileStatic(Vector3 target, Color32 noteColour)
    {
        instance.ShootProjectile (target, noteColour);
    }

    private void ShootProjectile(Vector3 target, Color32 noteColour)
    {
        var newProjectile = Instantiate(projectile);
        newProjectile.projectileColour = noteColour;
        projectile.transform.position = projectileSpawn.position;
        projectile.target = target;
    }
}
