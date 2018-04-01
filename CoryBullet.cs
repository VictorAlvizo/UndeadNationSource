using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoryBullet : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir = GameObject.Find("Cory(Clone)").GetComponent<CoryAI>().bulletDirection;

        transform.up = dir;

        rb.velocity = dir * speed;
	}
	
	void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
