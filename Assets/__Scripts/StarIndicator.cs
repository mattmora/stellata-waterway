using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarIndicator : MonoBehaviour
{
    public Star star;
    private SpriteRenderer sprite;

    private Vector3 nextPosition;
    private Quaternion nextRotation;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sprite.color = star.comboColor;
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (star.activated)
        {
            bool isNext = Services.Game.stars.Count > 0 && (Services.Game.stars.Peek() == star);
            sprite.enabled = isNext || star.recalled;
            //if (star.recalled)
            //{

            //}
            //else
            //{
            //    transform.SetPositionAndRotation(nextPosition, nextRotation);
            //}
        }
    }
}
