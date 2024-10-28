using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity.Examples.Gungeon;

public class Boss1Sword : Collidable
{
    private float WhenDropped;
    Weapon SwordChange;
    protected void Awake()
    {
        WhenDropped = Time.time;
        SwordChange = GameManager.instance.player.GetComponentInChildren<Weapon>();
    }
    protected override void OnCollide(Collider2D coll)
    {
        if ((Time.time - WhenDropped) >= 1f)
        {
            if (coll.name == "Player")
            {
                SwordChange.CurrentWeapon = 1;
                Destroy(gameObject);
                if (!GungeonGameManager.Instance.isGenerating)
                {
                    GungeonGameManager.Instance.Stage = 2;
                    GungeonGameManager.Instance.LoadNextLevel();
                }
            }
        }


    }
}
