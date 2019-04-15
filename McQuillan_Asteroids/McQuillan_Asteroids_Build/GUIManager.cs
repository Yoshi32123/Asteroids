using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    // score fields
    public float score;
    public float hullIntegrity;

    // store credit
    public float spaceDust;
    private float bulletPrice;
    private float shipPrice;
    private float hullPrice;

    // ship fields
    public GameObject userPrefab;
    public GameObject ship;
    public Sprite[] ships;
    private int shipUpgradeIndex;
    private int hullUpgradeIndex;
    private string currentShip;
    private string currentHull;
    private bool shipDestroyed;

    // cross-script changing fields
    private bool[] crossBullet = { false, false };
    private bool[] crossShip = { false, false };
    private bool[] crossHull = { false, false };

    // bullet prefabs
    public GameObject[] bullets;
    private int bulletUpgradeIndex;
    private string currentBullet;

	// Use this for initialization
	void Start ()
    {
        score = 0f;
        spaceDust = 0f;
        hullIntegrity = 100f;

        // starter pricing
        bulletPrice = 250f;
        shipPrice = 300f;
        hullPrice = 500f;

        // starter index for upgrades
        bulletUpgradeIndex = 1;
        hullUpgradeIndex = 0;
        shipUpgradeIndex = 0;

        // starter strings for GUI
        currentBullet = "energy (normal)";
        currentShip = "type 01 - grunt";
        currentHull = "titanium hull (weak)";

        // setting starter bools for upgrade stats
        crossBullet[0] = false;
        crossBullet[1] = false;
        crossShip[0] = false;
        crossShip[1] = false;
        crossHull[0] = false;
        crossHull[1] = false;

        // ship alive
        shipDestroyed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (hullIntegrity > 0)
        {
            Upgrade();
            StatChanger();
        }

        // keeps hull integrity at 0 if it drops below 0
        if (hullIntegrity < 0)
        { hullIntegrity = 0; }
    }

    /// <summary>
    /// Handles bullet/ship/hull upgrading
    /// </summary>
    private void Upgrade()
    {
        // upgrading bullets
        if (spaceDust > bulletPrice && bulletUpgradeIndex != 3)
        {
            // changes bullet prefab
            if (Input.GetKeyDown(KeyCode.B))
            {
                ship.GetComponent<BulletManager>().bulletPrefab = bullets[bulletUpgradeIndex];
                bulletUpgradeIndex++;
                spaceDust -= bulletPrice;
                bulletPrice += bulletPrice * 2;
            }

            // setting the right string for GUI
            if (ship.GetComponent<BulletManager>().bulletPrefab == bullets[1])
            {
                currentBullet = "plasma (burns through)";
                crossBullet[0] = true;
            }
            else if (ship.GetComponent<BulletManager>().bulletPrefab == bullets[2])
            {
                currentBullet = "phazon (screen wraps)";
                bulletPrice = 99999f;
                crossBullet[1] = true;
            }
        }

        // upgrading ship
        if (spaceDust > shipPrice && shipUpgradeIndex != 2)
        {
            // changes in game ship sprite
            if (Input.GetKeyDown(KeyCode.G))
            {
                shipUpgradeIndex++;
                ship.GetComponent<SpriteRenderer>().sprite = ships[shipUpgradeIndex * 3 + hullUpgradeIndex];
                spaceDust -= shipPrice;
                shipPrice += shipPrice * 2;
            }

            // setting the right string for GUI
            if (ship.GetComponent<SpriteRenderer>().sprite == ships[3] || ship.GetComponent<SpriteRenderer>().sprite == ships[4] || ship.GetComponent<SpriteRenderer>().sprite == ships[5])
            {
                currentShip = "type 02 - speed";
                crossShip[0] = true;
            }
            else if (ship.GetComponent<SpriteRenderer>().sprite == ships[6] || ship.GetComponent<SpriteRenderer>().sprite == ships[7] || ship.GetComponent<SpriteRenderer>().sprite == ships[8])
            {
                currentShip = "type 03 - whirlwind";
                shipPrice = 99999f;
                crossShip[1] = true;
            }
        }

        // upgrading hull
        if (spaceDust > hullPrice && hullUpgradeIndex != 2)
        {
            // changes in game ship sprite
            if (Input.GetKeyDown(KeyCode.H))
            {
                hullUpgradeIndex++;
                ship.GetComponent<SpriteRenderer>().sprite = ships[shipUpgradeIndex * 3 + hullUpgradeIndex];
                spaceDust -= hullPrice;
                hullPrice += hullPrice * 2;
            }

            // setting the right string for GUI
            if (ship.GetComponent<SpriteRenderer>().sprite == ships[1] || ship.GetComponent<SpriteRenderer>().sprite == ships[4] || ship.GetComponent<SpriteRenderer>().sprite == ships[7])
            {
                currentHull = "mithril hull (strong)";
                crossHull[0] = true;
            }
            else if (ship.GetComponent<SpriteRenderer>().sprite == ships[2] || ship.GetComponent<SpriteRenderer>().sprite == ships[5] || ship.GetComponent<SpriteRenderer>().sprite == ships[8])
            {
                currentHull = "jauxite hull (epic)";
                hullPrice = 99999;
                crossHull[1] = true;
            }
        }
    }

    /// <summary>
    /// Changes the stats on the specific object the upgrade is applied to
    /// </summary>
    private void StatChanger()
    {
        // bullet implementation
        ship.GetComponent<BulletManager>().bulletModifier = crossBullet;
        gameObject.GetComponent<CollisionDetection>().plasmaBullet = crossBullet[0];
        

        // ship implementation
        ship.GetComponent<ShipMovement>().speedUpgrade = crossShip;

        // hull implementation
        gameObject.GetComponent<CollisionDetection>().ShipDamageDecreaser = crossHull;
    }

    /// <summary>
    /// Handles store and scoring
    /// </summary>
    private void OnGUI()
    {
        if (hullIntegrity > 0)
        {
            // setting GUI characteristics
            GUI.color = Color.green;
            GUI.skin.box.fontSize = 12;
            GUI.skin.box.wordWrap = true;
            GUI.Box(new Rect(10, 10, 220, 210),
                string.Format(
                    "CURRENT STATS:\n" +
                    "Score: {0}\n" +
                    "Space Dust (currency): {1}\n" +
                    "Hull Integrity: {2}%\n" +
                    "\n" +
                    "UPGRADES:\n" +
                    "Upgrade Bullet (\"b\") Cost: {3}\n" +
                    "Upgrade Ship (\"g\") Cost: {4}\n" +
                    "Upgrade Hull (\"h\") Cost: {5}\n" +
                    "\n" +
                    "PLAYER STATS:\n" +
                    "Current bullet: {6}\n" +
                    "Current ship: {7}\n" +
                    "Current hull: {8}",
                    score, spaceDust, hullIntegrity, bulletPrice, shipPrice, hullPrice, currentBullet, currentShip, currentHull));
        }

        // If ship is dead
        if (hullIntegrity <= 0)
        {
            if (!shipDestroyed)
            // destroying bullets on screen
            for (int i = 0; i < ship.GetComponent<BulletManager>().bullets.Count; i++)
            {
                Destroy(ship.GetComponent<BulletManager>().bullets[i]);
                ship.GetComponent<BulletManager>().bullets.RemoveAt(i);
                i--;
            }

            // destroying ship
            Destroy(ship);
            shipDestroyed = true;

            // GUI Details
            GUI.color = Color.red;
            GUI.skin.box.fontSize = 24;
            GUI.skin.box.wordWrap = true;
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 150), 
                string.Format(
                    "You Died.\n" +
                    "Score: {0}\n" +
                    "\n" +
                    "Press \"Enter\" to play again", score)
                    );

            // restarts game
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ship = Instantiate(userPrefab);

                gameObject.GetComponent<CollisionDetection>().ship = ship;

                for (int i = 0; i < gameObject.GetComponent<AsteroidManager>().asteroids.Count; i++)
                {
                    Destroy(gameObject.GetComponent<AsteroidManager>().asteroids[i]);
                    gameObject.GetComponent<AsteroidManager>().asteroids.RemoveAt(i);
                    i--;
                }

                Start();
            }
        }
    }
}
