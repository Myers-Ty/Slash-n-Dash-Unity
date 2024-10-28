using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity.Examples.Gungeon;

public class Win : Collidable
{
    private float WhenDropped;
    public GameObject WinScreen;
    protected void Awake()
    {
        WhenDropped = Time.time;
        WinScreen = GameObject.FindGameObjectWithTag("Finish");
    }
    protected override void OnCollide(Collider2D coll)
    {
        if ((Time.time - WhenDropped) >= 1f)
        {
            if (coll.name == "Player")
            {
                WinScreen.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }


    }
}
