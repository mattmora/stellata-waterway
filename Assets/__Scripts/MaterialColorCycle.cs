using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorCycle : MonoBehaviour
{
    public float cycleRate;

    private Renderer rend;
    private Material material;

    private int propertyId;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        material = rend.sharedMaterial;

        propertyId = Shader.PropertyToID("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        Color currentColor = material.GetColor(propertyId);
        Color.RGBToHSV(currentColor, out float H, out float S, out float V);
        H += Mathf.Repeat(Time.deltaTime * cycleRate, 1.0f);
        material.SetColor(propertyId, Color.HSVToRGB(H, S, V));
    }
}
