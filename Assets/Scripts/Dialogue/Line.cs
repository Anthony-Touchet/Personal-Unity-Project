//This class is the base of all lines. If a normal line is to be played, the information will simply be
// displayed and the audio will begin to play.

using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    [CreateAssetMenu(menuName = "Dialogue/Line", fileName = "Line")]
    public class Line : ScriptableObject
    {
        public AudioClip sourceClip;
        public string line;
        public Sprite expression;
    }
}
