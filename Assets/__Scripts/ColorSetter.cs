using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ColorSetter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera camera;
    private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;

    public Palette colorPalette;
    public int colorIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetColorIndex(colorIndex);
    }

    public void SetColorIndex(int index)
    {
        colorIndex = index;
        Color color = colorPalette.colors[colorIndex];

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.color = color;
        }

        camera = GetComponent<Camera>();
        if (camera)
        {
            camera.backgroundColor = color;
        }

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer)
        {
            trailRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
