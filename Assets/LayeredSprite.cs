using UnityEngine;
using System.Collections;

public class LayeredSprite : MonoBehaviour {

	private SpriteRenderer _spriteBase;
	private SpriteRenderer[] _spriteLayers;

	private Color _baseColor;
	public Color BaseColor {
		get {
			return _baseColor;
		}
		set {
			_baseColor = value;
			_spriteBase.color = _baseColor;
		}
	}

	private Color _layerColor;
	public Color LayerColor {
		get {
			return _layerColor;
		}
		set {
			_layerColor = value;
			foreach(SpriteRenderer layer in _spriteLayers) {
				layer.color = _layerColor;
			}
		}
	}

	// Use this for initialization
	void Start () {
		LoadSprites();
		SetColors(new Color(0.1f,0.6f,0.3f),new Color(0.6f,0.3f,0.9f));
	}

	void Update() {

	}

	/* DEBUG ~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~ */

	public bool isStar;

	public static Color GetRandomStarColor() {
		float rand = Random.Range(0f,1f);
		if (rand < 0.5f) {
			return new Color(Random.Range(0.8f,1f),Random.Range(0.8f,1f),Random.Range(0f,0.1f));
		}
		else if (rand < 0.7f) {
			return new Color(Random.Range(0.8f,1f),Random.Range(0f,0.2f),Random.Range(0f,0.1f));
		}
		else {
			return new Color(Random.Range(0.8f,1f),Random.Range(0.8f,1f),Random.Range(0.8f,1f));
		}
	}

	/* End DEBUG ~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~ */

	public void Randomize() {
		RandomizeColors();
		RandomizeShowLayers();
	}

	private void LoadSprites() {
		foreach(Transform t in transform) {
			if (t.name == "SpriteBase") {
				_spriteBase = t.GetComponent<SpriteRenderer>();
			}
			else if (t.name == "SpriteLayers") {
				_spriteLayers = t.GetComponentsInChildren<SpriteRenderer>();
			}
		}
	}

	public void SetColors(Color baseColor, Color layerColor) {
		BaseColor = baseColor;
		LayerColor = layerColor;

	}

	private void RandomizeColors() {
		if (!isStar) {
			SetColors(Utility.GetRandomColor(), Utility.GetRandomColor());
		}
		else
		{
			SetColors(GetRandomStarColor(),GetRandomStarColor());
		}
	}

	private void ShowLayers(bool show) {
		for(int i = 0; i < _spriteLayers.Length; i++) {
			ShowLayer(i,show);
		}
	}

	private void ShowLayers(bool[] show) {
		int index = 0;
		while (index < show.Length) {
			if (index >= _spriteLayers.Length) {
				Debug.LogWarning("Length of array parameter in LayeredSprite.ShowLayers() is too long.");
				return;
			}
			ShowLayer(index, show[index]);
			index++;
		}
		if (index < show.Length) {
			Debug.LogWarning("Length of array parameter in LayeredSprite.ShowLayers() is too short.");
			return;
		}
	}

	private void ShowLayer(int index, bool show) {
		if (index < 0 && index >= _spriteLayers.Length) {
			Debug.LogError("Invalid index.");
			return;
		}
		_spriteLayers[index].gameObject.SetActive(show);
	}

	private void RandomizeShowLayers() {
		for(int i = 0; i < _spriteLayers.Length; i++) {
			bool rand = (Random.Range(0,2) == 1);
			ShowLayer(i,rand);
		}
	}
}
