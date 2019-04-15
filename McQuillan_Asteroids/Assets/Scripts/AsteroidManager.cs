using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    // bullets
    public List<GameObject> asteroids;
    [SerializeField]
    public List<GameObject> asteroidPrefabs;

    // camera
    private new Camera camera;
    private float totalCamHeight;
    private float totalCamWidth;

    // Use this for initialization
    void Start ()
    {
        // initialize asteroid list
        asteroids = new List<GameObject>();

        // camera initialization
        camera = Camera.main;
        totalCamHeight = camera.orthographicSize;
        totalCamWidth = totalCamHeight * camera.aspect;

        // set initial list
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = new Vector3(Random.Range(-totalCamWidth, totalCamWidth), Random.Range(totalCamHeight - 2, totalCamHeight), 1);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, 3)], position, rotation));
            asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, 3)], -position, rotation));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (asteroids.Count == 0)
        {
            // spawning 8 new asteroids
            for (int i = 0; i < 4; i++)
            {
                Vector3 position = new Vector3(Random.Range(-totalCamWidth, totalCamWidth), Random.Range(totalCamHeight - 1, totalCamHeight), 1);
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

                asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, 3)], position, rotation));
                asteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, 3)], -position, rotation));
            }
        }
	}
}
