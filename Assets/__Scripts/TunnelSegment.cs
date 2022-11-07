using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSegment : MonoBehaviour
{
    public List<TunnelPanel> panels;

    public void SetPanel(int index, Tunnel.PanelInfo state, bool clear)
    {
        if (clear) panels[index].Clear();
        panels[index].Set(state);
    }
}
