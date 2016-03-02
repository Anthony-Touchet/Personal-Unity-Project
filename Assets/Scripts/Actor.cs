using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    protected float speed;
    protected int health;
    protected int bullDam;



    // Use this for initialization
    protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    if(this.health <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    protected virtual void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            health -= bullDam;
        }
    }
}
