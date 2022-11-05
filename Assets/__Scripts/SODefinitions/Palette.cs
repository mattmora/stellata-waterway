using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/PaletteScriptableObject", order = 50)]
public class Palette : ScriptableObject
{
    public List<Color> colors;
}
