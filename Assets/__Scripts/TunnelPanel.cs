using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPanel : MonoBehaviour
{
    public GameObject panel;

    public Transform frontLeft;
    public Transform frontCenter;
    public Transform frontRight;
    public Transform left;
    public Transform center;
    public Transform right;
    public Transform backLeft;
    public Transform backCenter;
    public Transform backRight;

    public void SetActive(bool active)
    {
        panel.SetActive(active);
    }

    public void Clear()
    {
        panel.SetActive(true);
        if (frontLeft != null) Destroy(frontLeft.gameObject);
        if (frontCenter != null) Destroy(frontCenter.gameObject);
        if (frontRight != null) Destroy(frontRight.gameObject);
        if (left != null) Destroy(left.gameObject);
        if (center != null) Destroy(center.gameObject);
        if (right != null) Destroy(right.gameObject);
        if (backLeft != null) Destroy(backLeft.gameObject);
        if (backCenter != null) Destroy(backCenter.gameObject);
        if (backRight != null) Destroy(backRight.gameObject);
    }
}
