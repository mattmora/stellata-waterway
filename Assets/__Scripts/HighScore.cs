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
        int score = Services.Game.highScore;
        if (score < 100000)
        {
            text.text = score.ToString("D6");
        }
        else
        {
            string s = score.ToString("D8");
            s = s.Remove(s.Length - 3);
            text.text = s + "k";
        }
    }
}
