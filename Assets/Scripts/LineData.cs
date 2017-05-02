using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line
{
    public AudioClip sourceClip;
    public string line;
}

[Serializable]
public class BranchingLine : Line
{
    [Serializable]
    public class ReactionLine
    {
        public Line initalLine;
        public Line reactionLine;
    }

    public List<ReactionLine> reactionLines = new List<ReactionLine>();

    public void PlayChoice(int choice, string text)
    {
        
    }
}