using UnityEngine;
using System.Collections;

public class PlanetaryBody : MonoBehaviour {

	private float _rotationSpeed = 30; // Degrees / Second
	private float _rotationDirection = 1; // 1 = clockwise, -1 = counterclockwise, 0 = none.

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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Debug.Log(Nomenclature.GetRandomWord());
			Randomize();
		}
		transform.Rotate(new Vector3(0,0, _rotationSpeed * -_rotationDirection * Time.deltaTime));
	}
}
