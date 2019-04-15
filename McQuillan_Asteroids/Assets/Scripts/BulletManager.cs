using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // manager
    public GameObject manager;

    // bullets
    public List<GameObject> bullets;
    public GameObject bulletPrefab;

    // upgrades
    public bool[] bulletModifier = { false, false };

    // camera
    private new Camera camera;
    private float totalCamHeight;
    private float totalCamWidth;

    // delta time tracker
    private float timer;
    private bool fireReady = false;
    public bool hittable = true;

    // Use this for initialization
    void Start ()
    {
        // initialize bullet list
        bullets = new List<GameObject>();

        // camera initialization
        camera = Camera.main;
        totalCamHeight = camera.orthographicSize;
        totalCamWidth = totalCamHeight * camera.aspect;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Firing();

        if (!bulletModifier[1])
        {
            CheckBullets();
        }

        // UPGRADE FOR SCREENWRAP
        if (bulletModifier[1])
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].GetComponent<Bullet>().screenwrap = true;
            }
        }

        DestroyBullets();
    }

    /// <summary>
    /// Checks if the bullet is off screen
    /// </summary>
    public void CheckBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            // checks x and y values
            if ((Mathf.Abs(bullets[i].transform.position.y) > totalCamHeight ||
                Mathf.Abs(bullets[i].transform.position.x) > totalCamWidth &&
                !bulletModifier[1]) ||
                bullets[i].GetComponent<Bullet>().destroyMe)
            {
                bullets[i].GetComponent<Bullet>().destroyMe = true;
            }
        }
    }
    
    /// <summary>
    /// Adds bullets to list and prevents rapid fire
    /// </summary>
    public void Firing()
    {
        // makes a new bullet and adds it to the list
        if (Input.GetKey(KeyCode.Space) && fireReady == true && hittable)
        {
            bullets.Add(Instantiate(bulletPrefab, transform.position, transform.rotation));
            fireReady = false;
            timer = 0f;
        }

        // locking firing until timer is ready
        if (fireReady == false)
        {
            // updating timer
            timer += 1.0f + Time.deltaTime;

            // checking timer
            if (timer >= 25f)
            {
                fireReady = true;
            }
        }
    }

    /// <summary>
    /// Destroys bullets
    /// </summary>
    public void DestroyBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].GetComponent<Bullet>().destroyMe)
            {
                Destroy(bullets[i]);
                bullets.RemoveAt(i);
                i--;
            }
        }
    }
}
