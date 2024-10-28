using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public int Health = 3;
    public int MaxHealth = 3;


    protected float IFrames = 1.0f;
    protected float LastIFrames;

    public Vector3 KnockbackDirection;    
    public float KnockbackRecover = 1f;

    protected virtual void RecieveDamage(Damage dmg)
    {
        if (Time.time - LastIFrames > IFrames)
        {
            LastIFrames = Time.time;
            Health -= dmg.DamageAmt;
            KnockbackDirection = (transform.position - dmg.Origin).normalized * dmg.Knockback;

            if (Health <= 0)
            {
                Health = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}
