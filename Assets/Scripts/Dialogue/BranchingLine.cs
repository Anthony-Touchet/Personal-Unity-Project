using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    [CreateAssetMenu(menuName = "Dialogue/Branching Line", fileName = "Branching Line")]
    public class BranchingLine : Line
    {
        public List<Reaction> reactions = new List<Reaction>();
    }
}