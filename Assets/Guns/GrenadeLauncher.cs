using UnityEngine;

public class GrenadeLauncher : Gun
{
    [SerializeField] GameObject prefabGrenadeBlast;//for shotgun blast particles

     public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(100, 100, 0.5f, 50, null); // version without special effect

        //blast effect when shot
        Instantiate(prefabGrenadeBlast, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);


        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        return true;
    }
}
