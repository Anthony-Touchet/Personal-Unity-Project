using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Other
{
    public class CommandBehaviors : MonoBehaviour
    {
        private GameObject m_CurrentGameObject;
        private InputField m_InputField;
        private bool m_CommandProptUp;
        private FirstPersonCamera camControl;

        public delegate void Command();
        public delegate void CommandSingleParameter(string s);
        public delegate void CommandDoubleParameter(string s1, string s2);
        public delegate void CommandThreeParameters(string s1, string s2, string s3);

        // Command with only names
        private readonly Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();

        // Comands that take in one parameter
        private readonly Dictionary<string, CommandSingleParameter> singleParameters = 
            new Dictionary<string, CommandSingleParameter>();

        // Commands that take in two parameters
        private readonly Dictionary<string, CommandDoubleParameter> doubleParameters =
            new Dictionary<string, CommandDoubleParameter>();

        // Commands that take in three parameters.
        private readonly Dictionary<string, CommandThreeParameters> threeParameters =
            new Dictionary<string, CommandThreeParameters>();

        public Text executionText;
        public GameObject canvasGameObject;
        public Text currentObjectText;

        private void Awake ()
        {
            AddFunctions();
            camControl = FindObjectOfType<FirstPersonCamera>();
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
                camControl.enabled = !m_CommandProptUp;
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

            if (hit.transform.gameObject.tag != "Commandable")
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
                        commandDictionary[parameters[0]].Invoke();
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 2:
                    try
                    {
                        singleParameters[parameters[0]].Invoke(parameters[1]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 3:
                    try
                    {
                        doubleParameters[parameters[0]].Invoke(parameters[1], parameters[2]);
                    }
                    catch
                    { history += "ERROR!!\n"; }
                    break;

                case 4:
                    try
                    {
                        threeParameters[parameters[0]].Invoke(parameters[1], parameters[2], parameters[3]);
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
            commandDictionary.Add("destroy", DestroyCurrent);
            commandDictionary.Add("rotate", RotateCurrent);
            commandDictionary.Add("color", ApplyRandomColor);

            singleParameters.Add("rotate", RotateCurrent);
            singleParameters.Add("color", ApplyRandomColor);
            singleParameters.Add("spawn", Spawn);
            singleParameters.Add("remove", RemoveComponet);
            singleParameters.Add("add", AddComponet);
            singleParameters.Add("float", FloatGameObject);

            doubleParameters.Add("rotate", RotateCurrent);

            threeParameters.Add("rotate", RotateCurrent);
            threeParameters.Add("move", MovePosition);
            threeParameters.Add("color", ApplyColor);
        }

        private void RotateCurrent()
        {
            var r = m_CurrentGameObject.AddComponent<RotateGameObject>();
            r.angle = Vector3.up * 90;
        }

        private void RotateCurrent(string s)
        {
            var mag = float.Parse(s);
            var r = m_CurrentGameObject.AddComponent<RotateGameObject>();
            r.angle = Vector3.up * mag;
        }

        private void RotateCurrent(string axis, string angle)
        {
            var angleValue = float.Parse(angle);
            switch (axis)
            {
                case "x":
                    m_CurrentGameObject.transform.Rotate(Vector3.right, angleValue);
                    break;
                case "y":
                    m_CurrentGameObject.transform.Rotate(Vector3.up, angleValue);
                    break;
                case "z":
                    m_CurrentGameObject.transform.Rotate(Vector3.forward, angleValue);
                    break;

                default:
                    throw new Exception("No Axis Found");
            }
        }

        private void RotateCurrent(string s1, string s2, string s3)
        {
            var x = float.Parse(s1);
            var y = float.Parse(s2);
            var z = float.Parse(s3);
            var r = m_CurrentGameObject.AddComponent<RotateGameObject>();
            r.angle = new Vector3(x,y,z);
        }

        private void DestroyCurrent()
        {
            Destroy(m_CurrentGameObject);

            var obj = Resources.Load("Destroy Particles") as GameObject;
            if(!obj) return;

            obj = Instantiate(obj);

            obj.transform.position = m_CurrentGameObject.transform.position;
        }

        private void ApplyRandomColor()
        {
            if (m_CurrentGameObject.transform.GetComponent<RandomColor>())
                throw new Exception();

            var c = m_CurrentGameObject.AddComponent<RandomColor>();
            c.waitTime = .1f;
        }

        private void ApplyRandomColor(string wait)
        {
            if (m_CurrentGameObject.transform.GetComponent<RandomColor>())
                throw new Exception();

            var waitTime = float.Parse(wait);
            var c = m_CurrentGameObject.AddComponent<RandomColor>();
            c.waitTime = waitTime;
        }

        private void ApplyColor(string r, string g, string b)
        {
            var c = new Color(float.Parse(r), float.Parse(g), float.Parse(b));
            m_CurrentGameObject.GetComponent<MeshRenderer>().material.color = c;
        }

        private void MovePosition(string x, string y, string z)
        {
            var pos = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            m_CurrentGameObject.transform.position += pos;
        }

        private void Spawn(string path)
        {
            Instantiate(Resources.Load(path));
        }

        private void AddComponet(string comp)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var typelist = assembly.GetTypes().Where(t => string.Equals(t.Namespace, "Other", 
                StringComparison.Ordinal)).ToList();

            foreach (var type in typelist)
            {
                if (type.ToString() != "Other." + comp) continue;

                if(m_CurrentGameObject.GetComponent(type))
                    Destroy(m_CurrentGameObject.GetComponent(type));
                m_CurrentGameObject.AddComponent(type);
            }
        }

        private void RemoveComponet(string comName)
        {
            var componets = m_CurrentGameObject.GetComponents<Component>();
            foreach (var c in componets)
            {
                if (comName != c.GetType().ToString()) continue;

                Destroy(c);
                break;
            }
        }

        private void FloatGameObject(string mag)
        {
            if (m_CurrentGameObject.transform.GetComponent<Floating>())
                Destroy(m_CurrentGameObject.transform.GetComponent<Floating>());

            var magValue = float.Parse(mag);

            var floatComp = m_CurrentGameObject.AddComponent<Floating>();
            floatComp.magnitude = magValue;
        }
    }
}
