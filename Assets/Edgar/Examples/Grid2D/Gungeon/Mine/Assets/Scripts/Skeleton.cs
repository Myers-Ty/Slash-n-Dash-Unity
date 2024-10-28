using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Movement
{
    public float TriggerLength = 30f;
    public float ChaseLength = 40f;
    private bool Chasing;
    private bool CollisionWithPlayer;
    private Transform PlayerTransform;
    private Vector3 StartingPosition;

    private Collider2D[] Hits = new Collider2D[10];
    public ContactFilter2D Filter;

    public GameObject Heart;

    protected override void Start()
    {
        base.Start();
        PlayerTransform = GameManager.instance.player.transform;
        StartingPosition = transform.position;
        YSpeed = 4f;
        XSpeed = 4f;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(PlayerTransform.position, StartingPosition) < ChaseLength)
        {
            if (Vector3.Distance(PlayerTransform.position, StartingPosition) < TriggerLength)
            {
                Chasing = true;
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
                UpdateMotor((StartingPosition - transform.position) / 5); // return to original position
            }
        }
        else
        {
            UpdateMotor((StartingPosition - transform.position)/5); // return to original position
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
}
