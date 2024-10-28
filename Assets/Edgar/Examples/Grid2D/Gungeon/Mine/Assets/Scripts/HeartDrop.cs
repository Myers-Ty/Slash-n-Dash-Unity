using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : Collidable
{
    public int Damage = -1;
    public float Knockback = 0;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Damage dmg = new Damage
            {
                DamageAmt = Damage,
                Origin = transform.position,
                Knockback = Knockback
            };

            coll.SendMessage("RecieveDamage", dmg);

            Destroy(gameObject);
        }
    }
}
