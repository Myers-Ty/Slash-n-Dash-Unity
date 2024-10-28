using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : Entities
{
    protected Vector3 ChangeXY;
    protected BoxCollider2D Collision;
    protected RaycastHit2D Hit;
    protected float YSpeed = 5.0f;
    protected float XSpeed = 5.0f;
    protected Animator RunAnimation;

    protected virtual void Start()
    {
        Collision = GetComponent<BoxCollider2D>();
        RunAnimation = GetComponent<Animator>();
    }
    protected virtual void UpdateMotor(Vector3 input)
    {
        ChangeXY = new Vector3(input.x * XSpeed, input.y * YSpeed, 0);

        // Sprite Direction 
        if (ChangeXY.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            RunAnimation.SetBool("Running", true);
        }
        else if (ChangeXY.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            RunAnimation.SetBool("Running", true);
        }
        /*
         *if (ChangeXY.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            RunAnimation.SetBool("Running", true);
        }
        else if (ChangeXY.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            RunAnimation.SetBool("Running", true);
        }
         */
        else
        {
            RunAnimation.SetBool("Running", false);
        }

        // Prevents the entities from flyign away when being hit by a dash or spear. 
        // Resets the velocities to 0 before anything happens
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        // Knockback
        ChangeXY += KnockbackDirection;
        KnockbackDirection = Vector3.Lerp(KnockbackDirection, Vector3.zero, KnockbackRecover);

        // Change physics to check if it goes throught the player to not move
        // Allow Movement based on collision
        Hit = Physics2D.BoxCast(transform.position, Collision.size, 0, new Vector2(0, ChangeXY.y), Mathf.Abs(ChangeXY.y * Time.deltaTime), LayerMask.GetMask("Entity", "Wall"));
        if (Hit.collider == null)
        {
            transform.Translate(0, ChangeXY.y * Time.deltaTime, 0);
        }

        Hit = Physics2D.BoxCast(transform.position, Collision.size, 0, new Vector2(ChangeXY.x, 0), Mathf.Abs(ChangeXY.x * Time.deltaTime), LayerMask.GetMask("Entity", "Wall"));
        if (Hit.collider == null)
        {
            transform.Translate(ChangeXY.x * Time.deltaTime, 0, 0);
        }

    }
}
