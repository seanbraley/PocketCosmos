using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetaryBody : MonoBehaviour {

	private float _rotationSpeed = 30; // Degrees / Second
	private float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.

    private XXHash randomHash = new XXHash(12345);

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
	void Start () {
		_layeredSprite = GetComponent<LayeredSprite>();
		Size = _size;
        int m = 1000;
        List<int> intlist = new List<int>();
        for (int i = -m; i < m; i++)
        {
            for (int j = -m; j < m; j++)
            {
                if (i != 0 && j != 0)
                {
                    //Debug.Log(string.Format("<{0},{1}> => {2}", i, j, PointToNumber(i, j)));
                    intlist.Add(PointToNumber(i, j));
                }
            }
        }
        intlist.Sort();
        int prevNumber = 0;
        bool duplicates = false;
        foreach (int i in intlist)
        {
            if (i == prevNumber)
                duplicates = true;
            prevNumber = i;
        }
        if (duplicates)
            Debug.Log("There were duplicates :(");
        else
            Debug.Log("A-OK Captain!");
    }

	public void Randomize() {
		_rotationSpeed = Random.Range(0f,40f);
		Size = Random.Range(0.5f,1.5f);
		if (Random.Range(0,2) == 1) {
			_rotationDirection = 1;
		}
		else {
			_rotationDirection = -1;
		}
		_layeredSprite.Randomize();
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
			Randomize();
            Debug.Log(randomHash.GetHash(1));
            Debug.Log(randomHash.GetHash(2));
            Debug.Log(randomHash.GetHash(3));
        }
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}
}
