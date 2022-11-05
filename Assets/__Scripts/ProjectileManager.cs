using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float gravitationalCoefficient = 0.05f;

    public GameObject projectilePrefab;
    public List<Projectile> projectiles;

    private void Awake()
    {
        Services.Projectiles = this;

        projectiles = new List<Projectile>();
    }

    private void FixedUpdate()
    {
        List<Rock> rocks = Services.Rocks.rocks;
        List<Projectile> remove = new List<Projectile>();
        foreach (var projectile in projectiles)
        {
            if (projectile != null)
            {
                Vector3 gravity = Vector3.zero;
                foreach (Rock rock in Services.Rocks.rocks)
                {
                    if (rock != null)
                    {
                        Vector3 toRock = rock.transform.position - projectile.transform.position;
                        Vector3 direction = toRock.normalized;
                        float rSquared = toRock.sqrMagnitude;
                        float magnitude = (gravitationalCoefficient * projectile.mass * rock.mass) / rSquared;
                        gravity += direction * magnitude;
                    }
                }
                Debug.Log(gravity);
                projectile.AddForce(gravity * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else
            {
                remove.Add(projectile);
            }
        }
        projectiles.RemoveAll(projectile => remove.Contains(projectile));
    }

    public Projectile CreateProjectile(Vector3 position, float zRotation, Vector3 initialForce)
    {
        Projectile projectile = Instantiate(projectilePrefab, position, Quaternion.Euler(0f, 0f, zRotation)).GetComponent<Projectile>();
        projectile.initialForce = initialForce;
        projectiles.Add(projectile);
        return projectile;
    }

    public void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile);
    }
}
