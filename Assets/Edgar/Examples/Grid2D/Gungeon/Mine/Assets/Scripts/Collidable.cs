using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D Filter;
    private BoxCollider2D BoxCollider;
    private Collider2D[] Hits = new Collider2D[10];

    protected virtual void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();

    }

    protected virtual void Update()
    {
        BoxCollider.OverlapCollider(Filter, Hits);
        for (int i = 0; i < Hits.Length; i++)
        {
            if (Hits[i] == null)
            {
                continue;
            }
            OnCollide(Hits[i]);
            // reset the array index
            Hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
    }
}
