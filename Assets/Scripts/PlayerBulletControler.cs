using UnityEngine;
using System.Collections;

public class PlayerBulletControler : MonoBehaviour {

    public int damage;      //How much health will be lost on hitting an actor.
    Rigidbody bullRB;

    public enum BulletState
    {
        LIVE,
        DEAD,
    }
    public BulletState currentState;

    void Start()
    {
        bullRB = gameObject.GetComponent<Rigidbody>();
        bullRB.AddForce(10, 0, 0);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Terrain")
        {

        }
    }
}
