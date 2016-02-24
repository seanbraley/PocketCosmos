using UnityEngine;
using UnityEngine.SceneManagement;  // scene management at run-time.
using System.Collections;

public class Star : PlanetaryBody {

    // Add specific star properties
    public uint myNumber;

    private bool _discovered;
    public bool Discovered {
        get {
            return _discovered;
        }
        set {
            if (value) {
                if (_layeredSprite != null)
                    _layeredSprite.SetColors(
                        new Color(0 / 255f, 255 / 255f, 0 / 255f),      // Lime Green
                        new Color(0 / 255f, 128 / 255f, 0 / 255f)       // Green
                    );
                //CurrentWaypoint = HomeStarIcon_Prefab;
            }
            else {
                //CurrentWaypoint = UndiscoveredStarIcon_Prefab;
                if (_layeredSprite != null)
                    _layeredSprite.SetColors(
                        new Color(221 / 255f, 160 / 255f, 221 / 255f),  // Plum
                        new Color(138 / 255f, 43 / 255f, 226 / 255f)    // Blue Violet
                    );
            }
            _discovered = value;
        }
    }

    private float minDist = 5000;
    private float maxDist = 10000;

    // Stellar system properties
    public static float STELLAR_DISTANCE_CONSTANT = 1.35f;
    public static float PLANET_DISTANCE_CONSTANT = 10;
    public static int MAX_PLANETS = 8;
    public static float MIN_ROTATION = 5;
    public static float MAX_ROTATION = 15;
    public static int MIN_SIZE = 15;
    public static int MAX_SIZE = 1;

    public GameObject[] planetPrefab;

    private GameObject[] _planets;  // orbiting children

    private Vector2 _offset = Vector2.zero;

    public bool debug = false;

    private System.Random localRNG;

    public GameObject HomeStarIcon_Prefab;
    public GameObject UndiscoveredStarIcon_Prefab;
    private GameObject _currentWaypoint;
    public GameObject CurrentWaypoint {
        get {
            return _currentWaypoint;
        }
        set {
            Destroy(_currentWaypoint);
            if (value == null) {
                _currentWaypoint = null;
            }
            else
            {
                _currentWaypoint = Instantiate(value,transform.position + Vector3.back*3,Quaternion.identity) as GameObject;
                Vector3 SBsize = new Vector3(transform.localScale.x*3,transform.localScale.x*3,transform.localScale.x*3);
                _currentWaypoint.transform.localScale = SBsize;
                _currentWaypoint.transform.parent = this.transform;
            }
        }
    }

    void Start()
    {
        localRNG = new System.Random((int)myNumber);

        base.Start();

        if (SceneManager.GetActiveScene().buildIndex == 2)  // Sector level
        {
            _offset = new Vector2(localRNG.Next(5) / 10f, localRNG.Next(5) / 10f);
        }
        
        transform.position += (Vector3) _offset;

        // Update from the game data - check if user has discovered this star or not
        foreach (DiscoveredStar s in PlayerData.playdata.discoveredStarSystems) {
            if (s.starID == this.myNumber) {
                Discovered = true;
            }
        }
        if (!Discovered) {
            //CurrentWaypoint = UndiscoveredStarIcon_Prefab;
        }

        Generate();
        //_currentWaypoint = null;

    }

    public void SetNumber(int x, int y)
    {
        myNumber = Procedural.GetNumber(Procedural.PointToNumber(x, y));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Random.Range(10,50)*Time.deltaTime);
        //Size = Mathf.PingPong(Time.time + _pingPongOffset,0.5f);
        if (CurrentWaypoint != null) {
            CurrentWaypoint.transform.rotation = Quaternion.identity;
        }
    }    

    // Procedurally generate the star
    private void Generate()
    {
        Size = localRNG.Next(65, 100) / 100f;
        if (!Discovered)
            _layeredSprite.SetColors(
                new Color(221 / 255f, 160 / 255f, 221 / 255f),  // Plum
                new Color(138 / 255f, 43 / 255f, 226 / 255f)    // Blue Violet
            );
        else
            _layeredSprite.SetColors(
                new Color(0 / 255f, 255 / 255f, 0 / 255f),      // Lime Green
                new Color(0 / 255f, 128 / 255f, 0 / 255f)       // Green
            );
        //_layeredSprite.RandomizeSectorStar(localRNG);
    }

    // TO-DO
    private void SetChildren()
    {
        
    }    
}