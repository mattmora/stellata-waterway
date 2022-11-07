using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spikePrefab;
    public GameObject ballPrefab;

    public GameObject moveTutorial;
    public GameObject hitTutorial;
    public GameObject recallTutorial;
    public GameObject reloadPrompt;
    public GameObject midairTip;

    public List<List<Tunnel.PanelInfo>> tunnelData;

    public Queue<Star> stars;

    public bool moved;
    public bool jumped;
    public bool hit;
    public bool recalled;
    public bool tutorialDone;
    public bool doubleJumped;

    public int highScore;

    public class SaveData 
    {
        public int highScore;

        public SaveData(int highScore)
        {
            this.highScore = highScore;
        }
    }

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
            Services.Game.midairTip = midairTip;
            Services.Game.stars = new Queue<Star>();
            Destroy(this);
            return;
        }

        Load();

        List<Dictionary<string, object>> data = CSVReader.Read("tunnel");
        tunnelData = new List<List<Tunnel.PanelInfo>>();
        foreach (Dictionary<string, object> row in data)
        {
            List<Tunnel.PanelInfo> rowData = new List<Tunnel.PanelInfo>();
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
                Tunnel.PanelInfo info = new Tunnel.PanelInfo(active, spikeCount, ballIndex);
                rowData.Add(info);
            }
            tunnelData.Add(rowData);
        }
    }

    private void Update()
    {
        bool showReload = Services.Player.health <= 0;
        bool showMove = !moved || !jumped;
        bool showHit = !hit;
        bool showRecall = !recalled;

        reloadPrompt.SetActive(showReload);

        moveTutorial.SetActive(!showReload && showMove);
        hitTutorial.SetActive(!showReload && !showMove && showHit && Services.MainTunnel.dataIndex > 64);
        recallTutorial.SetActive(!showReload && !showMove && !showHit && showRecall && Services.MainTunnel.dataIndex > 64);

        tutorialDone = !showMove && !showHit && !showRecall;

        bool showMidair = tutorialDone && !doubleJumped && Services.MainTunnel.dataIndex > 208 && Services.MainTunnel.dataIndex < 240;
        midairTip.SetActive(!showReload && showMidair);
    }

    private static int CharacterCount(string s, string ch)
    {
        return s.Length - s.Replace(ch, "").Length;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(new SaveData(highScore));
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
        }
    }
}
