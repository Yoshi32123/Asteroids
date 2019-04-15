using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // bullet sprite
    // https://opengameart.org/content/lasers-and-beams

    // bullet stats
    private Vector3 bulletPosition;
    private Vector3 bulletDirection;
    private Vector3 bulletVelocity;
    public bool destroyMe;

    // upgraded?
    public bool screenwrap = false;
    private float timer = 0f;

    // camera
    private new Camera camera;
    private float totalCamHeight;
    private float totalCamWidth;
    
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        // bullet initialization
        bulletDirection = new Vector3(0, 1);
        bulletDirection = transform.rotation * bulletDirection;
        bulletPosition = transform.position;
        destroyMe = false;

        // camera initialization
        camera = Camera.main;
        totalCamHeight = camera.orthographicSize;
        totalCamWidth = totalCamHeight * camera.aspect;
    }
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        transform.position = bulletPosition;
        Move();

        // UPGRADE
        if (screenwrap)
        {
            ScreenWrap();
        }
    }

    /// <summary>
    /// Moves the bullet
    /// </summary>
    public void Move()
    {
        // setting bullet velocity
        bulletVelocity = bulletDirection * 0.5f;

        // moving sprite position
        bulletPosition += bulletVelocity;
    }

    /// <summary>
    /// If position reaches edge of camera, set position to other side of camera.
    /// </summary>
    public void ScreenWrap()
    {
        // y values
        if (Mathf.Abs(bulletPosition.y) > totalCamHeight)
        {
            bulletPosition.y = -bulletPosition.y;
        }

        // x values
        if (Mathf.Abs(bulletPosition.x) > totalCamWidth)
        {
            bulletPosition.x = -bulletPosition.x;
        }

        // incrementing timer
        timer += 1.0f * Time.deltaTime;
        
        // stopping/deleting bullet
        if (timer >= 0.58)
        {
            destroyMe = true;
        }
    }
}
