using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line
{
    public AudioClip sourceClip;
    public string line;

    public virtual void Play(string text) { }
}

[Serializable]
public class BranchingLine : Line
{
    public class ReactionLine
    {
        public Line initalLine;
        public Line reactionLine;
    }

    public List<ReactionLine> reactionLines = new List<ReactionLine>();

    public override void Play(string text)
    {
        
    }

    public void PlayChoice(int choice, string text)
    {
        
    }
}