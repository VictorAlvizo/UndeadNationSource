using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 4f;

    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();

        Vector2 direction = GameObject.Find("Player").GetComponent<Player>().bulletDirection;

        rb.velocity = direction * speed;

        transform.up = direction;
	}
	
	void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
