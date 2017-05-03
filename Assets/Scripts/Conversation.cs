using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Conversation : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private Line m_CurrentLine;
    private float m_Timer;
    private bool m_Done;
    private bool m_ChoiceWaiting;
    private GameObject m_DialogueScreen;
    private List<Line> m_AllLines = new List<Line>();

    private IEnumerator corutineEnumerator;

    public float lineChoiceSpacing;
    [Range(0, 1)]
    public float lineChoiceSize;
    public float waitAfterLine;
    public Text lineText;

    [Space]
    public List<Line> conversationLines;
    public List<BranchingLine> brancingLine;

    private void Awake()
    {
        m_AllLines.AddRange(conversationLines);
        foreach (var bl in brancingLine)
        {
            m_AllLines.Add(bl);
        }

        m_AllLines = m_AllLines.OrderBy(l => l.index).ToList();

        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
        m_Timer = waitAfterLine;
        m_AudioSource = GetComponent<AudioSource>();
        if (m_AllLines.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        m_CurrentLine = m_AllLines[0];
        PlayLine(m_CurrentLine);
        corutineEnumerator = DialogueCoRutine();
    }
	
	private void Update()
	{

	    if(corutineEnumerator != null && !m_ChoiceWaiting)
            corutineEnumerator.MoveNext();
    }

    private IEnumerator DialogueCoRutine()
    {
        while (!m_Done)
        {
            //If the Audio is playing, Don't worry
            while(m_AudioSource.isPlaying)
                yield return null;

            // If it is not playing, count down
            while (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
                yield return null;
            }

            // After countdown, reset timer
            m_Timer = waitAfterLine;

            for (var i = 0; i < m_AllLines.Count; i++)
            {
                if (m_AllLines[i] == m_CurrentLine && i + 1 != m_AllLines.Count)
                {
                    m_CurrentLine = m_AllLines[i + 1];
                    break;
                }

                if (i + 1 < m_AllLines.Count) continue;

                m_CurrentLine = null;
                m_AudioSource.clip = null;
                m_Done = true;
                break;
            }

            if (m_Done)
                break;

            // If the next line is a line, set it up, else
            if (m_CurrentLine.GetType() == typeof(Line))
            {
                PlayLine(m_CurrentLine);
            }

            else if (m_CurrentLine.GetType() == typeof(BranchingLine))
            {
                PlayLine(m_CurrentLine);
                PopulateButtons((BranchingLine)m_CurrentLine);
                m_ChoiceWaiting = true;
            }
        }
        yield return null;
    }

    // Dialogue can now be restarted.
    [ContextMenu("Restart")]
    public void RestartDialogue()
    {
        m_CurrentLine = m_AllLines[0];
        m_Done = false;
        PlayLine(m_CurrentLine);
    }

    private void PlayLine(Line line)
    {
        lineText.text = line.line;
        m_AudioSource.clip = line.sourceClip;
        m_AudioSource.Play();
    }

    [ContextMenu("Spawn Button")]
    private void PopulateButtons(BranchingLine pLine)
    {
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");

        foreach (Transform go in m_DialogueScreen.transform)
        {
            Destroy(go.gameObject);
        }

        var textPlacement = 1f;
        foreach (var reaction in pLine.reactions)
        {
            var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;
            var buttonTransform = buttonGameObject.GetComponent<RectTransform>();
            buttonGameObject.transform.SetParent(m_DialogueScreen.transform);

            textPlacement -= lineChoiceSpacing;
            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, textPlacement);
            textPlacement -= lineChoiceSize;
            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, textPlacement);

            buttonTransform.offsetMax = Vector2.zero;
            buttonTransform.offsetMin = Vector2.zero;

            buttonTransform.GetComponentInChildren<Text>().text = reaction.initalLine.line;
            var reaction1 = reaction;
            var buttonComponet = buttonGameObject.GetComponent<Button>();
            buttonComponet.onClick.AddListener(() =>
            {
                PlayLine(reaction1.reactionLine);
                m_ChoiceWaiting = false;
                foreach (Transform go in m_DialogueScreen.transform)
                {
                    if (go != buttonTransform)
                        Destroy(go.gameObject);
                    else
                    {
                        buttonComponet.onClick.RemoveAllListeners();
                        buttonComponet.interactable = false;

                        buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, 0.5f +
                            lineChoiceSize / 2);
                        buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, 0.5f - 
                            lineChoiceSize / 2);
                    }
                }
            });
        }
    }
}