using System;
using System.Collections.Generic;
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

        public KeyBinding(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public List<KeyBinding> keyBindings = new List<KeyBinding>();

    private void Awake()
    {
        var start = 48;
        for (var i = 0; i < 9; i++)
        {
            keyBindings.Add(new KeyBinding((KeyCode)start + i));
        }

        keyBindings[1].input = "destroy";
    }

	// Update is called once per frame
	void Update ()
    {
        foreach (var keyBinding in keyBindings)
        {
            if(Input.GetKeyUp(keyBinding.KeyCode))
                CommandBehaviors.self.CommandExecute(keyBinding.input);
        }
	}
}
