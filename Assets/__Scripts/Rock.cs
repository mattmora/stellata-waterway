using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float mass = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
    }

    private void Start()
    {
        if (!Services.Rocks.rocks.Contains(this))
        {
            Services.Rocks.rocks.Add(this);
        }
    }
}
