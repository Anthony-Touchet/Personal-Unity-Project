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
    private bool m_Repeat;

    private GameObject m_DialogueScreen;
    private List<Line> m_AllLines = new List<Line>();

    private IEnumerator m_CorutineEnumerator;
    
    private Transform m_PlayerTransform;

    private List<Line> m_ConversationLines = new List<Line>();
    private List<BranchingLine> m_BrancingLine = new List<BranchingLine>();
    private List<HubLine> m_HubLines = new List<HubLine>();

    public bool is3D;
    public float range;
    [Space]
    public float lineChoiceSpacing;
    [Range(0, 1)] public float lineChoiceSize;
    public float waitAfterLine;
    [Space]
    public Text lineText;
    public Image faceExpression;

    [Space]
    public List<Line> conversationLines;
    public List<BranchingLine> brancingLine;
    public List<HubLine> hubLines;

    private void Awake()
    {
        // Get info
        foreach (var bl in brancingLine)
        {
            m_BrancingLine.Add(Instantiate(bl));
        }

        foreach (var cl in conversationLines)
        {
            m_ConversationLines.Add(Instantiate(cl));
        }

        foreach (var hl in hubLines)
        {
            m_HubLines.Add(Instantiate(hl));
        }

        // Put all the info together
        m_AllLines.AddRange(m_ConversationLines);
        foreach (var bl in m_BrancingLine)
        {
            m_AllLines.Add(bl);
        }
        foreach (var hl in m_HubLines)
        {
            m_AllLines.Add(hl);
        }

        // Sort the Info based on it's position within the the conversation(ie: the index)
        m_AllLines = m_AllLines.OrderBy(l => l.index).ToList();

        if (m_AllLines.Count == 0)
        {
            Destroy(this);
            return;
        }

        m_PlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
        m_Timer = waitAfterLine;
        m_AudioSource = GetComponent<AudioSource>();

        if (is3D) return;
        RestartDialogue();
        m_CorutineEnumerator = DialogueCoRutine();
    }

    private void Update()
    {
        if (is3D)
        {
            if ((m_PlayerTransform.position - transform.position).magnitude > range && m_CorutineEnumerator != null)
            {
                m_CorutineEnumerator = null;
                EndDialogue();
            }

            else if ((m_PlayerTransform.position - transform.position).magnitude <= range &&
                     m_CorutineEnumerator == null)
            {
                RestartDialogue();
                m_CorutineEnumerator = DialogueCoRutine();
            }
        }

        if (m_CorutineEnumerator != null && !m_ChoiceWaiting)
            m_CorutineEnumerator.MoveNext();
    }

    private IEnumerator DialogueCoRutine()
    {
        while (!m_Done)
        {
            //If the Audio is playing, Don't worry
            while (m_AudioSource.isPlaying)
                yield return null;

            // If it is not playing, count down
            while (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
                yield return null;
            }

            // After countdown, reset timer
            m_Timer = waitAfterLine;
            
            // If we repeat and are a hub line, repeat
            if (m_Repeat)
            {
                PlayLine(m_CurrentLine);
                PopulateButtons((HubLine)m_CurrentLine);
                m_ChoiceWaiting = true;
                m_Repeat = true;
                continue;
            }

            for (var i = 0; i < m_AllLines.Count; i++)
            {
                if (m_AllLines[i] == m_CurrentLine && i + 1 != m_AllLines.Count)
                {
                    m_CurrentLine = m_AllLines[i + 1];
                    break;
                }

                if (i + 1 < m_AllLines.Count) continue;

                EndDialogue();
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

            else if (m_CurrentLine.GetType() == typeof(HubLine))
            {
                PlayLine(m_CurrentLine);
                PopulateButtons((HubLine)m_CurrentLine);
                m_ChoiceWaiting = true;
                m_Repeat = true;
            }
        }
        yield return null;
    }

    // Dialogue can now be restarted.
    [ContextMenu("Restart")]
    public void RestartDialogue()
    {
        EndDialogue();

        m_CorutineEnumerator = null;
        m_CurrentLine = m_AllLines[0];
        m_Done = false;
        
        m_CorutineEnumerator = DialogueCoRutine();
        PlayLine(m_CurrentLine);
    }

    private void EndDialogue()
    {
        m_CurrentLine = null;
        m_AudioSource.clip = null;
        m_Done = true;
        lineText.text = "";

        if(m_AudioSource.isPlaying)
            m_AudioSource.Stop();

        if (m_DialogueScreen.transform.childCount <= 0) return;

        ClearChoices();
    }

    private void PlayLine(Line line)
    {
        lineText.text = line.line;
        m_AudioSource.clip = line.sourceClip;
        m_AudioSource.Play();
        faceExpression.sprite = line.expression;
    }

    [ContextMenu("Spawn Button")]
    private void PopulateButtons(BranchingLine pLine)
    {
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");

        ClearChoices();

        var textPlacement = 1f;
        foreach (var reaction in pLine.reactions)
        {
            var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;
            if (buttonGameObject == null) continue;
            var buttonTransform = buttonGameObject.GetComponent<RectTransform>();
            buttonGameObject.transform.SetParent(m_DialogueScreen.transform);

            textPlacement -= lineChoiceSpacing;
            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, textPlacement);
            textPlacement -= lineChoiceSize;
            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, textPlacement);

            buttonTransform.offsetMax = Vector2.zero;
            buttonTransform.offsetMin = Vector2.zero;

            buttonTransform.GetComponentInChildren<Text>().text = reaction.playerLine.line;
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
                            lineChoiceSize/2);
                        buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, 0.5f -
                            lineChoiceSize/2);
                    }
                }
            });
        }
    }

    private void PopulateButtons(HubLine pLine)
    {
        m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");

        ClearChoices();

        var textPlacement = 1f;
        foreach (var reaction in pLine.choicesList)
        {
            var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;
            if (buttonGameObject == null) continue;
            var buttonTransform = buttonGameObject.GetComponent<RectTransform>();
            buttonGameObject.transform.SetParent(m_DialogueScreen.transform);

            textPlacement -= lineChoiceSpacing;
            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, textPlacement);
            textPlacement -= lineChoiceSize;
            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, textPlacement);

            buttonTransform.offsetMax = Vector2.zero;
            buttonTransform.offsetMin = Vector2.zero;

            buttonTransform.GetComponentInChildren<Text>().text = reaction.playerLine.line;

            var buttonComponet = buttonGameObject.GetComponent<Button>();
            var reaction1 = reaction;
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

                if (pLine.choicesList.IndexOf(reaction1) == 0)
                {
                    m_Repeat = false;
                }
            });
        }
    }

    private void ClearChoices()
    {
        foreach (Transform t in m_DialogueScreen.transform)
        {
            Destroy(t.gameObject);
        }
    }
}