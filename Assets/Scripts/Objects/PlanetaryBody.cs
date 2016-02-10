using UnityEngine;
using System.Collections;

public class PlanetaryBody : MonoBehaviour {

    // Atttributes relevant to orbit
	protected float _rotationSpeed = 30; // Degrees / Second, speed of planet's own rotational cycle
    protected float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.
    protected float _radius = 10;  // radius of planet's orbit around parent star
    protected float _angularSpeed = 1; // speed of rotation around parent star

    protected LayeredSprite _layeredSprite;

    protected Utility.OwnershipType _owner;   // who owns this planet    
    protected GameObject _orbits;   // what this planetary body orbits
    protected float _size = 1; // Units

    // Atrributes relevant to planet
    protected float _power = 1;


    // Use this for initialization
    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        //  TO-DO: Query database to instantiate planet based on server info
        // If entry not available then create a new database entry for this newly discovered
        _owner = Utility.OwnershipType.Unknown;
        _orbits = null;
        //Randomize(0u);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Debug.Log(Nomenclature.GetRandomWord());
			//Randomize(randomHash.GetHash(_progress++));
        }
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}


    // ----- Public functions -----
    public void Randomize(uint i)
    {
        Debug.Log("Creating planet from number: " + i);
        _rotationSpeed = (i % 40);
        if (i % 2 == 0)
            _rotationDirection = 1;
        else
            _rotationDirection = -1;

        Debug.Log("Rotational Speed: " + _rotationSpeed);

        Size = (((i % 10) / 10.0f) + .5f);
        Debug.Log("Size: " + Size);

        _layeredSprite.Randomize(i);
    }

    // ----- Accessors -----
    public float Size
    {
        get { return _size; }
        set
        {
            _size = value;
            transform.localScale = new Vector3(_size, _size, 1);
        }
    }

    public GameObject Orbits
    {
        get { return _orbits; }
        set { _orbits = value; }
    }

    public Utility.OwnershipType Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public float Power
    {
        get { return _power; }
        set { _power = value; }
    }
}
