using UnityEngine;
using System.Collections;

public class PlayerController : Actor {

    public GameObject bullet;   //Player Bullet Prefab
    public int ammo;           //THe amount of bullets the player has.

    void Lerp(float a, float b, float c)
    {
        gameObject.transform.position = new Vector3(transform.position.x + a, transform.position.y + b, transform.position.z + c);
    }

    void Start () {
        health = 100;
        speed = 5;
        bullDam = 10;
        fireRate = 1f;
        ammo = 3;
	}	

    public void FixedUpdate()
    {
        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        Lerp(x / speed, 0f, z / speed);
    }

	override public void Update () {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space) && ammo > 0)
        { GameObject temp;
            temp = Instantiate(bullet, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), new Quaternion()) as GameObject;
            ammo -= 1;
            temp.GetComponent<PlayerBulletControler>().damage = bullDam;
            
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            health -= GetComponent<EnemyBullet>().damage;
        }

        if (other.gameObject.tag == "PlayerBullet")
        {
            ammo += 1;
            Destroy(other.gameObject);
        }
    }
}
