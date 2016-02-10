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
    public static float MIN_SIZE = 4.0f;
    public static float MAX_SIZE = 6.0f;

    public GameObject[] planetPrefab;

    private GameObject[] _planets;  // orbiting children

    private Vector2 _offset;

    public bool debug = false;

    private float _pingPongOffset;

    void Start()
    {
        _pingPongOffset = Random.Range(0f,1f);
        _layeredSprite = GetComponent<LayeredSprite>();
        myNumber = Procedural.GetNumber(Procedural.PointToNumber((int)transform.position.x, (int)transform.position.y));
        Generate();
        SetChildren();
        _offset = new Vector2(1/Procedural.GetNumber(myNumber),1/Procedural.GetNumber(myNumber));
        transform.position += (Vector3) _offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Random.Range(10,50)*Time.deltaTime);
        Size = Mathf.PingPong(Time.time + _pingPongOffset,0.5f);
    }    

    // Procedurally generate the star
    private void Generate()
    {
        Size = 0.5f;
        _layeredSprite.Randomize(myNumber);
    }

    // TO-DO
    private void SetChildren()
    {
        
    }    
}