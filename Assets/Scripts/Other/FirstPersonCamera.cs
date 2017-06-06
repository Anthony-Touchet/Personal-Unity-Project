﻿using UnityEngine;

namespace Other
{
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonCamera : MonoBehaviour
    {
        private Transform m_CameraTransform;
        private Rigidbody m_Rigidbody;
        public float rotationSpeed;
        public float movementSpeed;

        private void Awake()
        {
            // Get camera in child
            m_CameraTransform = transform.GetComponentInChildren<Camera>().transform;

            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var movement = new Vector3();

            // Get Regular movement
            movement += transform.forward * Input.GetAxisRaw("Vertical");
            movement += transform.right * Input.GetAxisRaw("Horizontal");

            // Scale Movement and add Mouse scroll
            m_Rigidbody.position += (movement.normalized * movementSpeed * Time.deltaTime);

            var horizontal = Input.GetAxis("Mouse X");  // how much is mouse moving on the X
            var vertical = Input.GetAxis("Mouse Y");    // how much is mouse moving on the Y

            // Rotate this transform on it's Y based off X mouse movement
            transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);

            // Rotate Camera transform on it's X based off Y mouse movement
            m_CameraTransform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);

            // Control rotation so that the model doesn't get displaced
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }
}
