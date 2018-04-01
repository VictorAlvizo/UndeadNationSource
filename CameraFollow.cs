using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraFollow : MonoBehaviour {

    public Transform cameraTarget;

    public float speed = 2.5f;

    public float minX;
    public float maxX = 100;
    public float minY;
    public float maxY = 100;

	void FixedUpdate()
    {
        if(cameraTarget != null)
        {
            var newPos = Vector2.Lerp(transform.position, cameraTarget.position, Time.deltaTime * speed);

            var vect3 = new Vector3(newPos.x, newPos.y, -10f);

            var clampX = Mathf.Clamp(vect3.x, minX, maxX);
            var clampY = Mathf.Clamp(vect3.y, minY, maxY);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
