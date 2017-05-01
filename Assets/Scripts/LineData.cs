using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Line
{
    public AudioClip sourceClip;
    public string line;

    public virtual void Play(string text) {}
}

public class InstantLine : Line { }

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