using UnityEngine;
using System.Collections;

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
	public LineRenderer orbitPath;
	public GameObject orbitParent;
	public SystemStar homeStar;
	public Planet homePlanet;
	//public float rotationSpeed;
	public GameObject[] moons;

    public int planetNum;

	private Renderer renderer;

    private double rotationDistance;

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

	/*~*~*~*~*~*~*~*~*~*~*~* Initialization *~*~*~*~*~*~*~*~*~*~*~*/
	/* All code under this subheading will be called once, at the
	 * beginning of the game session.
	 */

	void Start ()
    {
        //Initialize();
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

        // Randomize Location around orbit parent (for illusion of time passing)
        // Consider actually counting the amount of time the user has been away.
        transform.position = (transform.position - parentBody.transform.position).normalized * ORBIT_CONSTANT * planetNum + parentBody.transform.position;
        transform.RotateAround(parentBody.transform.position, Vector3.forward, localRNG.Next(0, 360));

        // Set Color
        if (planetNum <= 2)
        {
            _layeredSprite.Randomize((uint)localRNG.Next(), ref localRNG, "red");
        }
        else if (planetNum > 2 & planetNum < 4)
        {
            _layeredSprite.Randomize((uint)localRNG.Next(), ref localRNG, "green");
        }
        else
        {
            _layeredSprite.Randomize((uint)localRNG.Next(), ref localRNG, "blue");
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

    /* void Initialize ():
	 * 		For initial creation of planets in a new game.
	 * 		Randomizes:
	 * 			Rotation (Length of Day)
	 * 			Size (The size of which depends on whether it is a planet or a moon)
	 * 			Color

	public void Initialize ()
    {

		renderer = GetComponent<Renderer>();
		//Randomize Rotation Speed
        
		float rot = Random.Range (MIN_ROTATION, MAX_ROTATION);
		//Set Size
		float size = 1;
		// If it is a planet
		if (orbitParent.tag == SystemStar.TAG) {
			// Randomize size dependent on set constants
			size = Random.Range (MIN_SIZE, MAX_SIZE);
			// Set home star, there is no home planet.
			homeStar = orbitParent.GetComponent<SystemStar>();
		}
		// If it is a moon
		else if (orbitParent.tag == TAG) {
			// Randomize size dependent on size of home planet
			size = Random.Range (orbitParent.transform.localScale.x/2,orbitParent.transform.localScale.x/2);
			// Set home star AND home planet.
			homePlanet = orbitParent.GetComponent<Planet>();
			homeStar = homePlanet.orbitParent.GetComponent<SystemStar>();
		}
		// Randomize Color
		float r = Random.Range (MIN_COLOR, MAX_COLOR);
		float g = Random.Range (MIN_COLOR, MAX_COLOR);
		float b = Random.Range (MIN_COLOR, MAX_COLOR);
		Color color = new Color (r, g, b);
		Build (rot,size,color);
		// Randomize Moons
		if (orbitParent.tag == SystemStar.TAG && Random.value <= MOON_CHANCE) {
			int numMoons = Random.Range (1,MAX_MOONS+1);
			BuildMoons(numMoons);
		}
	}
    	 */
    /*
    public void Initialize(ref System.Random starRNG, int planetNum)
    {
        renderer = GetComponent<Renderer>();
        //Randomize Rotation Speed

        float rot = starRNG.Next(MIN_ROTATION, MAX_ROTATION);
        //Set Size
        float size = 1;
        // If it is a planet
        if (orbitParent.tag == SystemStar.TAG)
        {
            // Randomize size dependent on set constants
            size = starRNG.Next(MIN_SIZE, MAX_SIZE);
            // Set home star, there is no home planet.
            homeStar = orbitParent.GetComponent<SystemStar>();
        }
        // If it is a moon
        else if (orbitParent.tag == TAG)
        {
            // Randomize size dependent on size of home planet
            size = starRNG.Next((int)orbitParent.transform.localScale.x / 2, (int)orbitParent.transform.localScale.x / 2);
            // Set home star AND home planet.
            homePlanet = orbitParent.GetComponent<Planet>();
            homeStar = homePlanet.orbitParent.GetComponent<SystemStar>();
        }

        Color color;

        // Randomize Color


        //Set Rotation Speed
        rotationSpeed = rot;
        //Set Planet Size
        transform.localScale = new Vector3(size, size, size);
        //Orbit Speed is a function of distance from orbit parent (whether it be a planet or star)
        orbitSpeed = ORBIT_CONSTANT / Vector3.Distance(orbitParent.transform.position, transform.position);
        //Randomize Location around orbit parent (for illusion of time passing)
        //Consider actually counting the amount of time the user has been away.
        transform.RotateAround(orbitParent.transform.position, Vector3.back, Random.Range(0, 360));
        //Set Color
        //renderer.material.color = color;
        GetComponent<LayeredSprite>().Randomize((uint)starRNG.Next());

        //Draw orbit path (same color as planet)
        orbitPath = GetComponent<LineRenderer>();
        orbitPath.materials[0].color = Color.gray;
        // Draw the orbit.
        DrawOrbit();

        // Randomize Moons
        if (orbitParent.tag == SystemStar.TAG && Random.value <= MOON_CHANCE)
        {
            int numMoons = Random.Range(1, MAX_MOONS + 1);
            BuildMoons(numMoons);
        }
    }

    /* void Build (float rotation, float size, Color color)
	 * 		For creation of a planet according to saved data.
	 * 		Also used to implement the random values created by Initialize ();
	 */
     /*
    public void Build (float rotation, float size, Color color) {
		//Set Rotation Speed
		rotationSpeed = rotation;
		//Set Planet Size
		transform.localScale = new Vector3 (size, size, size);
		//Orbit Speed is a function of distance from orbit parent (whether it be a planet or star)
		orbitSpeed = ORBIT_CONSTANT / Vector3.Distance (orbitParent.transform.position, transform.position);
		//Randomize Location around orbit parent (for illusion of time passing)
		//Consider actually counting the amount of time the user has been away.
		transform.RotateAround (orbitParent.transform.position, Vector3.back, Random.Range(0,360));
		//Set Color
		renderer.material.color = color;
		//Draw orbit path (same color as planet)
		orbitPath = GetComponent<LineRenderer>();
		orbitPath.materials [0].color = Color.gray;
		// Draw the orbit.
		DrawOrbit ();
	}

	/* void BuildMoons (int numMoons)
	 * 		Builds sub-planets to orbit this planet.
	 * 		Randomly generates the planets using the Instantiate() method.
	 */
	void BuildMoons(int numMoons) {
		moons = new GameObject[numMoons];
		for (int i = 0; i < numMoons; i++)
        {
			Vector3 planetPos = transform.position;
			// Position the moon based on the size of the planet.
			planetPos += new Vector3 ((transform.localScale.x) + (1+i) * transform.localScale.x/16,0,0);
			moons[i] = Instantiate(this.gameObject,planetPos,Quaternion.identity) as GameObject;
			moons[i].GetComponent<Planet>().SetOrbitParent(this.gameObject);
			moons[i].name = gameObject.name + System.Convert.ToChar (97+i);
		}
	}

	/* SetOrbitParent (GameObject parent)
	 * 		Set Orbit Parent Manually (for use by the parent itself)
	 * 		Investigate if there is a cleaner way to do this.
	 */
	public void SetOrbitParent(GameObject parent) {
		orbitParent = parent;
	}



	/*~*~*~*~*~*~*~*~*~*~*~* Updating *~*~*~*~*~*~*~*~*~*~*~*/
	/* All code under this subheading will be called on a
	 * per-frame basis. Maybe not all frames, but most.
	 */
	
	void Update ()
    {
	    transform.RotateAround (parentBody.transform.position, Vector3.forward, -orbitSpeed * Time.deltaTime);
	    if (CurrentWaypoint != null) {
	    	CurrentWaypoint.transform.rotation = Quaternion.identity;
	    }
    }

	/* void HandleMovement ()
	 * 		Do everything related to planetary movement:
	 * 			Rotate
	 * 			Orbit around home star
	 * 			Orbit around home planet (if moon)
	 * 			Redraw the orbit path (see DrawOrbit())
	 */
	void HandleMovement () {
		// Only orbit if camera is zoomed in (for optimization)
		if (Camera.main.orthographicSize < 1000) {
			// if the planet's orbit path is within view
			if (orbitPath.isVisible) {
				// Orbit the parent.
				transform.RotateAround (parentBody.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
				// Rotate about planetary axis
				//transform.Rotate (Vector3.back, rotationSpeed * Time.deltaTime);  // This is handled in the base class
			}
            /*
			// if the planet is a moon we must also orbit the host star.
			if (orbitParent.tag == Planet.TAG) {
				// But only if the home planet is currently orbiting.
				if (homePlanet.orbitPath.isVisible) {
					float parentOrbitSpeed = homePlanet.orbitSpeed;
					GameObject star = homeStar.gameObject;
					transform.RotateAround (star.transform.position, Vector3.back, parentOrbitSpeed * Time.deltaTime);
				}
				//Redraw Orbit Path (for zooming and moons)
				DrawOrbit ();
			}
            */
		}
		// we must always handle for zooming in and out.
		orbitPath.SetWidth(ORBIT_PATH_WIDTH * Camera.main.orthographicSize, ORBIT_PATH_WIDTH * Camera.main.orthographicSize);
	}

	/* void DrawOrbit ()
	 * 		Draw the planet's orbit path. Only necessary to call once if it 
	 * 		is a planet, but if it is a moon it must be drawn every frame.
	 */
	void DrawOrbit () {
		float radius = Vector3.Distance(parentBody.transform.position,transform.position);
		orbitPath.SetVertexCount(ORBIT_PATH_SEGMENTS + 1);
		float deltaTheta = (2.0f *  Mathf.PI) / ORBIT_PATH_SEGMENTS;
		float theta = 0;

		// For each segment (+1, to complete the circle)
		for (int i = 0; i < ORBIT_PATH_SEGMENTS + 1; i++) {
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
