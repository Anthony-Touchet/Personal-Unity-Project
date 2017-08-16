using UnityEngine;

namespace Genaric
{
    public class DoNotDestroyOnLoad : MonoBehaviour {

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
