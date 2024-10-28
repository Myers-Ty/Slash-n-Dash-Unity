using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Movement
{
    private bool Chasing;
    private bool CollisionWithPlayer;
    private Transform PlayerTransform;
    private Vector3 StartingPosition;

    private Collider2D[] Hits = new Collider2D[10];
    public ContactFilter2D Filter;

    // Boss Attack Variables
    private float PreviousAttack;
    public float cooldown = 5f;
    //public GameObject SpearGoblin;

    Vector2 movement;
    [SerializeField] float startDashTime = 2f;
    float currentDashTime;
    bool canAttack = true;
    bool canMove = true;

    public GameObject SpearGoblin;

    public GameObject Sword2;
    protected override void Start()
    {
        base.Start();
        PlayerTransform = GameManager.instance.player.transform;
        StartingPosition = transform.position;
        YSpeed = 1.75f;
        XSpeed = 1.75f;
    }

    private void Update()
    {
        movement.x = PlayerTransform.position.x - transform.position.x;
        movement.y = PlayerTransform.position.y - transform.position.y;

        if (canAttack && Time.time + Random.Range(0, (MaxHealth-Health)/8) - PreviousAttack > cooldown)
        {
            PreviousAttack = Time.time;
            StartCoroutine(Attack(new Vector2(movement.x, movement.y).normalized));
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
    IEnumerator Attack(Vector2 direction)
    {
        canAttack = false;
        canMove = false;
        RunAnimation.SetBool("Throw", true);
        currentDashTime = startDashTime; // Reset the dash timer.

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.
            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }
        Vector3 Direction;
        Direction.x = 0;
        Direction.y = 480;
        Direction.z = 0;
        ThrowSpear(Direction, 0);
        Direction.x = 0;
        Direction.y = -480;
        Direction.z = 0;
        ThrowSpear(Direction, 180);
        Direction.x = 480;
        Direction.y = 0;
        Direction.z = 0;
        ThrowSpear(Direction, 270);
        Direction.x = -480;
        Direction.y = 0;
        Direction.z = 0;
        ThrowSpear(Direction, 90);

        Direction.x = 480;
        Direction.y = 480;
        Direction.z = 0;
        ThrowSpear(Direction, 315);
        Direction.x = -480;
        Direction.y = 480;
        Direction.z = 0;
        ThrowSpear(Direction, 45);
        Direction.x = -480;
        Direction.y = -480;
        Direction.z = 0;
        ThrowSpear(Direction, 135);
        Direction.x = 480;
        Direction.y = -480;
        Direction.z = 0;
        ThrowSpear(Direction, 225);

        Direction.x = 240;
        Direction.y = 480;
        Direction.z = 0;
        ThrowSpear(Direction, 337.5f);
        Direction.x = 240;
        Direction.y = -480;
        Direction.z = 0;
        ThrowSpear(Direction, 202.5f);
        Direction.x = 480;
        Direction.y = 240;
        Direction.z = 0;
        ThrowSpear(Direction, 292.5f);
        Direction.x = -480;
        Direction.y = 240;
        Direction.z = 0;
        ThrowSpear(Direction, 67.5f);

        Direction.x = -240;
        Direction.y = -480;
        Direction.z = 0;
        ThrowSpear(Direction, 157.5f);
        Direction.x = -240;
        Direction.y = 480;
        Direction.z = 0;
        ThrowSpear(Direction, 22.5f);
        Direction.x = -480;
        Direction.y = -240;
        Direction.z = 0;
        ThrowSpear(Direction, 472.5f);
        Direction.x = 480;
        Direction.y = -240;
        Direction.z = 0;
        ThrowSpear(Direction, 247.5f);

        canAttack = true;
        canMove = true;
        RunAnimation.SetBool("Throw", false);

    }

    private void ThrowSpear(Vector3 Direction, float Rotation)
    {
        GameObject Spear;
        Spear = Instantiate(SpearGoblin, transform.position, transform.rotation);
        Spear.transform.rotation = Quaternion.Euler(0f, 0f, Rotation);

        // different speeds based upon distance
        Spear.GetComponent<Rigidbody2D>().AddForce(Direction);
    }
    protected override void Death()
    {
        GameObject SwordDrop;
        SwordDrop = Instantiate(Sword2, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
