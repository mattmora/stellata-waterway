using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tunnel;

public class GameManager : MonoBehaviour
{
    public GameObject spikePrefab;
    public GameObject ballPrefab;

    public GameObject moveTutorial;
    public GameObject hitTutorial;
    public GameObject recallTutorial;
    public GameObject reloadPrompt;

    public List<List<PanelInfo>> tunnelData;

    public Queue<Star> stars;

    public bool moved;
    public bool jumped;
    public bool hit;
    public bool recalled;
    public bool tutorialDone;

    private void Awake()
    {
        if (Services.Game == null)
        {
            Services.Game = this;
            stars = new Queue<Star>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Services.Game.moveTutorial = moveTutorial;
            Services.Game.hitTutorial = hitTutorial;
            Services.Game.recallTutorial = recallTutorial;
            Services.Game.reloadPrompt = reloadPrompt;
            Services.Game.stars = new Queue<Star>();
            Destroy(this);
        }

        List<Dictionary<string, object>> data = CSVReader.Read("tunnel");
        tunnelData = new List<List<PanelInfo>>();
        foreach (Dictionary<string, object> row in data)
        {
            List<PanelInfo> rowData = new List<PanelInfo>();
            for (int i = 0; i < 12; i++)
            {
                string infoString = row[i.ToString()].ToString();
                bool active = infoString.Contains("p");
                int spikeCount = CharacterCount(infoString, "s");
                int ballIndex = -1;
                if (int.TryParse(infoString.Replace("p", "").Replace("s", ""), out int index))
                {
                    ballIndex = index;
                }
                PanelInfo info = new PanelInfo(active, spikeCount, ballIndex);
                rowData.Add(info);
            }
            tunnelData.Add(rowData);
        }
    }

    private void Update()
    {
        bool showReload = Services.Player.health <= 0;
        bool showMove = !moved || !jumped;
        bool showHit = !hit && Services.MainTunnel.dataIndex > 64;
        bool showRecall = !recalled && Services.MainTunnel.dataIndex > 64;

        reloadPrompt.SetActive(showReload);
        moveTutorial.SetActive(!showReload && showMove);
        hitTutorial.SetActive(!showReload && !showMove && showHit);
        recallTutorial.SetActive(!showReload && !showMove && !showHit && showRecall);
        tutorialDone = !showReload && !showMove && !showHit && !showRecall;
    }

    private static int CharacterCount(string s, string ch)
    {
        return s.Length - s.Replace(ch, "").Length;
    }
}
