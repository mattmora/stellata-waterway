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

    public ulong score;

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
    public bool jumping;
    private bool jumpReady;
    public bool grounded;
    private Vector3 baseJumpPosition;
    private Sequence lastJump;
    private float initialY;
    private bool swinging;
    private bool invincible;

    public GameObject swingHitbox;

    public Renderer model;
    public Color damageColor;

    private bool saved;

    public AudioSource jumpAudio;
    public AudioSource doubleJumpAudio;
    public AudioSource landAudio;
    public AudioSource swingAudio;
    public AudioSource hitAudio;
    public AudioSource recallAudio;
    public AudioSource damageAudio;

    private void Awake()
    {
        Services.Player = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        initialY = jumpPivot.transform.position.y;
        baseJumpPosition = jumpPivot.transform.localPosition;
        saved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (score > Services.Game.highScore) Services.Game.SetHighScore(score);

        // Guard against tutorial exploit
        if (score > 5000) Services.Game.recalled = true;

        if (jumpPivot.transform.position.y < initialY - 5f)
        {
            if (health > 0) damageAudio.Play();
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
        tunnel.scrollSpeed = effectiveMoveSpeed * scrollSpeedRatio * (1f + (score / (150000f + score * 0.25f)));
        animator.speed = (vInput * 0.2f) + 0.7f;
        mainCamera.fieldOfView = (vInput * 3f) + 60f;

        Services.Game.targetNoisePitch = (vInput * 0.15f) + 1f;
        Services.Game.targetNoiseVolume = jumping ? 0f : 1f;

        // Y Rotation
        transform.rotation = Quaternion.Euler(0f, hInput * (30f - (vInput * 10f)), 0f);

        grounded = false;
        if (Physics.SphereCast(jumpPivot.transform.position, 0.5f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
        {
            grounded = hit.collider.CompareTag("Ground");
        }

        // Jump
        jumpReady = jumpReady || (grounded && !jumping);
        if (Input.GetKeyDown(KeyCode.Space) && jumpReady)
        {
            Services.Game.jumped = true;

            float basePower = 3f;
            if (!jumping)
            {
                jumpAudio.Play();
                jumpPivot.transform.DOPunchRotation(jumpPivot.transform.right * -45f, jumpDuration, 1, 0.2f);
            }
            else
            {
                basePower = 2f;
                doubleJumpAudio.Play();
                Services.Game.doubleJumped = true;
            }
            if (lastJump != null && lastJump.IsPlaying()) lastJump.Kill();
            float offset = baseJumpPosition.y - jumpPivot.transform.position.y;
            float under = Mathf.Max(offset - basePower, 0f);
            float power = Mathf.Max(basePower - offset, 0f);
            float duration = jumpDuration - (0.5f * offset * 0.3333f * jumpDuration);
            lastJump = jumpPivot.transform.DOLocalJump(baseJumpPosition - Vector3.up * under, power, 1, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (grounded) landAudio.Play();
                jumping = false;
                lastJump = null;
            });
            jumping = true;
            jumpReady = false;
        }

        // Swing
        float swing = 0f;
        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)) swing = -1f;
        //else if (Input.GetKeyDown(KeyCode.RightShift)) swing = -1f;
        if (swing != 0f && !swinging && health > 0)
        {
            swingAudio.Play();
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

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetMouseButtonDown(1)) && Services.Game.stars.Count > 0 && health > 0)
        {
            recallAudio.Play();
            Services.Game.stars.Dequeue().Recall();
        }

        // Holes
        if (!grounded && !jumping)
        {
            jumpPivot.transform.position -= Vector3.up * 8.5f * Time.deltaTime;
        }
        //else if (transform.position.y < initialY)
        //{
        //    transform.position += Vector3.up * 10f * Time.deltaTime;
        //}
    }

    public void RefreshJump()
    {
        jumpReady = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            if (invincible) return;
            damageAudio.Play();
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
