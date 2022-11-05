using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 initialForce;
    public float mass = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Services.Projectiles.projectiles.Contains(this))
        {
            Services.Projectiles.projectiles.Add(this);
        }
        AddForce(initialForce, ForceMode2D.Impulse);
    }

    public void AddForce(Vector3 force, ForceMode2D mode)
    {
        rb.AddForce(force, mode);
    }
}
