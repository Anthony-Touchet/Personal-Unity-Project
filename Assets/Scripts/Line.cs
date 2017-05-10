using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Dialogue/Line", fileName = "Line")]
public class Line : ScriptableObject
{
    public AudioClip sourceClip;
    public string line;
    public uint index;
    public Sprite expression;
}
