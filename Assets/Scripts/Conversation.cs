using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Conversation : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private Line m_CurrentLine;
    private float m_Timer;
    private IEnumerator corutine;

    public List<Line> conversationLines;
    public float waitAfterLine;

    void Awake()
    {
        m_Timer = waitAfterLine;
        m_AudioSource = GetComponent<AudioSource>();
        if (conversationLines.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        m_CurrentLine = conversationLines[0];
        m_AudioSource.clip = m_CurrentLine.sourceClip;
        m_AudioSource.Play();
        //corutine = DialogueCoRutine();
    }
	
	void Update()
    {
        if (m_AudioSource.isPlaying)
            return;

        m_Timer -= Time.deltaTime;
        if (m_Timer > 0)
            return;

        m_Timer = waitAfterLine;

        if (m_CurrentLine.GetType() == typeof(Line))
        {
            for (var i = 0; i < conversationLines.Count; i++)
            {
                if (conversationLines[i] == m_CurrentLine && i + 1 != conversationLines.Count)
                {
                    m_CurrentLine = conversationLines[i + 1];
                    break;
                }

                if (i + 1 != conversationLines.Count) continue;

                m_CurrentLine = conversationLines[0];
                break;
            }

            m_AudioSource.clip = m_CurrentLine.sourceClip;
            m_AudioSource.Play();
        }

        else if (m_CurrentLine.GetType() == typeof(BranchingLine))
        {

        }

        //if(corutine != null)
        //corutine.MoveNext();
    }

    //private IEnumerator DialogueCoRutine()
    //{
    //    while (true)
    //    {
    //        if (m_AudioSource.isPlaying)
    //            return null;

    //        m_Timer -= Time.deltaTime;
    //        if (m_Timer > 0)
    //            return null;

    //        m_Timer = waitAfterLine;

    //        m_AudioSource.clip = m_CurrentLine.sourceClip;
    //        m_AudioSource.Play();

    //        if (m_CurrentLine.GetType() == typeof(Line))
    //        {
    //            for (var i = 0; i < conversationLines.Count; i++)
    //            {
    //                if (conversationLines[i] == m_CurrentLine && i + 1 != conversationLines.Count)
    //                {
    //                    m_CurrentLine = conversationLines[i + 1];
    //                    break;
    //                }

    //                if (i + 1 != conversationLines.Count) continue;

    //                m_CurrentLine = conversationLines[0];
    //                break;
    //            }
    //        }

    //        else if (m_CurrentLine.GetType() == typeof(BranchingLine))
    //        {

    //        }
    //    }
    //}
}
