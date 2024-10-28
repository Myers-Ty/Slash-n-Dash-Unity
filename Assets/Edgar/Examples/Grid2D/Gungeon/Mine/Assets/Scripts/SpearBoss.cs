using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBoss : Collidable
{
    public int Damage;
    public float Knockback = 30;

    private float WhenThrown;

    private Collider2D IgnoreColl;

    protected void Awake()
    {
        WhenThrown = Time.time;
        IgnoreColl = this.GetComponent<Collider2D>();
    }
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
        if ((Time.time - WhenThrown) >= 0.25f)
        {
            Destroy(gameObject);
        }

    }
}
