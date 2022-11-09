using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public TrailRenderer trail;
    public float initialVelocity;
    public float velocityIncrement;
    public float rotation;
    private float baseRotation;
    public float rotationIncrement;
    private Rigidbody rb;
    private Collider coll;
    public Renderer rend;

    public bool activated;
    public bool recalled;

    private float maxVelocity;
    private float zVelocity;
    private float zVelocityDelta;

    public int scoreValue;
    public float combo;
    public Color comboColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        maxVelocity = initialVelocity;
        baseRotation = rotation;
        comboColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    private void Start()
    {
        rend.material.color = Color.Lerp(Color.white, comboColor, combo / 20f);
        trail.material.color = Color.Lerp(Color.white, comboColor, combo / 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated) return;

        if (recalled)
        {
            zVelocity = Mathf.SmoothDamp(zVelocity, -(maxVelocity + Services.MainTunnel.scrollSpeed * 2f), ref zVelocityDelta, 1f);
        }
        else
        {
            zVelocity = Mathf.SmoothDamp(zVelocity, 0f, ref zVelocityDelta, 4f * (initialVelocity / maxVelocity));
            transform.RotateAround(new Vector3(0f, 6.25f, transform.position.z), Vector3.forward, rotation * Time.deltaTime);
        }

        if (transform.position.z < -30f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + Vector3.forward * zVelocity * Time.fixedDeltaTime);
    }

    public void Recall()
    {
        coll.enabled = true;
        recalled = true;
        transform.parent = Services.MainTunnel.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            if (recalled) Services.Game.recalled = true;
            Services.Player.hitAudio.pitch = Mathf.Min(0.8f + (combo * 0.05f), 3f);
            Services.Player.hitAudio.Play();
            Services.Player.RefreshJump();
            Services.Game.stars.Enqueue(this);
            Services.Game.hit = true;
            coll.enabled = false;
            activated = true;
            transform.parent = null;
            trail.emitting = true;
            maxVelocity = initialVelocity + velocityIncrement * combo;
            rotation = baseRotation + rotationIncrement * combo;
            zVelocity = maxVelocity;
            zVelocityDelta = 0f;
            recalled = false;
            combo++;

            Services.Player.score += (ulong)(scoreValue * Mathf.Pow(combo, 1.2f) * (1f + (Services.Game.stars.Count / 10f)));

            rend.material.color = Color.Lerp(Color.white, comboColor, combo / 20f);
            trail.material.color = Color.Lerp(Color.white, comboColor, combo / 20f);

            Sequence hitStop = DOTween.Sequence();
            hitStop.Append(DOTween.To(() => Time.timeScale, t => Time.timeScale = t, 0.1f, 0.05f));
            hitStop.AppendInterval(0.2f);
            hitStop.Append(DOTween.To(() => Time.timeScale, t => Time.timeScale = t, 1f, 0.05f));
            hitStop.SetUpdate(true);
        }
    }
}
