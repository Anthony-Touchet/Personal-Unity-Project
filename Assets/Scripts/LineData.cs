using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line
{
    public AudioClip sourceClip;
    public string line;
    public uint index;
}

[Serializable]
public class Reaction
{
    public Line initalLine;
    public Line reactionLine;
}

[Serializable]
public class BranchingLine : Line
{
    public List<Reaction> reactions = new List<Reaction>();
}

[Serializable]
public class HubLine : Line
{
    public List<Reaction> choicesList = new List<Reaction>();
}