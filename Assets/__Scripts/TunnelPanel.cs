using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPanel : MonoBehaviour
{
    public GameObject panel;

    public List<Transform> slots;

    public void Set(Tunnel.PanelInfo state)
    {
        panel.SetActive(state.active);

        switch (state.spikeCount) {
            case 0: break;
            case 1:
                Instantiate(Services.Game.spikePrefab, slots[4]);
                break;
            case 2:
                Instantiate(Services.Game.spikePrefab, slots[3]);
                Instantiate(Services.Game.spikePrefab, slots[5]);
                break;
            case 3:
                Instantiate(Services.Game.spikePrefab, slots[0]);
                Instantiate(Services.Game.spikePrefab, slots[8]);
                break;
            case 4:
                Instantiate(Services.Game.spikePrefab, slots[1]);
                Instantiate(Services.Game.spikePrefab, slots[7]);
                break;
            case 5:
                Instantiate(Services.Game.spikePrefab, slots[2]);
                Instantiate(Services.Game.spikePrefab, slots[6]);
                break;
            default:
                break;
        }

        if (state.ballIndex >= 0)
        {
            float offset = (state.ballIndex / 9) * -1f;
            GameObject obj = Instantiate(Services.Game.ballPrefab, slots[state.ballIndex % 9]);
            obj.transform.position += Vector3.forward * offset;
        }
    }

    public void Clear()
    {
        panel.SetActive(true);
        foreach (Transform t in slots)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Destroy(t.GetChild(i).gameObject);
            }
        }
    }
}
