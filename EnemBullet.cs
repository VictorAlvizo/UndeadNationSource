using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemBullet : MonoBehaviour {

    public float speed = 8f;
	
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
