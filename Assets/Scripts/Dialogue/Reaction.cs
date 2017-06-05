// This class is used by other line classes to have a response lined with a choice that the player makes.

using System;

namespace Dialogue
{
    [Serializable]
    public class Reaction
    {
        public Line playerLine;
        public Line reactionLine;
    }
}
