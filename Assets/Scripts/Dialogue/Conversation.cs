using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Conversation : MonoBehaviour
    {
        protected AudioSource mAudioSource;      // Source for audio
        protected Line mCurrentLine;             // Current line the System is on
        protected float mTimer;                  // Timer for waiting

        protected bool mDone;                    // Is the conversation Over?
        protected bool mChoiceWaiting;           // Are we waiting for the player to make a choice?
        protected bool mRepeat;                  // Are we repeating the current Line?
        protected bool mButtonClicked;           // Should we ignore the last button press?

        protected GameObject mDialogueScreen;                // Screen we are putting options on
        protected List<Line> mAllLines = new List<Line>();   // All Line collectively

        protected IEnumerator mCorutineEnumerator;   // Corutine to control Dialogue

        public float lineChoiceSpacing;             // Dialogue Choices spacing
        [Range(0, 1)] public float lineChoiceSize;  // Controls how large the text will come out
        public float waitAfterLine;                 // How long to wait after each line
        [Space]
        public Text lineText;           // The Text that will display what the line is saying.

        [Space]
        public List<Line> conversationLines;        // All conversation lines

        // Setting up all of the Variables needed
        protected virtual void Awake()
        {
            // Put all the info together
            mAllLines.AddRange(conversationLines);

            mDialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
            mTimer = waitAfterLine;
            mAudioSource = GetComponent<AudioSource>();

            // Start Conversation
            RestartDialogue();
            mCorutineEnumerator = DialogueCoRutine();
        }

        // Unity Update, Primarily used to keep the Dialogue going
        protected virtual void Update()
        {
            // If the player wants to skip the dialogue, Click right mouse button and 
            // make sure we aren't clicking anything else.
            if (Input.GetKeyUp(KeyCode.Mouse0) && !mButtonClicked)
            {
                mTimer = 0;
                mAudioSource.Stop();
            }
            // If a button was just pressed, ignore above and reset
            else if (mButtonClicked)
            {
                mButtonClicked = false;
            }

            // Continue Iteration if we are not waiting to choose a choice
            if (mCorutineEnumerator != null && !mChoiceWaiting)
                mCorutineEnumerator.MoveNext();
        }

        // Dialogue Corutine. Used as a corutine so it can easily be stopped and restarted
        protected IEnumerator DialogueCoRutine()
        {
            while (!mDone)
            {
                //If the Audio is playing, Don't worry
                while (mAudioSource.isPlaying)
                    yield return null;

                // Execute Action after the line is done
                if (mCurrentLine.GetType() == typeof(ActionLine))
                {
                    var aL = (ActionLine)mCurrentLine;
                    aL.Execute(FindObjectOfType<Camera>().gameObject);
                }

                // If it is not playing, count down
                while (mTimer > 0)
                {
                    mTimer -= Time.deltaTime;
                    yield return null;
                }

                // After countdown, reset timer
                mTimer = waitAfterLine;
            
                // If we repeat and are a hub line, repeat
                if (mRepeat)
                {
                    PlayLine(mCurrentLine);
                    PopulateButtons((HubLine)mCurrentLine);
                    mChoiceWaiting = true;
                    mRepeat = true;
                    continue;
                }

                // Try to get the next line
                for (var i = 0; i < mAllLines.Count; i++)
                {
                    // Find current line and make sure it is in the list
                    if (mAllLines[i] == mCurrentLine && i + 1 < mAllLines.Count)
                    {
                        // Set current line and break out
                        mCurrentLine = mAllLines[i + 1];
                        break;
                    }

                    // If not at the end, keep looping
                    if (i + 1 < mAllLines.Count) continue;

                    // Else end Dialogue
                    EndDialogue();
                    break;
                }

                // If done, stop looping
                if (mDone)
                    break;

                // If the next line is a line, play it
                if (mCurrentLine.GetType() == typeof(Line) || mCurrentLine.GetType() == typeof(ActionLine))
                {
                    PlayLine(mCurrentLine);
                }
                // else if it is branching, populate buttons and waiting for choice
                else if (mCurrentLine.GetType() == typeof(BranchingLine))
                {
                    PlayLine(mCurrentLine);
                    PopulateButtons((BranchingLine)mCurrentLine);
                    mChoiceWaiting = true;
                }
                // else if it is a hub, populate buttons, waiting for choice, repeat
                else if (mCurrentLine.GetType() == typeof(HubLine))
                {
                    PlayLine(mCurrentLine);
                    PopulateButtons((HubLine)mCurrentLine);
                    mChoiceWaiting = true;
                    mRepeat = true;
                }
            }
            yield return null;
        }

        // Stop the Dialogue System
        protected void EndDialogue()
        {
            mCurrentLine = null;       // Clear current Line
            mAudioSource.clip = null;  // Clear audio source
            mDone = true;              // Done
            lineText.text = "";         // Clear text

            // If playing audio, stop
            if(mAudioSource.isPlaying)
                mAudioSource.Stop();

            // If we have children on the screen, clear
            if (mDialogueScreen.transform.childCount <= 0) return;

            ClearChoices();
        }

        // Set Audio, words, and face
        protected virtual void PlayLine(Line line)
        {
            lineText.text = line.line;                  // Set Line Text
            mAudioSource.clip = line.sourceClip;       // Set the audio
            mAudioSource.Play();                       // Play Audio
        }

        // Populate buttons based off a Branching Line
        protected void PopulateButtons(BranchingLine pLine)
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
                buttonGameObject.transform.SetParent(mDialogueScreen.transform);       // Set to screen

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
                    mChoiceWaiting = false;            // No longer waiting for choice
                    mButtonClicked = true;             // A button has been pressed

                    // Run through all choices to see Buttons
                    foreach (Transform go in mDialogueScreen.transform)
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
        protected void PopulateButtons(HubLine pLine)
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
                buttonGameObject.transform.SetParent(mDialogueScreen.transform);       // Set to screen

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
                    mChoiceWaiting = false;            // No longer waiting for choice
                    mButtonClicked = true;             // A button has been pressed

                    // Run through all choices to see Buttons
                    foreach (Transform go in mDialogueScreen.transform)
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
                        mRepeat = false;
                    }
                });
            }
        }

        // Destroy all children of the Dialogue screen
        protected void ClearChoices()
        {
            foreach (Transform t in mDialogueScreen.transform)
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

            mCorutineEnumerator = null;    // Clear enumeration
            mCurrentLine = mAllLines[0];  // Set line to first in list
            mDone = false;                 // Not Done
            mChoiceWaiting = false;        // Not waiting
            mRepeat = false;               // Not repeating

            mCorutineEnumerator = DialogueCoRutine();  // Restart Corutine
            PlayLine(mCurrentLine);                    // Play first line

            mButtonClicked = true;                     // Had to click a button to activate restart
        }
    }
}