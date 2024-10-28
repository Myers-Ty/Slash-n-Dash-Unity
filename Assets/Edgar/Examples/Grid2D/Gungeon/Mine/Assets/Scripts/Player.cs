using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Edgar.Unity.Examples.Gungeon;

public class Player : Movement
{
    // Hearts Code
    // public int Health;
    public int HeartCount;

    public Image[] HeartList;
    public Sprite Hearts_Full;
    public Sprite Hearts_Half;
    public Sprite Hearts_Empty;

    Vector2 movement;

    [SerializeField] float startDashTime = 0.2f;
    [SerializeField] float dashSpeed = 11;
    float currentDashTime;
    bool canDash = true;
    bool canMove = true;

    Weapon SwordChange;

    public AudioClip MusicC;
    public AudioClip DashC;
    AudioSource DashS;
    AudioSource MusicS;
    protected void Awake()
    {
        SwordChange = GameManager.instance.player.GetComponentInChildren<Weapon>();
        MusicS = AddAudio(true, true, 0.05f);
        MusicS.clip = MusicC;
        MusicS.Play();
        DashS = AddAudio(false, false, 0.075f);
        DashS.pitch = 0.7f;
        DashS.clip = DashC;
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Dash(new Vector2(1f, 1f)));
            }

            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Dash(new Vector2(1f, -1f)));
            }

            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
            {
                StartCoroutine(Dash(new Vector2(-1f, 1f)));
            }

            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                StartCoroutine(Dash(new Vector2(-1f, -1f)));
            }

            else if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(Dash(Vector2.up));
            }

            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(Dash(Vector2.left));
            }

            else if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(Dash(Vector2.down));
            }

            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Dash(Vector2.right));
            }
        }
    }
    private void FixedUpdate()
    {        
        // Hearts update code

        // Keeps health within dispalyable value
        if (Health > HeartCount*2)
        {
            Health = HeartCount*2;
        }
        for (int i = 0; i < HeartList.Length; i++) // TODO HEARTS 4 & 5 DO NOT WORK, loops only 5 times as the issues
        {
            // Updates sprites of health
            if (i*2 < Health)
            {
                HeartList[i].sprite = Hearts_Full;
            }
            else
            {
                HeartList[i].sprite = Hearts_Empty;
            }

            if (Health == i + 1 && Health % 2 == 1)
            {
                HeartList[i/2].sprite = Hearts_Half;
            }


            // Disables and Enables bonus health if applicable
            if (i < HeartCount)
            {
                HeartList[i].enabled = true;
            }
            else
            {
                HeartList[i].enabled = false;
            }
        }

        if (canMove == true)
        {
            movement.Normalize();
            UpdateMotor(new Vector3(movement.x, movement.y, 0));
        }
    }

    IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        canMove = false;

        DashS.Play();

        currentDashTime = startDashTime; // Reset the dash timer.

        while (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            this.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed; // Dash in the direction that was held down.

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f); // Stop dashing.

        canDash = true;
        canMove = true;

    }

    protected override void Death()
    {
        if (!GungeonGameManager.Instance.isGenerating)
        {
            GungeonGameManager.Instance.Stage = 1;
            SwordChange.CurrentWeapon = 0;
            GungeonGameManager.Instance.LoadNextLevel();
        }
        Health = MaxHealth;

    }

    public AudioSource AddAudio(bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        //newAudio.clip = clip; 
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }
}
