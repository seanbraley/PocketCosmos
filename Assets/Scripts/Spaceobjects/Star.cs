using UnityEngine;
using System.Collections;

public class Star : PlanetaryBody {

    // Add specific star properties
    public uint myNumber;

    private bool discovered;
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

    private Vector2 _offset;

    public bool debug = false;

    private System.Random localRNG;

    void Start()
    {
        localRNG = new System.Random((int)myNumber);

        _layeredSprite = GetComponent<LayeredSprite>();

        Generate();
        SetChildren();

        float offset_x = localRNG.Next(5) / 10f;
        float offset_y = localRNG.Next(5) / 10f;


        _offset = new Vector2(offset_x, offset_y);
        
        transform.position += (Vector3) _offset;
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
    }    

    // Procedurally generate the star
    private void Generate()
    {
        Size = localRNG.Next(65, 100) / 100f;
        _layeredSprite.RandomizeSectorStar(localRNG);
    }

    // TO-DO
    private void SetChildren()
    {
        
    }    
}