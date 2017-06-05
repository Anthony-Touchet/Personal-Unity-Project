using UnityEngine;

namespace Other
{
    public class MoveGameObject : MonoBehaviour
    {
        private Vector3 disPerSecond;
        private float timePassed;

        public Vector3 distance;
        public float travelTime;

        private void Awake ()
        {
            disPerSecond = distance/travelTime;
        }
	
        // Update is called once per frame
        private void Update ()
        {
            if (timePassed >= travelTime)
            {
                Destroy(this);
                return;
            }

            transform.position += disPerSecond*Time.deltaTime;
            timePassed += Time.deltaTime;
        }
    }
}
