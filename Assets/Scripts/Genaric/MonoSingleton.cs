using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// The ?? operator returns the left-hand operand if it is not null, or else it returns the right operand.

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Self;

    public static T self
    {
        get { return m_Self ?? (m_Self = FindObjectOfType<T>()); }
    }

    private void Awake()
    {
        if(self != this)
            Destroy(this);

        SubAwake();
    }

    public virtual void SubAwake() { }
}
