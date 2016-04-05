using UnityEngine;
using System.Collections;

public class PlayerBulletControler : MonoBehaviour {

    public int damage;      //How much health will be lost on hitting an actor.
    Rigidbody bullRB;       //the Bullet's RigidBody
    private Vector3 fireAt;
    private GameObject player;

    public int bullSpeed = 500; //Bullet's speed.

    public enum BulletState //Bullet's state
    {
        LIVE,
        DEAD,
    }
    public BulletState currentState;    //Bullet's currrent state.

    void Start()
    {
        player = GameObject.Find("Player");
        bullRB = gameObject.GetComponent<Rigidbody>();  //Get RigidBody
        bullRB.useGravity = false;                      //Turn off the bullet's gravity.

        fireAt = gameObject.transform.position - player.gameObject.transform.position;  //Fire from the player away from the player
        
        bullRB.AddForce(fireAt * bullSpeed);               //Apply a force. 
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Terrain")   //if hit terrain
        {
            currentState = BulletState.DEAD;    //Bullet is dead
            bullRB.useGravity = true;           //Bullet has gravity
        }
    }
}
