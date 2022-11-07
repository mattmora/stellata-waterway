using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tunnel tunnel;
    public Animator animator;
    public GameObject swingPivot;
    public GameObject jumpPivot;

    public int health;

    public int score;

    public float moveSpeed;
    public float scrollSpeedRatio;
    public float xRange;
    public float resistance;
    public float jumpDuration;
    public float swingDuration;
    public float hitDelay;
    public float hitDuration;

    private Camera mainCamera;

    private float hInput, vInput;
    private bool jumping;
    private bool jumpReady;
    private bool grounded;
    private float initialY;
    private bool swinging;
    private bool invincible;

    public GameObject swingHitbox;

    public Renderer model;
    public Color damageColor;

    private void Awake()
    {
        Services.Player = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < initialY - 10f)
        {
            health = 0;
        }

        if (health <= 0)
        {
            transform.position -= Vector3.forward * 3f * Time.deltaTime;
        }
        else
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 1f);
        }

        float effectiveMoveSpeed = moveSpeed * (((vInput + 1f) * 0.5f * 0.5f) + 0.75f);

        // X Movement
        Vector3 position = transform.position + (hInput * Vector3.right * effectiveMoveSpeed * Time.deltaTime);
        //position.x = Mathf.Clamp(position.x, -xRange, xRange);
        float xD = Mathf.Abs(position.x);
        if (xD > 0.01f)
        {
            Services.Game.moved = true;
            float sign = Mathf.Sign(position.x);
            float a = xD / xRange;
            float rotationSpeed = (effectiveMoveSpeed * 360f) / (2f * Mathf.PI * 7.5f);
            tunnel.transform.Rotate(new Vector3(0f, 0f, -sign * a * rotationSpeed * Time.deltaTime));
            position.x *= Mathf.Pow(1f - resistance, Time.deltaTime * a);
        }
        transform.position = position;

        // Z Movement
        tunnel.scrollSpeed = effectiveMoveSpeed * scrollSpeedRatio * (1f + (score / 200000f));
        animator.speed = (vInput * 0.2f) + 0.7f;
        mainCamera.fieldOfView = (vInput * 3f) + 60f;

        // Y Rotation
        transform.rotation = Quaternion.Euler(0f, hInput * (30f - (vInput * 10f)), 0f);

        grounded = false;
        if (Physics.Raycast(jumpPivot.transform.position, Vector3.down, out RaycastHit hit))
        {
            grounded = hit.collider.CompareTag("Ground");
        }

        // Jump
        jumpReady = jumpReady || (grounded && !jumping);
        if (Input.GetKeyDown(KeyCode.Space) && jumpReady)
        {
            Services.Game.jumped = true;
            jumpPivot.transform.DOPunchRotation(jumpPivot.transform.right * -45f, jumpDuration, 1, 0.2f);
            jumpPivot.transform.DOLocalJump(jumpPivot.transform.localPosition, 3f, 1, jumpDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                jumping = false;
            });
            jumping = true;
            jumpReady = false;
            
        }

        // Swing
        float swing = 0f;
        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)) swing = -1f;
        //else if (Input.GetKeyDown(KeyCode.RightShift)) swing = -1f;
        if (swing != 0f && !swinging)
        {
            swingPivot.transform.DOLocalRotate(new Vector3(0f, 360f * swing, 0f), swingDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad);
            DOTween.Sequence().AppendInterval(hitDelay).AppendCallback(() =>
            {
                swingHitbox.SetActive(true);
            }).AppendInterval(hitDuration).AppendCallback(() =>
            {
                swingHitbox.SetActive(false);
            });
            swingPivot.transform.DOPunchPosition(new Vector3(swing * 0.5f, 0f, -1.5f), swingDuration, 1, 1f).OnComplete(() =>
            {
                swinging = false;
            });
            swinging = true;
        }

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetMouseButtonDown(1)) && Services.Game.stars.Count > 0)
        {
            Services.Game.stars.Dequeue().Recall();
        }

        // Holes
        if (!grounded && !jumping || transform.position.y < -2f)
        {
            transform.position -= Vector3.up * 10f * Time.deltaTime;
        }
        else if (transform.position.y < initialY)
        {
            transform.position += Vector3.up * 10f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            if (invincible) return;
            health -= 1;
            Sequence blink = DOTween.Sequence();
            for (int i = 1; i <= 6; i++)
            {
                blink.Append(model.material.DOColor(damageColor, 0f));
                blink.AppendInterval(0.08f);
                blink.Append(model.material.DOColor(Color.white, 0f));
                blink.AppendInterval(0.08f);
            }
            blink.AppendCallback(() =>
            {
                invincible = false;
            });
            invincible = true;
        }
    }
}
