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
