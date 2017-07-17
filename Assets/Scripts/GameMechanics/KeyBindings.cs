using System;
using System.Collections.Generic;
using System.Linq;
using GameMechanics;
using UnityEngine;
using UnityEngine.Events;

public class KeyBindings : MonoBehaviour
{
    [Serializable]
    public class KeyBinding
    {
        public string input;
        public KeyCode KeyCode { get; private set; }

        public KeyBinding(KeyCode keyCode, string p_Input)
        {
            KeyCode = keyCode;
            input = p_Input;
        }
    }

    public List<KeyBinding> keyBindings = new List<KeyBinding>();

    private void Awake()
    {
        var m_PrivateKeyBindings = 
            keyBindings.Select(keyBinding => new KeyBinding((KeyCode) 48 + keyBindings.IndexOf(keyBinding), keyBinding.input)).ToList();
        keyBindings = m_PrivateKeyBindings;
    }

	private void Update ()
    {
        foreach (var keyBinding in keyBindings)
        {
            if(Input.GetKeyUp(keyBinding.KeyCode))
                CommandBehaviors.self.CommandExecute(keyBinding.input);
        }
	}
}
