using System.Collections;
using System.Collections.Generic;
using Genaric;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement2D : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private bool m_CanJump = true;

    public float movementSpeed;
    public float jumpPower;

    public KeyCode leftControl;
    public KeyCode rightControl;
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
        if (Input.GetKey(rightControl))
            movement += transform.right;
        if (Input.GetKey(leftControl))
            movement -= transform.right;

        m_Rigidbody.AddForce(movement.normalized * movementSpeed);

        if (!Input.GetKeyDown(jumpControl) || !m_CanJump) return;

        m_Rigidbody.AddForce(Vector3.up * jumpPower * 10, ForceMode.Impulse);
        m_CanJump = false;
        AudioManager.self.PlaySound("Jump");
    }

    private void OnCollisionEnter(Collision other)
    {
        // TODO: Check to see if gameObject is on top of another.
        m_CanJump = true;
    }
}
