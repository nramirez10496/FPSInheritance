using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] GameObject prefabShotgunBlast;//for shotgun blast particles
  
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(10, 100, 0.5f, 25, null); // version without special effect

        //blast effect when shot
        Instantiate(prefabShotgunBlast, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);


        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        return true;
    }

    
}
