using UnityEngine;

namespace Other
{
    public class Floating : MonoBehaviour
    {
        private Vector3 min;
        private Vector3 max;
        private float m_DistancePerSec;
        private bool m_Rising = true;

        public float magnitude;

        private void Start()
        { 
            min = transform.position;
            max = new Vector3(min.x, min.y + magnitude, min.z);
            m_DistancePerSec = 5;
        }

        private void Update()
        {
            // Move GameObject
            if (m_Rising)
            {
                transform.position += new Vector3(0, m_DistancePerSec * Time.deltaTime, 0);
            }
            else
            {
                transform.position -= new Vector3(0, m_DistancePerSec * Time.deltaTime, 0);
            }

            // See if need to move in other direction
            if (transform.position.y > max.y && m_Rising)
                m_Rising = false;
            else if (transform.position.y < min.y && !m_Rising)
                m_Rising = true;
        }
    }
}
