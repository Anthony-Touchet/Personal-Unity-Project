using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    [CreateAssetMenu(menuName = "Dialogue/Hub Line", fileName = "Hub Line")]
    public class HubLine : Line
    {
        public List<Reaction> choicesList = new List<Reaction>();
    }
}
