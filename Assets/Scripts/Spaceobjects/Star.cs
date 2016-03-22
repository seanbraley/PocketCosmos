using UnityEngine;
using UnityEngine.SceneManagement;  // scene management at run-time.
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Completed;

/// <summary>
/// This class is for the stars found in the sector view (the little ones)
/// </summary>
public class Star : PlanetaryBody {

    // Add specific star properties
    public uint myNumber;

    private Object myLock = new Object();
    private int keepLoaded = 0;

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
                ConnectToNearbyStars();
                KeepLoaded();
            }
            else {
                //CurrentWaypoint = UndiscoveredStarIcon_Prefab;
                if (_layeredSprite != null)
                    _layeredSprite.SetColors(
                        new Color(221 / 255f, 160 / 255f, 221 / 255f),  // Plum
                        new Color(138 / 255f, 43 / 255f, 226 / 255f)    // Blue Violet
                    );
                DisconnectFromNearbyStars();
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

    public static float NEARBY_STAR_DISTANCE = 6*Mathf.PI;

    public GameObject[] planetPrefab;
    public Material lineRendererMaterial;

    private GameObject[] _planets;  // orbiting children

    private Vector2 _offset = Vector2.zero;

    public bool debug = false;

    private System.DateTime discoveryTime;

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
                Vector3 SBsize = new Vector3(transform.localScale.x * 3, transform.localScale.x * 3, transform.localScale.x * 3);
                _currentWaypoint.transform.localScale = SBsize;
                _currentWaypoint.transform.parent = this.transform;
            }
        }
    }

    void Start()
    {
        // Create local RNG from star seed
        localRNG = new System.Random((int)myNumber);

        // Initialize sprites etc
        base.Start();

        // If we are in the sector scene calculate offset
        if (SceneManager.GetActiveScene().buildIndex == 2)  // Sector level
        {
            _offset = new Vector2(localRNG.Next(5) / 10f, localRNG.Next(5) / 10f);
        }
        
        // Apply offset
        transform.position += (Vector3) _offset;

        // Update from the game data - check if user has discovered this star or not
        foreach (long starID in PlayerData.instance.discoveredStarSystems)
        {
            if (starID == myNumber)
            {
                Discovered = true;
                //discoveryTime = s.discoveryTime;
            }            
        }

        Generate();

        if (Discovered) {
            ConnectToNearbyStars();
        }
    }

    public Dictionary<Star,LineRenderer> neighborConnections;

    void ConnectToNearbyStars() {
        DisconnectFromNearbyStars();
        neighborConnections = new Dictionary<Star,LineRenderer>();
        Debug.Log(GameManager.allStars.Count);
        List<Star> nearbyHomeStars = new List<Star>();

        foreach (GameObject star_obj in GameManager.allStars){
            Star star = star_obj.GetComponent<Star>();
            if (star != this && star.Discovered && Vector3.Distance(transform.position,star.transform.position) <= NEARBY_STAR_DISTANCE) { // TODO: currently functioning as HOMESTARS (Change this)
                nearbyHomeStars.Add(star);
            }
        }

        nearbyHomeStars = nearbyHomeStars.OrderBy(x => Vector3.Distance(this.transform.position,x.transform.position)).ToList();
        nearbyHomeStars = (from star in nearbyHomeStars
                where (!star.neighborConnections.ContainsKey(this)) select star).ToList();

        for(int i=0;i < Mathf.Min(2,nearbyHomeStars.Count); i++) {
            Star star = nearbyHomeStars[i];

            GameObject lr_obj = new GameObject();
            lr_obj.transform.position = this.transform.position;
            lr_obj.gameObject.name = "NeighborConnection";
            lr_obj.transform.parent = this.transform;
            lr_obj.AddComponent<LineRenderer>();

            LineRenderer lr = lr_obj.GetComponent<LineRenderer>();
            lr.receiveShadows = false;
            lr.castShadows = false;
            lr.material = lineRendererMaterial;
            lr.material.SetColor ("_TintColor", new Color(0,1f,0,(100f/255f)*0.4f));
            lr.SetWidth(0.3f,0.3f);
            lr.SetPositions(new Vector3[] {transform.position,star.transform.position});
            neighborConnections.Add(star,lr);
        }
    }

    void DisconnectFromNearbyStars() {
        if (neighborConnections != null) {
            foreach (KeyValuePair<Star,LineRenderer> kvp in neighborConnections) {
                LineRenderer lr = kvp.Value;
                Destroy(lr.gameObject);
            }
            neighborConnections = new Dictionary<Star,LineRenderer>();
        }
    }

    void UpdateNeighborConnections() {
        if (neighborConnections != null) {
            foreach (KeyValuePair<Star,LineRenderer> kvp in neighborConnections) {
                LineRenderer lr = kvp.Value;
                Star star = kvp.Key;
                if (star == null) {
                    Destroy(lr.gameObject);
                }
                else {
                    lr.SetPositions(new Vector3[] {transform.position,star.transform.position});
                }
            }
            neighborConnections = (from kv in neighborConnections
                where kv.Key != null select kv).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }

    public bool CheckUnload()
    {
        lock(myLock)
        {
            return keepLoaded == 0;
        }
    }

    public void KeepLoaded()
    {
        lock(myLock)
        {
            keepLoaded++;
            GameManager.keepLoadedStars.Add(gameObject);
        }
    }

    public void Unload()
    {
        lock(myLock)
        {
            keepLoaded--;
            GameManager.keepLoadedStars.Remove(gameObject);
        }
    }

    public System.DateTime GetDiscoveryTime()
    {
        return discoveryTime;
    }

    public void SetDiscoveryTime(System.DateTime time)
    {
        discoveryTime = time;
    }


    public uint GetNumber()
    {
        return myNumber;
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

        UpdateNeighborConnections();
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