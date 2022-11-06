using Assets.Pixelation.Example.Scripts;
using UnityEngine;

namespace Assets.Pixelation.Scripts
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Pixelation")]
    public class Pixelation : ImageEffectBase
    {
        [Range(16.0f, 1024f)] public float BlockCount = 128;

        private Camera mainCamera;
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            float k = mainCamera.aspect;
            Vector2 count = new Vector2(BlockCount, BlockCount/k);
            Vector2 size = new Vector2(1.0f/count.x, 1.0f/count.y);
            //
            material.SetVector("BlockCount", count);
            material.SetVector("BlockSize", size);
            Graphics.Blit(source, destination, material);
        }
    }
}