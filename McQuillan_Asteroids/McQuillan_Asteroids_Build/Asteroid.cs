using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // asteroid sprite
    // https://forum.thegamecreators.com/thread/209786

    // asteroid stats
    private Vector3 asteroidDirection;
    private Vector3 asteroidVelocity;
    private Vector3 asteroidPosition;
    public bool justCreated;

    // camera
    private new Camera camera;
    private float totalCamHeight;
    private float totalCamWidth;

    // delta time tracker
    private float timer;

    // Use this for initialization
    void Start ()
    {
        // setting up direction/position
        asteroidDirection = new Vector3(1, 0);
        asteroidDirection = transform.rotation * asteroidDirection;
        asteroidPosition = transform.position;

        // setting bullet velocity
        asteroidVelocity = asteroidDirection * 0.02f;

        // camera initialization
        camera = Camera.main;
        totalCamHeight = camera.orthographicSize;
        totalCamWidth = totalCamHeight * camera.aspect;
        justCreated = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = asteroidPosition;
        Move();
        ScreenWrap();

        if (justCreated)
        {
            timer += Time.deltaTime;

            if (timer > 0.2f)
            {
                justCreated = false;
            }
        }
	}

    /// <summary>
    /// Moves the asteroid
    /// </summary>
    public void Move()
    {
        // moving sprite position
        asteroidPosition += asteroidVelocity;
    }

    /// <summary>
    /// If position reaches edge of camera, set position to other side of camera.
    /// </summary>
    public void ScreenWrap()
    {
        // y values
        if (Mathf.Abs(asteroidPosition.y) > totalCamHeight)
        {
            asteroidPosition.y = -asteroidPosition.y;
        }

        // x values
        if (Mathf.Abs(asteroidPosition.x) > totalCamWidth)
        {
            asteroidPosition.x = -asteroidPosition.x;
        }
    }
}
