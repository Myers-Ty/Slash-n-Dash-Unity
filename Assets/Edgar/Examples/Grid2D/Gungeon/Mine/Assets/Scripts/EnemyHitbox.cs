using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int Damage;
    public float Knockback = 2;

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
        }
    }
}
