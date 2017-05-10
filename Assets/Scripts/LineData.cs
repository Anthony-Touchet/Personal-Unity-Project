using System;
using System.Collections.Generic;
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

[Serializable]
public class Reaction
{
    public Line playerLine;
    public Line reactionLine;
}

[Serializable]
[CreateAssetMenu(menuName = "Dialogue/Branching Line", fileName = "Branching Line")]
public class BranchingLine : Line
{
    public List<Reaction> reactions = new List<Reaction>();
}

[Serializable]
[CreateAssetMenu(menuName = "Dialogue/Hub Line", fileName = "Hub Line")]
public class HubLine : Line
{
    public List<Reaction> choicesList = new List<Reaction>();
}