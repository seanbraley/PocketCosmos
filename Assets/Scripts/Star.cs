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

    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        myNumber = Procedural.GetNumber(Procedural.PointToNumber((int)transform.position.x, (int)transform.position.y));
        Generate();
        SetChildren();
    }

    // Update is called once per frame
    void Update()
    {

    }    

    // Procedurally generate the star
    private void Generate()
    {
        Size = 1f;
        _layeredSprite.Randomize(myNumber);
    }

    // TO-DO
    private void SetChildren()
    {
        
    }    
   
}
