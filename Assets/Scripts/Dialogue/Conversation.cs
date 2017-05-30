using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dialogue
{
    [RequireComponent(typeof(AudioSource))]
    public class Conversation : MonoBehaviour
    {
        private AudioSource m_AudioSource;      // Source for audio
        private Line m_CurrentLine;             // Current line the System is on
        private float m_Timer;                  // Timer for waiting

        private bool m_Done;                    // Is the conversation Over?
        private bool m_ChoiceWaiting;           // Are we waiting for the player to make a choice?
        private bool m_Repeat;                  // Are we repeating the current Line?
        private bool m_ButtonClicked;           // Should we ignore the last button press?

        private GameObject m_DialogueScreen;                // Screen we are putting options on
        private List<Line> m_AllLines = new List<Line>();   // All Line collectively

        private IEnumerator m_CorutineEnumerator;   // Corutine to control Dialogue

        private Transform m_PlayerTransform;    // player transform for 3D

        public bool is3D;       // Will this be used in 3D
        public float range;     // Range for the 3D listening range
        [Space]
        public float lineChoiceSpacing;             // Dialogue Choices spacing
        [Range(0, 1)] public float lineChoiceSize;  // Controls how large the text will come out
        public float waitAfterLine;                 // How long to wait after each line
        [Space]
        public Text lineText;           // The Text that will display what the line is saying.
        public Image faceExpression;    // Image that will display the face of the character

        [Space]
        public List<Line> conversationLines;        // All conversation lines

        // Setting up all of the Variables needed
        private void Awake()
        {
            // Put all the info together
            m_AllLines.AddRange(conversationLines);

            m_PlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            m_DialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
            m_Timer = waitAfterLine;
            m_AudioSource = GetComponent<AudioSource>();

            // If 3D, Stop here.
            if (is3D) return;

            // Else, start Conversation
            RestartDialogue();
            m_CorutineEnumerator = DialogueCoRutine();
        }

        // Unity Update, Primarily used to keep the Dialogue going
        private void Update()
        {
            // 3D Dialogue
            if (is3D)
            {
                // End conversation if out of range
                if ((m_PlayerTransform.position - transform.position).magnitude > range && 
                    m_CorutineEnumerator != null)
                {
                    m_CorutineEnumerator = null;
                    EndDialogue();
                }

                // Resart conversation if in range
                else if ((m_PlayerTransform.position - transform.position).magnitude <= range &&
                         m_CorutineEnumerator == null)
                {
                    RestartDialogue();
                    m_CorutineEnumerator = DialogueCoRutine();
                }
            }

            // If the player wants to skip the dialogue, Click right mouse button and 
            // make sure we aren't clicking anything else.
            if (Input.GetKeyUp(KeyCode.Mouse0) && !m_ButtonClicked)
            {
                m_Timer = 0;
                m_AudioSource.Stop();
            }
            // If a button was just pressed, ignore above and reset
            else if (m_ButtonClicked)
            {
                m_ButtonClicked = false;
            }

            // Continue Iteration if we are not waiting to choose a choice
            if (m_CorutineEnumerator != null && !m_ChoiceWaiting)
                m_CorutineEnumerator.MoveNext();
        }

        // Dialogue Corutine. Used as a corutine so it can easily be stopped and restarted
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

                // Try to get the next line
                for (var i = 0; i < m_AllLines.Count; i++)
                {
                    // Find current line and make sure it is in the list
                    if (m_AllLines[i] == m_CurrentLine && i + 1 < m_AllLines.Count)
                    {
                        // Set current line and break out
                        m_CurrentLine = m_AllLines[i + 1];
                        break;
                    }

                    // If not at the end, keep looping
                    if (i + 1 < m_AllLines.Count) continue;

                    // Else end Dialogue
                    EndDialogue();
                    break;
                }

                // If done, stop looping
                if (m_Done)
                    break;

                // If the next line is a line, play it
                if (m_CurrentLine.GetType() == typeof(Line))
                {
                    PlayLine(m_CurrentLine);
                }
                else if (m_CurrentLine.GetType() == typeof(ActionLine))
                {
                    PlayLine(m_CurrentLine);
                    var aL = (ActionLine) m_CurrentLine;
                    aL.Execute(FindObjectOfType<Camera>().gameObject);
                }
                // else if it is branching, populate buttons and waiting for choice
                else if (m_CurrentLine.GetType() == typeof(BranchingLine))
                {
                    PlayLine(m_CurrentLine);
                    PopulateButtons((BranchingLine)m_CurrentLine);
                    m_ChoiceWaiting = true;
                }
                // else if it is a hub, populate buttons, waiting for choice, repeat
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

        // Stop the Dialogue System
        private void EndDialogue()
        {
            m_CurrentLine = null;       // Clear current Line
            m_AudioSource.clip = null;  // Clear audio source
            m_Done = true;              // Done
            lineText.text = "";         // Clear text

            // If playing audio, stop
            if(m_AudioSource.isPlaying)
                m_AudioSource.Stop();

            // If we have children on the screen, clear
            if (m_DialogueScreen.transform.childCount <= 0) return;

            ClearChoices();
        }

        // Set Audio, words, and face
        private void PlayLine(Line line)
        {
            lineText.text = line.line;                  // Set Line Text
            m_AudioSource.clip = line.sourceClip;       // Set the audio
            m_AudioSource.Play();                       // Play Audio
            faceExpression.sprite = line.expression;    // Set face expresion
        }

        // Populate buttons based off a Branching Line
        private void PopulateButtons(BranchingLine pLine)
        {
            // Clear choices to make clean
            ClearChoices();

            // Start from the top of the screen
            var textPlacement = 1f;
            foreach (var reaction in pLine.reactions)
            {   
                // Make choice gameObject
                var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;    

                // Null check
                if (buttonGameObject == null) continue;

                var buttonTransform = buttonGameObject.GetComponent<RectTransform>();   // Get Transform
                buttonGameObject.transform.SetParent(m_DialogueScreen.transform);       // Set to screen

                // Placing text anchors
                textPlacement -= lineChoiceSpacing;
                buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, textPlacement);
                textPlacement -= lineChoiceSize;
                buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, textPlacement);

                // Zero out offsets
                buttonTransform.offsetMax = Vector2.zero;
                buttonTransform.offsetMin = Vector2.zero;

                // Set text
                buttonTransform.GetComponentInChildren<Text>().text = reaction.playerLine.line;

                // Setting up button Functionality
                var reaction1 = reaction;
                var buttonComponet = buttonGameObject.GetComponent<Button>();
                buttonComponet.onClick.AddListener(() =>
                {
                    PlayLine(reaction1.reactionLine);   // Play Line
                    m_ChoiceWaiting = false;            // No longer waiting for choice
                    m_ButtonClicked = true;             // A button has been pressed

                    // Run through all choices to see Buttons
                    foreach (Transform go in m_DialogueScreen.transform)
                    {
                        // If not the button clicked, Destroy
                        if (go != buttonTransform)
                            Destroy(go.gameObject);
                        else
                        {
                            // Else, render button to only text
                            buttonComponet.onClick.RemoveAllListeners();
                            buttonComponet.interactable = false;

                            // Center Choice
                            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, 0.5f +
                                lineChoiceSize/2);
                            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, 0.5f -
                                lineChoiceSize/2);
                        }
                    }
                });
            }
        }

        // Populate buttons based off a Hub Line
        private void PopulateButtons(HubLine pLine)
        {
            // Clear choices to make clean
            ClearChoices();

            // Start from the top of the screen
            var textPlacement = 1f;
            foreach (var reaction in pLine.choicesList)
            {
                // Make choice gameObject
                var buttonGameObject = Instantiate(Resources.Load("LineButton")) as GameObject;

                // Null check
                if (buttonGameObject == null) continue;

                var buttonTransform = buttonGameObject.GetComponent<RectTransform>();   // Get Transform
                buttonGameObject.transform.SetParent(m_DialogueScreen.transform);       // Set to screen

                // Placing text anchors
                textPlacement -= lineChoiceSpacing;
                buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, textPlacement);
                textPlacement -= lineChoiceSize;
                buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, textPlacement);

                // Zero out offsets
                buttonTransform.offsetMax = Vector2.zero;
                buttonTransform.offsetMin = Vector2.zero;

                // Set text
                buttonTransform.GetComponentInChildren<Text>().text = reaction.playerLine.line;

                // Setting up button Functionality
                var buttonComponet = buttonGameObject.GetComponent<Button>();
                var reaction1 = reaction;
                buttonComponet.onClick.AddListener(() =>
                {
                    PlayLine(reaction1.reactionLine);   // Play Line
                    m_ChoiceWaiting = false;            // No longer waiting for choice
                    m_ButtonClicked = true;             // A button has been pressed

                    // Run through all choices to see Buttons
                    foreach (Transform go in m_DialogueScreen.transform)
                    {
                        // If not the button clicked, Destroy
                        if (go != buttonTransform)
                            Destroy(go.gameObject);
                        else
                        {
                            // Else, render button to only text
                            buttonComponet.onClick.RemoveAllListeners();
                            buttonComponet.interactable = false;

                            // Center Choice
                            buttonTransform.anchorMax = new Vector2(buttonTransform.anchorMax.x, 0.5f +
                                lineChoiceSize / 2);
                            buttonTransform.anchorMin = new Vector2(buttonTransform.anchorMin.x, 0.5f -
                                lineChoiceSize / 2);
                        }
                    }

                    // If Button is first, stop repeating
                    if (pLine.choicesList.IndexOf(reaction1) == 0)
                    {
                        m_Repeat = false;
                    }
                });
            }
        }

        // Destroy all children of the Dialogue screen
        private void ClearChoices()
        {
            foreach (Transform t in m_DialogueScreen.transform)
            {
                Destroy(t.gameObject);
            }
        }

        // Used to restart the System
        [ContextMenu("Restart Dialogue")]
        public void RestartDialogue()
        {
            // End any current dialog, just to be safe
            EndDialogue();

            m_CorutineEnumerator = null;    // Clear enumeration
            m_CurrentLine = m_AllLines[0];  // Set line to first in list
            m_Done = false;                 // Not Done
            m_ChoiceWaiting = false;        // Not waiting
            m_Repeat = false;               // Not repeating

            m_CorutineEnumerator = DialogueCoRutine();  // Restart Corutine
            PlayLine(m_CurrentLine);                    // Play first line

            m_ButtonClicked = true;                     // Had to click a button to activate restart
        }

        // Used to restart the scene and revert any actions taken
        [ContextMenu("Scene Restart")]
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}