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
public class BranchingLine : Line
{
    [Serializable]
    public class Reaction
    {
        public Line initalLine;
        public Line reactionLine;
    }

    public List<Reaction> reactions = new List<Reaction>();

    public void PlayChoice(int choice, string text)
    {
        
    }
}