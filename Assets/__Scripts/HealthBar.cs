using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public List<Image> healthImages;
    public int barHealth;

    private void Start()
    {
        barHealth = Services.Player.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (barHealth != Services.Player.health)
        {
            for (int i = barHealth - 1; i >= Services.Player.health; i--)
            {
                Image image = healthImages[i];
                image.DOFade(0f, 0.3f);
                image.transform.DOScale(2f, 0.3f);

            }
            barHealth = Services.Player.health;
        }
    }
}
