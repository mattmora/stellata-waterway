using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotation;

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }
}
