using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public float scrollSpeed;

    public List<TunnelSegment> segments;

    private Vector3 loopStart;
    private Vector3 loopEnd;

    private PanelInfo empty = new PanelInfo(true, 0, -1);

    public class PanelInfo 
    {
        public bool active;
        public int spikeCount;
        public int ballIndex;

        public PanelInfo(bool active, int spikeCount, int ballIndex)
        {
            this.active = active;
            this.spikeCount = spikeCount;
            this.ballIndex = ballIndex;
        }
    }

    public int dataIndex;

    public bool simple;

    private void Awake()
    {
        if (!simple) Services.MainTunnel = this;
        dataIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        loopStart = segments[segments.Count - 1].transform.position;
        loopEnd = segments[0].transform.position;
        segments[0].gameObject.SetActive(false);
        segments.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TunnelSegment segment in segments)
        {
            segment.transform.position += new Vector3(0f, 0f, -scrollSpeed * Time.deltaTime);
            if (segment.transform.position.z < loopEnd.z)
            {
                float z = segment.transform.position.z - loopEnd.z;
                if (!simple)
                {
                    PopulateSegment(segment); 
                }
                segment.transform.position = loopStart + new Vector3(0f, 0f, z);
            }
        }
    }

    private void PopulateSegment(TunnelSegment segment)
    {
        List<PanelInfo> row = Services.Game.tunnelData[dataIndex];
        if (Services.Player.health <= 0 || (!Services.Game.tutorialDone && dataIndex > 65))
        {
            for (int i = 0; i < row.Count; i++)
            {
                segment.SetPanel(i, empty, true);
            }
        }
        else
        {
            for (int i = 0; i < row.Count; i++)
            {
                segment.SetPanel(i, row[i], true);
            }
            dataIndex++;
            dataIndex = dataIndex % Services.Game.tunnelData.Count;
        }
    }
}
