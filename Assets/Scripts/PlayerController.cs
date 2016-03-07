using UnityEngine;
using System.Collections;

public class PlayerController : Actor {

    public GameObject bullet;   //Player Bullet Prefab
    private int ammo;           //THe amount of bullets the player has.

	void Start () {
        health = 100;
        speed = 5;
        bullDam = 10;
        fireRate = 1f;
	}	

	override public void Update () {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject temp = (Instantiate(bullet, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), new Quaternion())) as GameObject;
            temp.GetComponent<EnemyBullet>().damage = bullDam;
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            health -= GetComponent<EnemyBullet>().damage;
        }
    }
}
