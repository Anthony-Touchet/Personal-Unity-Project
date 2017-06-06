using UnityEngine;

namespace Other
{
    public class DestroyOnTimer : MonoBehaviour
    {
        public float time;

        // Use this for initialization
        private void Update ()
        {
            if(time < 0)
                Destroy(gameObject);
            time -= Time.deltaTime;
        }	
	
    }
}
