using UnityEngine.UI;

namespace Dialogue
{
    public class Conversation2DBehavior : Conversation
    {
        public Image faceExpression;    // Image that will display the face of the character

        protected override void PlayLine (Line line)
        {
		    base.PlayLine(line);
            faceExpression.sprite = line.expression;    // Set face expresion
        }
    }
}
