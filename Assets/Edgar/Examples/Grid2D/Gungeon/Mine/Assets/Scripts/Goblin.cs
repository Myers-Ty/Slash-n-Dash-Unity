using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Movement
{
    public float TriggerLength = 100f;
    public float ChaseLength = 100f;
    private bool Chasing;
    private bool CollisionWithPlayer;
    private Transform PlayerTransform;
    private Vector3 StartingPosition;

    private Collider2D[] Hits = new Collider2D[10];
    public ContactFilter2D Filter;

    // Spear Throwing Variables
    private float PreviousSpear;
    public float cooldown = 3f;
    public GameObject SpearGoblin;

    public GameObject Heart;
    protected override void Start()
    {
        base.Start();
        PlayerTransform = GameManager.instance.player.transform;
        StartingPosition = transform.position;
        YSpeed = 1.25f;
        XSpeed = 1.25f;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(PlayerTransform.position, StartingPosition) < ChaseLength)
        {
            if (Vector3.Distance(PlayerTransform.position, StartingPosition) < TriggerLength)
            {
                Chasing = true;
            }
            if (Vector3.Distance(PlayerTransform.position, StartingPosition) < ChaseLength)
            {
                if (Time.time + Random.Range(0.5f,1f) - PreviousSpear > cooldown)
                {
                    PreviousSpear = Time.time;
                    ThrowSpear();
                }
            }
            if (Chasing)
            {
                if (!CollisionWithPlayer)
                {
                    UpdateMotor((PlayerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(StartingPosition - transform.position); // return to original position
            }
        }
        else
        {
            UpdateMotor(StartingPosition - transform.position); // return to original position
            Chasing = false;
        }

        if ((int)(transform.position.x * 10) == (int)(StartingPosition.x * 10) && (int)(transform.position.y * 10) == (int)(StartingPosition.y * 10))
        {
            RunAnimation.SetBool("Running", false);
        }

        CollisionWithPlayer = false;
        Collision.OverlapCollider(Filter, Hits);
        for (int i = 0; i < Hits.Length; i++)
        {
            if (Hits[i] == null)
            {
                continue;
            }
            if(Hits[i].name == "Player")
            {
                CollisionWithPlayer = true;
            }
            // reset the array index
            Hits[i] = null;
        }
    }

    protected override void Death()
    {
        if (Random.Range(1, 4).Equals(2))
        {
            GameObject HeartDrop;
            HeartDrop = Instantiate(Heart, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    private void ThrowSpear()
    {
        GameObject Spear;
        Spear = Instantiate(SpearGoblin, transform.position, transform.rotation);

        Vector3 difference = PlayerTransform.position - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Spear.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 270);

        // different speeds based upon distance
        Spear.GetComponent<Rigidbody2D>().AddForce((PlayerTransform.position - transform.position).normalized * 640);
    }
}
