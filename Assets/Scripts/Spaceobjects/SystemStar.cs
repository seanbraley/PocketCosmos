using UnityEngine;
using System.Collections;
using Completed;        // include this namespace in order to access game manager 
using Superbest_random; // Included for gaussian

public class SystemStar : PlanetaryBody {

	public static string TAG = "Star";
	public static float STELLAR_DISTANCE_CONSTANT = 1.35f;
	public static float PLANET_DISTANCE_CONSTANT = 1.75f;
    public static int MEAN_PLANET = 5;
    public static double RANGE_PLANET = 1.45;
	public static int MIN_PLANETS = 2;
	public static int MAX_PLANETS = 7;
    public static int MIN_ROTATION = 5;
	public static int MAX_ROTATION = 15;
	public static int MIN_SIZE = 40;
	public static int MAX_SIZE = 50;

	private bool discovered;
    public System.DateTime discoveryTime;
    public int numPlanets;
	public float rotationSpeed;
	private float minDist = 5000;
	private float maxDist = 10000;
	public GameObject[] planets;
	public GameObject planetPrefabSmooth;
	public GameObject planetPrefabJagged;
	public GameObject planetPrefabWindy;

    private static int testSeed = 42;
    public int myNumber;

    public System.Random myRNG;

	private Player player;

    private Renderer renderer;
   


	/*~*~*~*~*~*~*~*~*~*~*~* Initialization *~*~*~*~*~*~*~*~*~*~*~*/
	/* All code under this subheading will be called once, at the
	 * beginning of the game session.
	 */
	
	void Start()
    {
        base.Start();
        parentBody = null;
        // testing
        //Initialize(testSeed);
        GameObject go = GameObject.Find("GameManager");
        if (go != null) {
        	myNumber = (int)go.GetComponent<GameManager>().selectedID;
            testSeed = myNumber;
            Initialize(testSeed);
        }
    }

    void Initialize(int value)
    {
        // Sets basic things
        Randomize(value);

        Size = localRNG.Next(MIN_SIZE, MAX_SIZE);

        // The necessity of these should be checked
        renderer = GetComponent<Renderer>();
        player = Camera.main.GetComponent<Player>();

        // Modify the sprite
        _layeredSprite.Randomize((uint)localRNG.Next());

        discoveryTime = GameManager.destinationStarDiscoveryTime;

        // Set up the solar system
        BuildSolarSystem();
    }

    void Initialize(uint value)
    {
        myRNG = new System.Random((int)value);
        Initialize();
    }

    /* void Initialize ():
	 * 		For initial creation of stars in a new game.
	 * 		Randomizes:
	 * 			Name
	 * 			Rotation
	 * 			Size
	 */

    void Initialize () {
		renderer = GetComponent<Renderer>();
        player = Camera.main.GetComponent<Player>();
        rotationSpeed = myRNG.Next(MIN_ROTATION, MAX_ROTATION);

        float size = 20f;
        transform.localScale = new Vector3(size, size, size);
		BuildSolarSystem();
    }

    void BuildSolarSystem()
    {
        //localRNG.Next(MIN_PLANETS, MAX_PLANETS+1)
        double val = (RANGE_PLANET * localRNG.NextGaussian());
        numPlanets = (int)(MEAN_PLANET + val);
        planets = new GameObject[numPlanets];
        Vector3 planetOrbitPos = transform.position;

        for (int i = 0; i < numPlanets; i++)
        {
            planetOrbitPos += new Vector3(transform.localScale.x * PLANET_DISTANCE_CONSTANT, 0);
            int planetType = localRNG.Next(2);
            if (planetType == 1)
                planets[i] = Instantiate(planetPrefabWindy, planetOrbitPos, Quaternion.identity) as GameObject;
            else if (planetType == 2)
                planets[i] = Instantiate(planetPrefabJagged, planetOrbitPos, Quaternion.identity) as GameObject;
            else
                planets[i] = Instantiate(planetPrefabSmooth, planetOrbitPos, Quaternion.identity) as GameObject;

            planets[i].GetComponent<Planet>().name = name + System.Convert.ToChar(65 + i);
            planets[i].GetComponent<Planet>().PlanetType = planetType;
            planets[i].GetComponent<Planet>().Randomize(localRNG.Next());
            planets[i].GetComponent<Planet>().Initialize(this, i + 1);
        }
    }


	/* void SetMinDist(float dist)
	 * 		Sets the minimum legal distance for spawning neighbor stars.
	 * 		Used to increase the legal distance of the next neighbor stars.
	 */
	public void SetMinDist(float dist) {
		minDist = dist;
	}

	/* void SetMaxDist(float dist)
	 * 		Sets the maximum legal distance for spawning neighbor stars.
	 * 		Used to increase the legal distance of the next neighbor stars.
	 */
	public void SetMaxDist(float dist) {
		maxDist = dist;
	}

	/* Vector3[] GeneratePoints(float minDist, float maxDist)
	 * 		Returns a series of Vectors which correspond to possible
	 * 		star spawn points.
	 */
	Vector3[] GeneratePoints(float minDist, float maxDist) {
		Vector3[] points = new Vector3[36];
		float deltaTheta = (2.0f *  Mathf.PI) / 36;
		float theta = 0;
		for (int i = 0; i < 36; i++) {
			float radius = Random.Range(minDist,maxDist);
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);
			Vector3 pos = new Vector3(x, y, 0) + transform.position;
			points[i] = pos;
			theta += deltaTheta;
		}
		return points;
	}

	/* bool CheckLegality(Vector3 pos, float distance)
	 * 		Checks to see if a position is a legal
	 * 		for a star, given a specified legal distance.
	 */
	bool CheckLegality(Vector3 pos, float distance) {
		GameObject[] stars = GameObject.FindGameObjectsWithTag ("Star");
		foreach (GameObject star in stars) {
			if (Vector3.Distance(pos,star.transform.position) < distance-1)
				return false;
		}
		return true;
	}



	/*~*~*~*~*~*~*~*~*~*~*~* Updating *~*~*~*~*~*~*~*~*~*~*~*/
	/* All code under this subheading will be called on a
	 * per-frame basis. Maybe not all frames, but most.
	 */
	
	void Update () {
		transform.Rotate (Vector3.back, rotationSpeed*Time.deltaTime);
    }
}
