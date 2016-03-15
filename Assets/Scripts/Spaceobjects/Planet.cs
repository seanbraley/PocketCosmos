using UnityEngine;
using System.Collections;
using Superbest_random;

public class Planet : PlanetaryBody {

	/* KNOWN ISSUES:
	 * 		- Planets only orbit if they are visible, to allow for huge galaxies.
	 * 		  This has potential to be exploited to minimize travel time.
	 * 		  Consider factoring in whether the home star is also visible (too lazy right now).
	 * 		  	  If this is the case, moons must also be accounted for.
	 * 		  Otherwise, don't use actual distance to calculate travel time (meeeehhhhh).
	 */

	public static string TAG = "Planet";
	public static float MIN_COLOR = 0.25f;
	public static float MAX_COLOR = 0.75f;
	public static int MIN_SIZE = 25;
	public static int MAX_SIZE = 35;
	public static float MIN_ORBIT = 0.8f;
	public static float MAX_ORBIT = 1.2f;
    public static int MIN_ORBIT_SPEED = 5;
    public static int MAX_ORBIT_SPEED = 10;
	public static int MIN_ROTATION = 2;
	public static int MAX_ROTATION = 5;
	public static float ORBIT_CONSTANT = 35;
	public static int ORBIT_PATH_SEGMENTS = 128;
	public static float ORBIT_PATH_WIDTH = 0.004f;
	public static float MOON_CHANCE = 0.25f;
	public static int MAX_MOONS = 1;

	public int orbitSpeed;

    // Gameplay variables
    public double energyModifier;
    public double populationRate;
    public uint population;

	public LineRenderer orbitPath;
	public GameObject orbitParent;
	public SystemStar homeStar;
	public Planet homePlanet;
	//public float rotationSpeed;
	public GameObject[] moons;

    public int planetNum;
    public uint myNumber;

	private Renderer renderer;

    private double rotationDistance;
    private int initialRotationOffset;

    public int PlanetType;

    public GameObject CollectSpacebuxxxxxxx_Prefab;
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
    			Vector3 SBsize = new Vector3(transform.localScale.x,transform.localScale.x,transform.localScale.x);
    			_currentWaypoint.transform.localScale = SBsize;
    			_currentWaypoint.transform.parent = this.transform;
    		}
    	}
    }

	void Start ()
    {
        SetUpRNG(myNumber);
        
        base.Start();

        homeStar = parentBody.GetComponent<SystemStar>();

        // Set Color
        if (planetNum <= 2)  // hot planets generate more power, negative population rate (usually)
        {
            energyModifier = localRNG.NextGaussian(2);
            populationRate = localRNG.NextGaussian(-1);
            _layeredSprite.Randomize((uint)localRNG.Next(), ref localRNG, "red");
        }
        else if (planetNum > 2 & planetNum <= 5)
        {
            energyModifier = localRNG.NextGaussian(1);
            populationRate = localRNG.NextGaussian(2);
            _layeredSprite.Randomize((uint) localRNG.Next(), ref localRNG, "green");
        }
        else
        {
            energyModifier = localRNG.NextGaussian(-1);
            populationRate = localRNG.NextGaussian(-1);
            _layeredSprite.Randomize((uint)localRNG.Next(), ref localRNG, "blue");
        }

        Debug.Log(string.Format("Created planet: {0}{1}. EnergyProduced: {2}, populationRate: {3}", 
            homeStar.myNumber, System.Convert.ToChar(64 + planetNum), energyModifier*homeStar.baseEnergyLevel, populationRate));

        renderer = GetComponent<Renderer>();

        Size = localRNG.Next(MIN_SIZE, MAX_SIZE);


        // Set Planet Size
        transform.localScale = new Vector3(Size, Size, Size);

        // Orbit Speed is a function of distance from orbit parent (whether it be a planet or star)
        orbitSpeed = localRNG.Next(MIN_ORBIT_SPEED, MAX_ORBIT_SPEED);

        // Rotational offset
        initialRotationOffset = localRNG.Next(0, 360);

        // Randomize Location around orbit parent (for illusion of time passing)
        // Consider actually counting the amount of time the user has been away.
        transform.position = (transform.position - parentBody.transform.position).normalized * ORBIT_CONSTANT * planetNum +
                             parentBody.transform.position;
        transform.RotateAround(parentBody.transform.position, Vector3.forward, initialRotationOffset);

        // Adjust for persistent rotation
        System.TimeSpan dt = System.DateTime.Now - homeStar.discoveryTime;
        transform.RotateAround(parentBody.transform.position, Vector3.forward, -orbitSpeed * (float)dt.TotalSeconds);

        // Draw orbit path (same color as planet)
        orbitPath = GetComponent<LineRenderer>();
        orbitPath.materials[0].color = Color.gray;

        // Draw the orbit.
        DrawOrbit();

    }

    public void Initialize(PlanetaryBody parent, int planetNum)
    {
        base.Start();

        parentBody = parent;

        this.planetNum = planetNum;

        renderer = GetComponent<Renderer>();

        Size = localRNG.Next(MIN_SIZE, MAX_SIZE);

        homeStar = parentBody.GetComponent<SystemStar>();

        //Randomize Rotation Speed
        //rotationSpeed = localRNG.Next(MIN_ROTATION, MAX_ROTATION);

        // Set Planet Size
        transform.localScale = new Vector3(Size, Size, Size);

        // Orbit Speed is a function of distance from orbit parent (whether it be a planet or star)
        orbitSpeed = localRNG.Next(MIN_ORBIT_SPEED, MAX_ORBIT_SPEED);

        // Rotational offset
        initialRotationOffset = localRNG.Next(0, 360);

        // Randomize Location around orbit parent (for illusion of time passing)
        // Consider actually counting the amount of time the user has been away.
        transform.position = (transform.position - parentBody.transform.position).normalized*ORBIT_CONSTANT*planetNum +
                             parentBody.transform.position;
        transform.RotateAround(parentBody.transform.position, Vector3.forward, initialRotationOffset);

        System.TimeSpan dt = System.DateTime.Now - homeStar.discoveryTime;

        transform.RotateAround(parentBody.transform.position, Vector3.forward, -orbitSpeed*(float) dt.TotalSeconds);

        // Set Color
        if (planetNum <= 2)
        {
            _layeredSprite.Randomize((uint) localRNG.Next(), ref localRNG, "red");
        }
        else if (planetNum > 2 & planetNum < 4)
        {
            _layeredSprite.Randomize((uint) localRNG.Next(), ref localRNG, "green");
        }
        else
        {
            _layeredSprite.Randomize((uint) localRNG.Next(), ref localRNG, "blue");
        }

        // Draw orbit path (same color as planet)
        orbitPath = GetComponent<LineRenderer>();
        orbitPath.materials[0].color = Color.gray;

        // Draw the orbit.
        DrawOrbit();

        // Randomize Moons
        /* add moons after
        if (orbitParent.tag == SystemStar.TAG && Random.value <= MOON_CHANCE)
        {
            int numMoons = Random.Range(1, MAX_MOONS + 1);
            BuildMoons(numMoons);
        }
        */
    }

    public void SetWaypoint(string wpName) {
    	if (wpName == "spacebux") {
    		CurrentWaypoint = CollectSpacebuxxxxxxx_Prefab;
    	}
    	if (wpName == null) {
    		CurrentWaypoint = null;
    	}
    }
	
	void Update ()
    {
	    transform.RotateAround (parentBody.transform.position, Vector3.forward, -orbitSpeed * Time.deltaTime);
	    if (CurrentWaypoint != null) {
	    	CurrentWaypoint.transform.rotation = Quaternion.identity;
	    }
    }

	void DrawOrbit ()
    {
		float radius = Vector3.Distance(parentBody.transform.position,transform.position);
		orbitPath.SetVertexCount(ORBIT_PATH_SEGMENTS + 1);
		float deltaTheta = (2.0f *  Mathf.PI) / ORBIT_PATH_SEGMENTS;
		float theta = 0;

		// For each segment (+1, to complete the circle)
		for (int i = 0; i < ORBIT_PATH_SEGMENTS + 1; i++)
        {
			// Do some trigonometry (I dun get it)
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);
			// account for the solar system's position
			Vector3 pos = new Vector3(x, y, 0) + parentBody.transform.position;
            orbitPath.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}

public enum PlanetType
{
    M, // earth
    Y, // hot
    D, // Rock
}
