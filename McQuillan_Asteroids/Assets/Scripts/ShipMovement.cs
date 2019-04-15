using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    // ship sprites
    // ship https://assetstore.unity.com/packages/2d/textures-materials/2d-spaceships-pack-1-86267

    // --- fields ---
    //public float speed;
    public float rateOfAcceleration;
    public Vector3 vehiclePosition;

    // physics vectors
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;

    // rotation fields
    private float angleOfRotation;
    private Vector3 vecToRotate;
    private Quaternion angle;

    // velocity fields
    public float maxSpeed;

    // UPGRADE FIELDS
    public bool[] speedUpgrade = { false, false };

    // camera
    private new Camera camera;
    private float totalCamHeight;
    private float totalCamWidth;

    // --- end fields ---

    // Use this for initialization
    void Start()
    {
        vehiclePosition = new Vector3(0, 0, 0);
        direction = new Vector3(1, 0, 0);
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, 0, 0);

        camera = Camera.main;
        totalCamHeight = camera.orthographicSize;
        totalCamWidth = totalCamHeight * camera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        SetTransfrom();
        RotationHandler();
        Drive();
        ScreenWrap();

        // UPGRADES
        if (speedUpgrade[1])
        {
            rateOfAcceleration = 0.008f;
            maxSpeed = 0.16f;
        }
        else if (speedUpgrade[0])
        {
            rateOfAcceleration = 0.005f;
            maxSpeed = 0.11f;
        }
    }

    /// <summary>
    /// Changes/Sets the transform component
    /// </summary>
    public void SetTransfrom()
    {
        // rotate vehicle sprite
        transform.Rotate(0, 0, angleOfRotation);

        // changing the position of the sprite
        transform.position = vehiclePosition;
    }

    /// <summary>
    /// Works with changes in velocity
    /// </summary>
    public void Drive()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // get accelerate
            acceleration = rateOfAcceleration * direction;

            // calculating the velocity vector
            velocity += acceleration;
        }
        else
        {
            // decelerate
            velocity = velocity * 0.9f;
        }

        // limit velocity
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // changing vehicle position
        vehiclePosition += velocity;
    }

    /// <summary>
    /// Rotates vehicle sprite based on user inpuc
    /// </summary>
    public void RotationHandler()
    {
        // player can control direction
        // left arrow rotates left
        // right arrow rotates right
        if (Input.GetKey(KeyCode.LeftArrow))        // pressing left
        {
            angleOfRotation = 3f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))  // pressing right
        {
            angleOfRotation = -3f;
        }
        else
        {
            angleOfRotation = 0;
        }

        direction = Quaternion.Euler(0, 0, angleOfRotation) * direction;
    }

    /// <summary>
    /// If position reaches edge of camera, set position to other side of camera.
    /// </summary>
    public void ScreenWrap()
    {
        // y values
        if (Mathf.Abs(vehiclePosition.y) > totalCamHeight)
        {
            vehiclePosition.y = -vehiclePosition.y;
        }

        // x values
        if (Mathf.Abs(vehiclePosition.x) > totalCamWidth)
        {
            vehiclePosition.x = -vehiclePosition.x;
        }
    }
}
