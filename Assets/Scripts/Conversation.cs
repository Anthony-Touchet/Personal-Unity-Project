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
    private bool done;

    public List<Line> conversationLines;
    public List<BranchingLine> brancingLine;
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
        PlayLine();
    }
	
	private void Update()
	{
        //Check to see if we should speak
        if (done)
            return;

        //If the Audio is playing, Don't worry
        if (m_AudioSource.isPlaying)
            return;

        // If it is not playing, count down
        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
            return;
        }

        // After countdown, reset timer
        m_Timer = waitAfterLine;

        for (var i = 0; i < conversationLines.Count; i++)
        {
            if (conversationLines[i] == m_CurrentLine && i + 1 != conversationLines.Count)
            {
                m_CurrentLine = conversationLines[i + 1];
                break;
            }

            if (i + 1 < conversationLines.Count) continue;

            m_CurrentLine = null;
            m_AudioSource.clip = null;
            done = true;
            return;
        }

        // If the next line is a line, set it up, else
        if (m_CurrentLine.GetType() == typeof(Line))
        {
            PlayLine();
        }

        else if (m_CurrentLine.GetType() == typeof(BranchingLine))
        {

        }
    }

    // Dialogue can now be restarted.
    [ContextMenu("Restart")]
    public void RestartDialogue()
    {
        m_CurrentLine = conversationLines[0];
        done = false;
        PlayLine();
    }

    private void PlayLine()
    {
        m_AudioSource.clip = m_CurrentLine.sourceClip;
        m_AudioSource.Play();
    }
}


