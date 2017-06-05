using UnityEngine;

namespace Dialogue
{
    public class Conversation3DBehavior : Conversation
    {
        private Transform m_PlayerTransform;    // player transform for 3D

        public float range;     // Range for the 3D listening range
        
        protected override void Awake ()
        {
            // Put all the info together
            mAllLines.AddRange(conversationLines);

            m_PlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            mDialogueScreen = GameObject.FindGameObjectWithTag("DialogueCanvas");
            mTimer = waitAfterLine;
            mAudioSource = GetComponent<AudioSource>();
        }
	
        protected override void Update ()
        {
            // End conversation if out of range
            if ((m_PlayerTransform.position - transform.position).magnitude > range &&
                mCorutineEnumerator != null)
            {
                mCorutineEnumerator = null;
                EndDialogue();
            }

            // Resart conversation if in range
            else if ((m_PlayerTransform.position - transform.position).magnitude <= range &&
                     mCorutineEnumerator == null)
            {
                RestartDialogue();
                mCorutineEnumerator = DialogueCoRutine();
            }

            // Call the rest of Update
            base.Update();
        }
    }
}
