/* This line allows for the "Illusion of choice". The way this line will be used is as follows:
1. The HubLine.sourceClip will be played as the first line.
2. The player will then be promted by choices.
3. Only one choice will advance the conversation from the hub line
4. All other choices will in turn cause the hubline to repreat itself
*/

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
