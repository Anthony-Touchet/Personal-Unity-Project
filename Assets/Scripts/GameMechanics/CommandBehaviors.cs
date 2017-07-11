using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMechanics
{
    public class CommandBehaviors : MonoSingleton<CommandBehaviors>
    {
        private GameObject m_CurrentGameObject;
        private InputField m_InputField;
        private bool m_CommandProptUp;

        public GameObject currentGameObject { get { return m_CurrentGameObject; } }

        public delegate void Command();
        public delegate void CommandSingleParameter(string s);
        public delegate void CommandDoubleParameter(string s1, string s2);
        public delegate void CommandThreeParameters(string s1, string s2, string s3);

        // Command with only names
        private readonly Dictionary<string, Command> m_CommandDictionary = new Dictionary<string, Command>();

        // Comands that take in one parameter
        private readonly Dictionary<string, CommandSingleParameter> m_SingleParameters = 
            new Dictionary<string, CommandSingleParameter>();

        // Commands that take in two parameters
        private readonly Dictionary<string, CommandDoubleParameter> m_DoubleParameters =
            new Dictionary<string, CommandDoubleParameter>();

        // Commands that take in three parameters.
        private readonly Dictionary<string, CommandThreeParameters> m_ThreeParameters =
            new Dictionary<string, CommandThreeParameters>();

        public Text executionText;
        public GameObject canvasGameObject;
        public Text currentObjectText;

        public override void SubAwake ()
        {
            AddFunctions();
            m_InputField = FindObjectOfType<InputField>();
            m_CurrentGameObject = null;
            executionText.text = "";
            currentObjectText.text = "";

            canvasGameObject.SetActive(m_CommandProptUp);
        }

        private void Update ()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                m_CommandProptUp = !m_CommandProptUp;
                canvasGameObject.SetActive(m_CommandProptUp);
            }

            if (m_CommandProptUp == false)
                return;

            // Get an object
            if (!Input.GetKeyUp(KeyCode.Mouse0) || EventSystem.current.currentSelectedGameObject != null)
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if (hit.transform == null)
                return;

            if (!hit.transform.gameObject.CompareTag("Commandable"))
                return;

            m_CurrentGameObject = hit.transform.gameObject;
            currentObjectText.text = m_CurrentGameObject.name;
        }

        public void CommandExecute()
        {
            var command = m_InputField.text;
            var history = command + "\n";
            var parameters = m_InputField.text.Split(' ').ToList();
            switch (parameters.Count)
            {
                case 1:
                    try
                    {
                        m_CommandDictionary[parameters[0]].Invoke();
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 2:
                    try
                    {
                        m_SingleParameters[parameters[0]].Invoke(parameters[1]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 3:
                    try
                    {
                        m_DoubleParameters[parameters[0]].Invoke(parameters[1], parameters[2]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 4:
                    try
                    {
                        m_ThreeParameters[parameters[0]].Invoke(parameters[1], parameters[2], parameters[3]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                default:
                    history += "ERROR!!\n";
                    break;
            }

            UpdateBoard(history);
        }

        public void CommandExecute(string command)
        {
            var history = command + "\n";
            var parameters = command.Split(' ').ToList();
            switch (parameters.Count)
            {
                case 1:
                    try
                    {
                        m_CommandDictionary[parameters[0]].Invoke();
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 2:
                    try
                    {
                        m_SingleParameters[parameters[0]].Invoke(parameters[1]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 3:
                    try
                    {
                        m_DoubleParameters[parameters[0]].Invoke(parameters[1], parameters[2]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 4:
                    try
                    {
                        m_ThreeParameters[parameters[0]].Invoke(parameters[1], parameters[2], parameters[3]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                default:
                    history += "ERROR!!\n";
                    break;
            }

            UpdateBoard(history);
        }

        public void UpdateBoard(string data)
        {
            var textHeight = executionText.rectTransform.rect.height - 
                             (executionText.rectTransform.rect.height * .15f);

            var numberOfLines = (int)(textHeight / executionText.fontSize);

            if (executionText.text.Count(c => c == '\n') >= numberOfLines - 1)
            {
                executionText.text = "";
            }
            executionText.text += data;
        }

        private void AddFunctions()
        {
            m_CommandDictionary.Add("destroy", Executions.DestroyCurrent);
            m_CommandDictionary.Add("rotate", Executions.RotateCurrent);
            m_CommandDictionary.Add("color", Executions.ApplyRandomColor);

            m_SingleParameters.Add("rotate", Executions.RotateCurrent);
            m_SingleParameters.Add("color", Executions.ApplyRandomColor);
            m_SingleParameters.Add("spawn", Executions.Spawn);
            m_SingleParameters.Add("remove", Executions.RemoveComponet);
            m_SingleParameters.Add("add", Executions.AddComponet);
            m_SingleParameters.Add("float", Executions.FloatGameObject);

            m_DoubleParameters.Add("rotate", Executions.RotateCurrent);

            m_ThreeParameters.Add("rotate", Executions.RotateCurrent);
            m_ThreeParameters.Add("move", Executions.MovePosition);
            m_ThreeParameters.Add("color", Executions.ApplyColor);
        }
    }
}
