using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int WeaponDamage = 1;
    public float Knockback = 25f;
    public int CurrentWeapon = 0;
    private SpriteRenderer SpriteRenderer;

    private Animator SwingAnimation;
    private float cooldown = 0.5f;
    private float PreviousSwing;

    public AudioClip SwordSwingC;
    AudioSource SwordSwingS;

    protected override void Start()
    {
        base.Start();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SwingAnimation = GetComponent<Animator>();
        SwordSwingS = AddAudio(false, false, 0.1f);
        SwordSwingS.clip = SwordSwingC;
    }

    protected override void Update()
    {
        base.Update();

        if (CurrentWeapon == 0)
        {
            WeaponDamage = 1;
            Knockback = 25f;
            SwingAnimation.SetTrigger("Weapon1");
        }
        else if (CurrentWeapon == 1)
        {
            WeaponDamage = 2;
            Knockback = 35f;
            SwingAnimation.SetTrigger("Weapon2");
        }

        //       if(Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Time.time - PreviousSwing > cooldown)
            {
                PreviousSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Entity")
        {
            if (coll.name != "Player")
            {
                Damage dmg = new Damage
                {
                    DamageAmt = WeaponDamage,
                    Origin = transform.position,
                    Knockback = Knockback
                };

                coll.SendMessage("RecieveDamage", dmg);
            }
        }
    }
    private void Swing()
    {
        SwordSwingS.Play();
        SwingAnimation.SetTrigger("Swing");
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
