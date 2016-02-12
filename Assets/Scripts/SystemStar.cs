using UnityEngine;
using System.Collections;

public class SystemStar : PlanetaryBody {

	public static string TAG = "Star";
	public static float STELLAR_DISTANCE_CONSTANT = 1.35f;
	public static float PLANET_DISTANCE_CONSTANT = 1.75f;
	public static int MIN_PLANETS = 6;
	public static int MAX_PLANETS = 6;
    public static int MIN_ROTATION = 5;
	public static int MAX_ROTATION = 15;
	public static int MIN_SIZE = 50;
	public static int MAX_SIZE = 60;

	private bool discovered;
	public float rotationSpeed;
	private float minDist = 5000;
	private float maxDist = 10000;
	public GameObject[] planets;
	public GameObject planetPrefab;

    private static int testSeed = 42;

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
        //Initialize();
        // testing
        Initialize(42);
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

        // Set up the solar system
        BuildSolarSystem(localRNG.Next(MIN_PLANETS, MAX_PLANETS));
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

		BuildSolarSystem(localRNG.Next(MIN_PLANETS, MAX_PLANETS+1));
    }

    void BuildSolarSystem(int numPlanets)
    {
        planets = new GameObject[numPlanets];
        Vector3 planetOrbitPos = transform.position;

        for (int i = 0; i < numPlanets; i++)
        {
            planetOrbitPos += new Vector3(transform.localScale.x * PLANET_DISTANCE_CONSTANT, 0);
            planets[i] = Instantiate(planetPrefab, planetOrbitPos, Quaternion.identity) as GameObject;
            planets[i].GetComponent<Planet>().name = name + System.Convert.ToChar(65 + i);
            planets[i].GetComponent<Planet>().Randomize(localRNG.Next());
            planets[i].GetComponent<Planet>().Initialize(this, i + 1);
        }
    }


    /* void BuildSolarSystem (int numPlanets)
	 * 		Builds a solar system with a set number of planets.
	 * 		The planets will be generated randomly.
	 */
    public void BuildSolarSystem(int numPlanets, bool discovered)
    {
		if (discovered)  // Only call this once?
			return;
		discovered = true;
		planets = new GameObject[numPlanets];
		Vector3 planetPos = transform.position;
		for (int i = 0; i < numPlanets; i++)
        {
            planetPos += new Vector3(transform.localScale.x * 0.8f * PLANET_DISTANCE_CONSTANT, 0, 0);
            planets[i] = Instantiate(planetPrefab, planetPos, Quaternion.identity) as GameObject;
            planets[i].GetComponent<Planet>().SetOrbitParent(this.gameObject);
            planets[i].name = name + System.Convert.ToChar(65 + i);  // Ehhh Bee Seee Dee EEEE efff geee...
            planets[i].GetComponent<Planet>().Randomize(localRNG.Next());
            planets[i].GetComponent<Planet>().Initialize(this, i);


            //planets[i]
        }
	}

	/* void SpawnNeighbors()
	 * 		Creates the neighboring stars of a star.
	 * 		To be called when the star is discovered.
	 */
	public void SpawnNeighbors() {
		Vector3[] points = GeneratePoints(minDist,maxDist);
		int randIndex = Random.Range(0,points.Length);
		for (int i = randIndex; i < randIndex + points.Length; i++) {
			float dist = Random.Range(minDist,maxDist);
			if (CheckLegality(points[i % points.Length],dist)) {
				GameObject neighbor = Instantiate (this.gameObject,
				                                   points[i % points.Length],
				                                   Quaternion.identity) as GameObject;
				neighbor.GetComponent<SystemStar>().SetMinDist (minDist * STELLAR_DISTANCE_CONSTANT);
				neighbor.GetComponent<SystemStar>().SetMaxDist (maxDist * STELLAR_DISTANCE_CONSTANT);
			}
			
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
