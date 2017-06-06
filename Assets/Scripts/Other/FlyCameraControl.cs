using UnityEngine;

namespace Other
{
    public class FlyCameraControl : MonoBehaviour
    {
        private Transform m_CameraTransform;
        public float rotationSpeed;
        public float movementSpeed;
        public float scrollSpeed = 1;

        private void Awake ()
        {
            m_CameraTransform = transform.GetComponentInChildren<Camera>().transform;   // Get Camera Transform
        }

        private void Update ()
        {
            var movement = new Vector3();

            // Get Regular movement
            movement += m_CameraTransform.forward * Input.GetAxisRaw("Vertical");
            movement += m_CameraTransform.right * Input.GetAxisRaw("Horizontal");

            // Get Scroll Movement and scale it
            var mouseWheel = m_CameraTransform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed *
                             Time.deltaTime * 10;

            // Scale Movement and add Mouse scroll
            transform.position += (movement.normalized * movementSpeed * Time.deltaTime) + mouseWheel;

            // If button not clicked, Don't Rotate
            if (Input.GetAxis("Fire2") <= 0)
                return;

            var horizontal = Input.GetAxis("Mouse X");  // how much is mouse moving on the X
            var vertical = Input.GetAxis("Mouse Y");    // how much is mouse moving on the Y

            // Rotate this transform on it's Y based off X mouse movement
            transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);

            // Rotate Camera transform on it's X based off Y mouse movement
            m_CameraTransform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);
        }
    }
}
