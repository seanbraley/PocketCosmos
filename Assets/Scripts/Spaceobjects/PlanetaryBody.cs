using UnityEngine;
using System.Collections;

/// <summary>
/// Generic class which holds information about planetary bodies and controls rotation / orbit?
/// </summary>
public class PlanetaryBody : MonoBehaviour {

    public static int MAX_ROTATION_SPEED = 40;
    public static int MIN_ROTATION_SPEED = 1;

    public PlanetaryBody parentBody;

    // Atttributes relevant to orbit
    protected float _rotationSpeed = 30; // Degrees / Second, speed of planet's own rotational cycle
    protected float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.
    protected float _radius = 10;  // radius of planet's orbit around parent star
    protected float _angularSpeed = 1; // speed of rotation around parent star

    protected LayeredSprite _layeredSprite;

    // Attributes relevant to resources
    protected int _progress = 1;
    protected int _energy = 0;
    protected int _population = 0;
    protected int _spacebuxxx = 0;  // yus

    protected System.Random localRNG;  // Every planetary body has one of these

    protected GameObject _owner = null;   // who owns this planet
    public GameObject Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    protected GameObject _orbits = null;   // what this planetary body orbits
    public GameObject Orbits
    {
        get { return _orbits; }
        set {  _orbits = value;  }
    }

    protected float _size = 1; // Units
	public float Size {
		get { return _size; }
		set {
			_size = value;
			transform.localScale = new Vector3(_size,_size,1);
		}
	}

    public GameObject Halo_Prefab;
    protected GameObject _halo;


    public void ShowHalo(bool show) {
        if (show) {
            DestroyHalo();
            _halo = Instantiate(Halo_Prefab,transform.position+(Vector3.forward * 5),Quaternion.identity) as GameObject;
            _halo.transform.localScale = transform.localScale * 1.25f;
            _halo.GetComponent<SpriteRenderer>().sortingOrder = -5;
            _halo.transform.parent = this.transform;
        }
        else {
            DestroyHalo();
        }
    }
    public void DestroyHalo() {
        if (_halo) {        
            Destroy(_halo.gameObject);
            _halo = null;
        }
    }
    public void SetHaloColor(Color c) {
        _halo.GetComponent<SpriteRenderer>().color = c;
    }

    /// <summary>
    /// Start method will get the layer sprite
    /// </summary>
    protected virtual void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        _layeredSprite.LoadSprites();
    }

    public void SetUpRNG(uint i)
    {
        localRNG = new System.Random((int)i);
    }

    /// <summary>
    /// Sets Rotation speed and direction
    /// </summary>
    public void SetBasicFeatures()
    {
        // Set basic attributes (ie rotation)
        _rotationSpeed = localRNG.Next(MIN_ROTATION_SPEED, MAX_ROTATION_SPEED);
        _rotationDirection = (localRNG.NextDouble() > .5) ? 1 : -1;
    }

    /// <summary>
    /// This should be implemented in each subclass
    /// </summary>
    /// <param name="i"></param>
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
	void Update ()
    {
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}

}
