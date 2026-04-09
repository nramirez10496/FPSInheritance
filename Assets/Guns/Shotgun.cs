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

    // example function, make hit enemy fly upward
    void DoThing(HitData data)
    {
        Vector3 impactLocation = data.location;

        var colliders = Physics.OverlapSphere(impactLocation, 1);
        foreach (var c in colliders)
        {
            if (c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }
    }
}
