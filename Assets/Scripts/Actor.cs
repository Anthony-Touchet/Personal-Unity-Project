using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    protected float speed;
    protected int health;
    protected int bullDam;
    protected float fireRate;

	public virtual void Update () {
	    if(this.health <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
