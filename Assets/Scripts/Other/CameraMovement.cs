using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform movementTransform;
    public float speed;
    public float scrollSpeed = 1;
	
	// Update is called once per frame
	private void Update ()
    {
		var movement = new Vector3();

        // Get Regular movement
        movement += movementTransform.forward * Input.GetAxisRaw("Vertical");
        movement += movementTransform.right * Input.GetAxisRaw("Horizontal");

        // Get Scroll Movement and scale it
        var mouseWheel = movementTransform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed *
            Time.deltaTime * 10;

        // Scale Movement and add Mouse scroll
	    transform.position += (movement.normalized*speed*Time.deltaTime) + mouseWheel;
	}
}
