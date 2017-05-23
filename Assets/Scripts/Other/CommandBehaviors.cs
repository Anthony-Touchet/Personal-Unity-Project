using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandBehaviors : MonoBehaviour
{
    private GameObject m_CurrentGameObject;
    private InputField m_InputField;
    private bool m_CommandProptUp;
    private CameraControl camControl;

    public delegate void Command();
    public delegate void CommandSingleParameter(string s);
    public delegate void CommandThreeParameters(string s1, string s2, string s3);

    private Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>();
    private Dictionary<string, CommandSingleParameter> singleParameters = 
        new Dictionary<string, CommandSingleParameter>();
    private Dictionary<string, CommandThreeParameters> threeParameters =
       new Dictionary<string, CommandThreeParameters>();


    public Text executionText;
    public GameObject canvasGameObject;

	// Use this for initialization
	private void Awake ()
	{
	    AddFunctions();
        camControl = FindObjectOfType<CameraControl>();
        m_InputField = FindObjectOfType<InputField>();
	    m_CurrentGameObject = null;
	    executionText.text = "";

        canvasGameObject.SetActive(m_CommandProptUp);
	}
	
	// Update is called once per frame
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
        if (Input.GetKeyUp(KeyCode.Mouse0) && EventSystem.current.currentSelectedGameObject == null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if (hit.transform == null)
                return;

            m_CurrentGameObject = hit.transform.gameObject;
            UpdateBoard(m_CurrentGameObject.name + " was selected.\n");
        }
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

        threeParameters.Add("rotate", RotateCurrent);
        threeParameters.Add("moveposition", MovePosition);
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
    }

    private void ApplyRandomColor()
    {
        var c = m_CurrentGameObject.AddComponent<RandomColor>();
        c.waitTime = .1f;
    }

    private void ApplyRandomColor(string wait)
    {
        var waitTime = float.Parse(wait);
        var c = m_CurrentGameObject.AddComponent<RandomColor>();
        c.waitTime = waitTime;
    }

    private void MovePosition(string x, string y, string z)
    {
        var pos = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        m_CurrentGameObject.transform.position = pos;
    }

    private void Spawn(string path)
    {
        Instantiate(Resources.Load(path));
    }
}
