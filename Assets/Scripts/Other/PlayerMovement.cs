using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private bool m_CanJump = true;

    public float movementSpeed;
    public float jumpPower;

    public KeyCode forwardControl;
    public KeyCode backControl;
    public KeyCode leftStraifeControl;
    public KeyCode rightStraifeControl;
    public KeyCode jumpControl;

    private void Awake ()
	{
	    m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        var movement = new Vector3();

        // Get Regular movement
        if (Input.GetKey(forwardControl))
            movement += transform.forward;
        if (Input.GetKey(backControl))
            movement -= transform.forward;
        if (Input.GetKey(rightStraifeControl))
            movement += transform.right;
        if (Input.GetKey(leftStraifeControl))
            movement -= transform.right;

        // Scale Movement and add Mouse scroll
        m_Rigidbody.position += (movement.normalized * movementSpeed * Time.deltaTime);

        if (!Input.GetKeyDown(jumpControl) || !m_CanJump) return;

        m_Rigidbody.AddForce(Vector3.up * jumpPower * 10, ForceMode.Impulse);
        m_CanJump = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        // TODO: Check to see if gameObject is on top of another.
        m_CanJump = true;
    }
}
