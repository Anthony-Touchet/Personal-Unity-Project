using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMechanics
{
    [RequireComponent(typeof(BoxCollider))]
    public class NextLevelTrigger : MonoBehaviour
    {
        public int nextSceneIndex;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
