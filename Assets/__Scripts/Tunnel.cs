using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    public float scrollSpeed;

    public List<TunnelSegment> segments;

    private Vector3 loopStart;
    private Vector3 loopEnd;

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
                SetNextSegment(segment);
                segment.transform.position = loopStart + new Vector3(0f, 0f, z);
            }
        }
    }

    private void SetNextSegment(TunnelSegment segment)
    {

    }
}
