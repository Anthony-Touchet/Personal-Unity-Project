using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandBehaviors : MonoBehaviour
{
    private GameObject m_CurrentGameObject;
    private InputField m_InputField;

    public Text executionText;

	// Use this for initialization
	private void Awake ()
	{
	    m_InputField = FindObjectOfType<InputField>();
	    m_CurrentGameObject = null;
	    executionText.text = "";
	}
	
	// Update is called once per frame
	private void Update ()
    {
        // Get an object
        if (Input.GetKeyUp(KeyCode.Mouse0) && EventSystem.current.currentSelectedGameObject == null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if (hit.transform == null)
                return;

            m_CurrentGameObject = hit.transform.gameObject;
            executionText.text += m_CurrentGameObject.name + " was selected. \n";
        }
	}

    public void CommandExecute(string command)
    {
        var history = "";
        switch (m_InputField.text)
        {
            case "Destroy":
                try
                {
                    Destroy(m_CurrentGameObject);
                    history += m_CurrentGameObject.name + " was destroyed\n";
                }
                catch
                {
                    history += "No Gameobject Selected. \n";
                }
                
                break;

            default:
                history += "Command not found \n";
                break;
        }
        executionText.text += history;
    }
}
