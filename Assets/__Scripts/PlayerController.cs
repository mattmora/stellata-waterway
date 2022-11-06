using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tunnel tunnel;
    public Animator animator;
    public GameObject pivot;

    public float moveSpeed;
    public float xRange;
    public float resistance;
    public float jumpDuration;

    private Camera mainCamera;

    private float hInput, vInput;
    private bool jumping;
    private bool jumpReady;
    private bool grounded;

    private float initialY;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 1f);

        float effectiveMoveSpeed = moveSpeed * (((vInput + 1f) * 0.5f * 0.5f) + 0.75f);

        // X Movement
        Vector3 position = transform.position + (hInput * Vector3.right * effectiveMoveSpeed * Time.deltaTime);
        //position.x = Mathf.Clamp(position.x, -xRange, xRange);
        float xD = Mathf.Abs(position.x);
        if (xD > 0.01f)
        {
            float sign = Mathf.Sign(position.x);
            float a = xD / xRange;
            float rotationSpeed = (effectiveMoveSpeed * 360f) / (2f * Mathf.PI * 7.5f);
            tunnel.transform.Rotate(new Vector3(0f, 0f, -sign * a * rotationSpeed * Time.deltaTime));
            position.x *= Mathf.Pow(1f - resistance, Time.deltaTime * a);
        }
        transform.position = position;

        // Z Movement
        tunnel.scrollSpeed = effectiveMoveSpeed * 2.5f;
        animator.speed = (vInput * 0.2f) + 0.7f;
        mainCamera.fieldOfView = (vInput * 3f) + 60f;

        // Y Rotation
        transform.rotation = Quaternion.Euler(0f, hInput * (30f - (vInput * 10f)), 0f);

        grounded = false;
        if (Physics.Raycast(pivot.transform.position, Vector3.down, out RaycastHit hit))
        {
            grounded = hit.collider.CompareTag("Ground");
        }

        jumpReady = jumpReady || (grounded && !jumping);
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpReady)
        {
            pivot.transform.DOLocalJump(pivot.transform.localPosition, 3f, 1, jumpDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                jumping = false;
            });
            pivot.transform.DOPunchRotation(pivot.transform.right * -45f, jumpDuration, 0, 1f);
            jumping = true;
            jumpReady = false;
        }

        if (!grounded && !jumping)
        {
            transform.position -= Vector3.up * 10f * Time.deltaTime;
        }
        else if (transform.position.y < initialY)
        {
            transform.position += Vector3.up * 10f * Time.deltaTime;
        }
    }
}
