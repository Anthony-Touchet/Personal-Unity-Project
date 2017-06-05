/*
This class is designed to allow an action after the line has been spoken. The function will need a target 
to preform the action on.
*/

using System;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    [Serializable]
    [CreateAssetMenu(menuName = "Dialogue/Action Line", fileName = "Action Line")]
    public class ActionLine : Line
    {
        public enum ActionType
        {
            ROTATE,
            TEXT,
            NUMBERS
        }

        public ActionType actionType;

        public void Execute(GameObject target)
        {
            switch (actionType)
            {
                case ActionType.ROTATE:
                    var rot = target.AddComponent<RotateGameObject>();
                    rot.angle = Vector3.forward * 90;
                    break;

                case ActionType.TEXT:
                    target.GetComponent<Text>().text = "GFTYDVUCTBYRF";
                    break;

                case ActionType.NUMBERS:
                    target.GetComponent<Text>().text = "1436841864864";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
