using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetaryBody : MonoBehaviour {

	private float _rotationSpeed = 30; // Degrees / Second
	private float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.
    
    private XXHash randomHash = new XXHash(12345);

    private int _progress = 1;
    
    private GameObject _orbits = null;   // what this planetary body orbits
    public GameObject Orbits
    {
        get
        {
            return _orbits;
        }
        set
        {
            _orbits = value;
        }
    }

    private float _size = 1; // Units
	public float Size {
		get {
			return _size;
		}
		set {
			_size = value;
			transform.localScale = new Vector3(_size,_size,1);
		}
	}

	private LayeredSprite _layeredSprite;

    // Use this for initialization
    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        Randomize(0u);
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

        Size = (((i % 10) / 10.0f) + .5f) * 5;
        Debug.Log("Size: " + Size);

        _layeredSprite.Randomize(i);
    }

    public int PointToNumber(int x, int y)
    {
        return CantorPairing(PositiveMapping(x), PositiveMapping(y));
    }

    private int CantorPairing(int f1, int f2)
    {
        return ((f1 + f2)*(f1 + f2 + 1) / 2 + f2);
    }

    private int PositiveMapping(int n)
    {
        if (n <= 0)
            return n * 2;
        else
            return -n * 2 - 1;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Debug.Log(Nomenclature.GetRandomWord());
			Randomize(randomHash.GetHash(_progress++));
        }
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}
}
