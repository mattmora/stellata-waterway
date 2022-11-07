using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarIndicator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Star nextStar = Services.Game.stars.Peek();
        if (nextStar != null)
        {
            transform.parent = nextStar.transform;
            transform.localPosition = new Vector3(0f, 0f, -2f);
            transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }
}
