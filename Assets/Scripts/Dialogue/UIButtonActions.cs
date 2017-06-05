using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Dialogue
{
    // ReSharper disable once InconsistentNaming
    public class UIButtonActions : MonoBehaviour
    {

        // Used to restart the scene and revert any actions taken
        [ContextMenu("Scene Restart")]
        public void RestartScene()
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            var assembly = Assembly.GetExecutingAssembly();

            var typelist = assembly.GetTypes().Where(t => string.Equals(t.Namespace, "Other",
                StringComparison.Ordinal)).ToList();

            var componets = mainCamera.GetComponents<Component>();
            foreach (var c in componets)
            {
                if(typelist.Contains(c.GetType()))
                    Destroy(c);
            }

            mainCamera.transform.rotation = Quaternion.identity;

            RestartDialogue();
        }

        public void RestartDialogue()
        {
            var convo2D = FindObjectOfType<Conversation2DBehavior>();
            var convo3D = FindObjectOfType<Conversation3DBehavior>();

            if (convo3D != null)
            {
                convo3D.RestartDialogue();
            }

            if (convo2D != null)
            {
                convo2D.RestartDialogue();
            }
        }
    }
}
