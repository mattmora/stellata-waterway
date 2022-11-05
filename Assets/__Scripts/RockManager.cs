using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    public GameObject rockPrefab;
    public List<Rock> rocks;

    private void Awake()
    {
        Services.Rocks = this;

        rocks = new List<Rock>();
    }
}
