using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Movement
{
    private bool Chasing;
    private bool CollisionWithPlayer;
    private Transform PlayerTransform;
    private Vector3 StartingPosition;

    private Collider2D[] Hits = new Collider2D[10];
    public ContactFilter2D Filter;

    // Boss Attack Variables
    private float PreviousAttack;
    public float cooldown = 10f;
    //public GameObject SpearGoblin;

    Vector2 movement;
    [SerializeField] float startDashTime = 0.4f;
    [SerializeField] float dashSpeed = 15;
    float currentDashTime;
    bool canDash = true;
    bool canMove = true;


    public GameObject Sword2;
    protected override void Start()
    {
        base.Start();
        PlayerTransform = GameManager.instance.player.transform;
        StartingPosition = transform.position;
        YSpeed = 2f;
        XSpeed = 2f;
    }

    private void Update()
    {
        movement.x = PlayerTransform.position.x - transform.position.x;
        movement.y = PlayerTransform.position.y - transform.position.y;

        if (canDash && Time.time + Random.Range(0, (MaxHealth-Health)/2) - PreviousAttack > cooldown)
        {
            PreviousAttack = Time.time;
            StartCoroutine(Dash(new Vector2(movement.x, movement.y).normalized));
        }
    }
    private void FixedUpdate()
    {
        if (!CollisionWithPlayer && canMove)
        {
            UpdateMotor((PlayerTransform.position - transform.position).normalized);
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
    IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        canMove = false;
        this.GetComponentInChildren<EnemyHitbox>().Knockback = 50;
        RunAnimation.SetBool("Lurch", true);
        currentDashTime = startDashTime; // Reset the dash timer.

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            if (startDashTime - currentDashTime <= 0.7)
            {
                this.GetComponent<Rigidbody2D>().velocity = direction * 0;
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed; // Dash in the direction that was being moved in by the boss
            }

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f); // Stop dashing.

        canDash = true;
        canMove = true;
        this.GetComponentInChildren<EnemyHitbox>().Knockback = 20;
        RunAnimation.SetBool("Lurch", false);

    }
    protected override void Death()
    {
        GameObject SwordDrop;
        SwordDrop = Instantiate(Sword2, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
