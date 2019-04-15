using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // GameObject prefabs
    public GameObject ship;
    public GameObject manager;
    public List<Sprite> asteroidSprites;
    public List<GameObject> asteroid2;
    public List<GameObject> asteroid3;
    public int asteroidPrefab = 0;
    private List<GameObject> asteroidsJustSpawned;

    // upgrades
    public bool[] ShipDamageDecreaser = { false, false };
    public float damage;
    public bool plasmaBullet;

    // invincibility frame fields
    private bool hittable;
    private float timer;

    // debug
    string text;

    // Use this for initialization
    void Start()
    {
        damage = 34f;
        hittable = true;
        plasmaBullet = false;
        asteroidsJustSpawned = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<GUIManager>().hullIntegrity > 0)
        {
            BulletCollision();
            ShipCollision();

            ship.GetComponent<BulletManager>().hittable = hittable;
        }
    }

    /// <summary>
    /// Detects Asteroid/Bullet collisions
    /// </summary>
    void BulletCollision()
    {
        // looping through bullets
        for (int j = 0; j < ship.GetComponent<BulletManager>().bullets.Count; j++)
        {
            // looping through asteroids for detection
            for (int i = 0; i < manager.GetComponent<AsteroidManager>().asteroids.Count; i++)
            {
                // double checks j is valid for collision detection
                if (j >= 0)
                {
                    // checks for a bullet collision
                    if (AABBCollision(ship.GetComponent<BulletManager>().bullets[j], manager.GetComponent<AsteroidManager>().asteroids[i]) && !manager.GetComponent<AsteroidManager>().asteroids[i].GetComponent<Asteroid>().justCreated)
                    {
                        // gets the right sprite linked
                        if (manager.GetComponent<AsteroidManager>().asteroids[i].GetComponent<SpriteRenderer>().sprite == asteroidSprites[0])
                        { asteroidPrefab = 0; }
                        else if (manager.GetComponent<AsteroidManager>().asteroids[i].GetComponent<SpriteRenderer>().sprite == asteroidSprites[1])
                        { asteroidPrefab = 1; }
                        else if (manager.GetComponent<AsteroidManager>().asteroids[i].GetComponent<SpriteRenderer>().sprite == asteroidSprites[2])
                        { asteroidPrefab = 2; }

                        #region 2nd and 3rd Spawners
                        // checking the size of the asteroid and make 2 new, smaller ones if not the smallest yet
                        if (manager.GetComponent<AsteroidManager>().asteroids[i].transform.localScale.x == 1f)
                        {
                            // declares a random num
                            Vector3 rot = manager.GetComponent<AsteroidManager>().asteroids[i].transform.rotation.eulerAngles;
                            rot = new Vector3(rot.x, rot.y, rot.z + 15);

                            // spawns new asteroids
                            asteroidsJustSpawned.Add(
                                Instantiate(asteroid2[asteroidPrefab], manager.GetComponent<AsteroidManager>().asteroids[i].transform.position,
                                Quaternion.Euler(rot)));

                            rot = new Vector3(rot.x, rot.y, rot.z - 30);
                            asteroidsJustSpawned.Add(
                                Instantiate(asteroid2[asteroidPrefab], manager.GetComponent<AsteroidManager>().asteroids[i].transform.position,
                                Quaternion.Euler(rot)));

                            // adds to score
                            manager.GetComponent<GUIManager>().score += 20;
                            manager.GetComponent<GUIManager>().spaceDust += 10;
                        }
                        else if (manager.GetComponent<AsteroidManager>().asteroids[i].transform.localScale.x == 0.6f)
                        {
                            // declares a random num
                            Vector3 rot = manager.GetComponent<AsteroidManager>().asteroids[i].transform.rotation.eulerAngles;
                            rot = new Vector3(rot.x, rot.y, rot.z + 15);

                            // spawns new asteroids
                            asteroidsJustSpawned.Add(
                                Instantiate(asteroid3[asteroidPrefab], manager.GetComponent<AsteroidManager>().asteroids[i].transform.position,
                                Quaternion.Euler(rot)));

                            rot = new Vector3(rot.x, rot.y, rot.z - 30);
                            asteroidsJustSpawned.Add(
                                Instantiate(asteroid3[asteroidPrefab], manager.GetComponent<AsteroidManager>().asteroids[i].transform.position,
                                Quaternion.Euler(rot)));

                            // adds to score
                            manager.GetComponent<GUIManager>().score += 50;
                            manager.GetComponent<GUIManager>().spaceDust += 5;
                        }
                        else
                        {
                            // adds to score
                            manager.GetComponent<GUIManager>().score += 100;
                            manager.GetComponent<GUIManager>().spaceDust += 2;
                        }
                        #endregion

                        // for when upgrade is purchased
                        if (plasmaBullet)
                        {
                            // destroys asteroid
                            Destroy(manager.GetComponent<AsteroidManager>().asteroids[i]);
                            manager.GetComponent<AsteroidManager>().asteroids.RemoveAt(i);
                            i--;
                        }
                        else if (!plasmaBullet)
                        {
                            // destroys bullet
                            Destroy(ship.GetComponent<BulletManager>().bullets[j]);
                            ship.GetComponent<BulletManager>().bullets.RemoveAt(j);
                            j--;

                            // destroys asteroid
                            Destroy(manager.GetComponent<AsteroidManager>().asteroids[i]);
                            manager.GetComponent<AsteroidManager>().asteroids.RemoveAt(i);
                            i--;
                        }

                    }
                }
            }

            // sets new asteroids created bool to false
            for (int k = 0; k < asteroidsJustSpawned.Count; k++)
            {
                if (!asteroidsJustSpawned[k].GetComponent<Asteroid>().justCreated)
                {
                    manager.GetComponent<AsteroidManager>().asteroids.Add(asteroidsJustSpawned[k]);

                    asteroidsJustSpawned.RemoveAt(k);
                    k--;
                }
            }
        }

        
    }

    /// <summary>
    /// Detects Asteroid/Ship collisions
    /// </summary>
    public void ShipCollision()
    {
        // if no invincibility frames, do damage
        if (hittable)
        {
            for (int i = 0; i < manager.GetComponent<AsteroidManager>().asteroids.Count; i++)
            {
                if (AABBCollision(ship, manager.GetComponent<AsteroidManager>().asteroids[i]))
                {
                    // DEPENDS ON UPGRADES
                    if (ShipDamageDecreaser[1])
                    {
                        manager.GetComponent<GUIManager>().hullIntegrity -= damage / 4;
                    }
                    else if (ShipDamageDecreaser[0])
                    {
                        manager.GetComponent<GUIManager>().hullIntegrity -= damage / 2;
                    }
                    else
                    {
                        manager.GetComponent<GUIManager>().hullIntegrity -= damage;
                    }

                    ship.GetComponent<SpriteRenderer>().color = Color.blue;
                    hittable = false;
                }
            }
        }

        // ship invincibility frames
        if (hittable == false)
        {
            // updating timer
            timer += 1.0f + Time.deltaTime;
        }

        // checking timer
        if (timer >= 120f)
        {
            ship.GetComponent<SpriteRenderer>().color = Color.white;
            hittable = true;
            timer = 0f;
        }
    }

    /// <summary>
    /// Checks collision between two sprites
    /// </summary>
    /// <param name="sprite1"></param>
    /// <param name="sprite2"></param>
    /// <returns></returns>
    public bool AABBCollision(GameObject sprite1, GameObject sprite2)
    {
        // getting box colliders
        BoxCollider2D box1 = sprite1.GetComponent<BoxCollider2D>();
        BoxCollider2D box2 = sprite2.GetComponent<BoxCollider2D>();
        Vector2 pos1 = sprite1.transform.position;
        Vector2 pos2 = sprite2.transform.position;

        // setting variable checkers for a
        float a_min_x = pos1.x;
        float a_min_y = pos1.y;
        float a_max_x = pos1.x + box1.bounds.size.x;
        float a_max_y = pos1.y + box1.bounds.size.y;
        // settomg variable checkers for b
        float b_min_x = pos2.x;
        float b_min_y = pos2.y;
        float b_max_x = pos2.x + box2.bounds.size.x;
        float b_max_y = pos2.y + box2.bounds.size.y;

        // checking bounds
        if (a_max_x < b_min_x || b_max_x < a_min_x || a_max_y < b_min_y || b_max_y < a_min_y)
        {
            return false;
        }

        return true;
    }
    
}
