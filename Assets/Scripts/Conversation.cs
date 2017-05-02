using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Conversation : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private Line m_CurrentLine;
    private float m_Timer;
    private bool done;
    private bool choiceWaiting;
    private GameObject m_DialogueScreen;

    public List<Line> conversationLines;
    public List<BranchingLine> brancingLine;
    public float waitAfterLine;

    void Awake()
    {
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
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

	    if (m_CurrentLine.GetType() == typeof(BranchingLine) && choiceWaiting)
	    {

	        return;
	    }

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
            PlayLine();
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

    [ContextMenu("Spawn Button")]
    private void PopulateButtons(/*BranchingLine pLine*/)
    {
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");

        float spacing = 1f;
        foreach (var line in brancingLine[0].reactions)
        {
            var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;
            var buttonTransform = buttonGameObject.GetComponent<RectTransform>();
            buttonGameObject.transform.SetParent(m_DialogueScreen.transform);

            spacing -= .01f;
            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, spacing);
            spacing -= .06f;
            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, spacing);

            buttonTransform.offsetMax = Vector2.zero;
            buttonTransform.offsetMin = Vector2.zero;

            buttonTransform.GetComponentInChildren<Text>().text = line.initalLine.line;
            buttonGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                // TODO: Put what happens when the button is pressed here
            });
        }
    }
}


