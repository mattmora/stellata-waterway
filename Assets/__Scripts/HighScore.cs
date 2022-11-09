using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ulong score = Services.Game.highScore;
        if (score < 1000000000)
        {
            text.text = score.ToString("D9");
        }
        else
        {
            string s = score.ToString("D11");
            s = s.Remove(s.Length - 3);
            text.text = s + "K";
        }
    }
}
