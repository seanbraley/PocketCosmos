using UnityEngine;
using System.Collections;

public class PlanetaryBody : MonoBehaviour {

    // Atttributes relevant to orbit
	private float _rotationSpeed = 30; // Degrees / Second, speed of planet's own rotational cycle
	private float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.
    private float _radius = 10;  // radius of planet's orbit around parent star
    private float _angularSpeed = 1; // speed of rotation around parent star

    protected LayeredSprite _layeredSprite;

    // Attributes relevant to resources
    private int _progress = 1;
    private int _energy = 0;
    private int _population = 0;
    private int _spacebuxxx = 0;

    private GameObject _owner = null;   // who owns this planet
    public GameObject Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }
    
    private GameObject _orbits = null;   // what this planetary body orbits
    public GameObject Orbits
    {
        get { return _orbits; }
        set {  _orbits = value;  }
    }

    private float _size = 1; // Units
	public float Size {
		get { return _size; }
		set {
			_size = value;
			transform.localScale = new Vector3(_size,_size,1);
		}
	}


    // Use this for initialization
    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        //Randomize(0u);
    }

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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Debug.Log(Nomenclature.GetRandomWord());
			//Randomize(randomHash.GetHash(_progress++));
        }
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}

}
