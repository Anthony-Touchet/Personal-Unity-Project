using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    public int damage;      //How much health will be lost on hitting an actor.
    float diesOn;           //When the bullet will die.
    float liveTime = 1f;    //Amount of time the bullet lasts.
    float currentTime;      //The current time 
    
	public enum BulletState
    {
        LIVE,
        DEAD,
    }
    public BulletState currentState;

	void Start () {
        currentTime = Time.time;
        diesOn = Time.time + liveTime;
	}
	
	void FixedUpdate () {
        currentTime += Time.deltaTime;
        if(currentTime >= diesOn)
        {
            currentState = BulletState.DEAD;
        }

        if(currentState == BulletState.DEAD)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        
    }
}
