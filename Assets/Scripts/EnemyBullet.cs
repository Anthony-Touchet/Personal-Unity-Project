using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    public int damage;      //How much health will be lost on hitting an actor.
    float diesOn;           //When the bullet will die.
    float liveTime = 1f;    //Amount of time the bullet lasts.
    float currentTime;      //The current time 
    Rigidbody bullRB;

	void Start () {
        bullRB = gameObject.GetComponent<Rigidbody>();  //Get bullet's rigid body.
        currentTime = Time.time;                        //Get the time when the bullet is created
        diesOn = Time.time + liveTime;                  //When the bullet needs to be destroyed.
	}
	
	void FixedUpdate () {
        currentTime += Time.deltaTime;  //Add passing time to current
        if(currentTime >= diesOn)       //if current time is greater or equal to death time 
        {
            Destroy(gameObject);    //Bullet is Destroyed
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")  //If the bullet colides with the terain
        {
            Destroy(gameObject);    //Destroy Bullet
        }
    }
}
