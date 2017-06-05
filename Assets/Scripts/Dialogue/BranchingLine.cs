/*
 This class is another Line that aludes to an Illusion of choice. It allows the player to pick one onption,
 respond to that option, and continue the conversation forward.
 */

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