using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform m_CameraTransform;
    public float speed;

	private void Awake ()
	{
	    m_CameraTransform = transform.GetComponentInChildren<Camera>().transform;   // Get Camera Transform
	}

    private void Update ()
    {
        // If button not clicked, Don't Rotate
        if (Input.GetAxis("Fire2") <= 0)
            return;

        var horizontal = Input.GetAxis("Mouse X");  // how much is mouse moving on the X
        var vertical = Input.GetAxis("Mouse Y");    // how much is mouse moving on the Y

        // Rotate this transform on it's Y based off X mouse movement
        transform.Rotate(Vector3.up, horizontal * speed * Time.deltaTime);

        // Rotate Camera transform on it's X based off Y mouse movement
        m_CameraTransform.Rotate(Vector3.right, -vertical * speed * Time.deltaTime);
    }
}
