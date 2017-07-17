using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBehavior : MonoBehaviour
{ 
    private Vector3 m_OriginalPosition;
    private Transform m_PlayerTransform;
    private Rigidbody m_PlayerRigidbody;

    public float respawnLimit;

	private void Awake()
	{
	    var player = FindObjectOfType<PlayerMovement2D>();
        m_OriginalPosition = player.transform.position;
	    m_PlayerTransform = player.transform;
	    m_PlayerRigidbody = player.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (!(m_PlayerTransform.position.y <= respawnLimit)) return;

        m_PlayerTransform.position = m_OriginalPosition;
        m_PlayerRigidbody.velocity += -m_PlayerRigidbody.velocity;
    }
}
