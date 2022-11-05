using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject pointer;
    public float pointerRotationLimit = 90f;

    public float shootInterval = 0.25f;
    public float shootForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPosition = Services.Cursor.position;
        Vector3 toCursor = cursorPosition - pointer.transform.position;

        // AIM
        float angle = Vector3.SignedAngle(pointer.transform.up, toCursor, Vector3.forward);
        pointer.transform.Rotate(new Vector3(0f, 0f, angle));
        float z = pointer.transform.localEulerAngles.z;
        if (z > 180f) z -= 360f;
        z = Mathf.Clamp(z, -pointerRotationLimit, pointerRotationLimit);
        pointer.transform.localEulerAngles = new Vector3(0f, 0f, z);

        // SHOOT
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootRoutine());
        }

        // GRAPPLE
        if (Input.GetMouseButtonDown(1))
        {

        }
    }
    
    private IEnumerator ShootRoutine()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 position = pointer.transform.position;
            float z = pointer.transform.eulerAngles.z;
            Vector3 initialForce = shootForce * pointer.transform.up;
            Services.Projectiles.CreateProjectile(position, z, initialForce);
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
