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
			_spriteBase.color = value;
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
		//SetColors(new Color(0.1f,0.6f,0.3f),new Color(0.6f,0.3f,0.9f));
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

    public void RandomizeSectorStar(System.Random sourceRNG)
    {
        Color c1;
        Color c2;
        switch (sourceRNG.Next(1,2))
        {
            case 1:
                c1 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 150) / 255f,
                    sourceRNG.Next(0, 150) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 150) / 255f,
                    sourceRNG.Next(0, 150) / 255f
                );
                break;

            case 2:
                c1 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 200) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 200) / 255f
                );
                break;
            default:
                c1 = new Color(
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f
                );
                break;
        }
        SetColors(Color.white, Color.white);
    }

    public void Randomize(uint i) {
		RandomizeColors(i);
		RandomizeShowLayers(i);
	}

    public void Randomize(uint i, ref System.Random sourceRNG, string favor)
    {
        RandomizeColors(i, ref sourceRNG, favor);
        RandomizeShowLayers(i);
    }

	public void LoadSprites() {
		foreach(Transform t in transform) {
			if (t.name == "SpriteBase") {
				_spriteBase = t.GetComponent<SpriteRenderer>();
			}
			else if (t.name == "SpriteLayers") {
				_spriteLayers = t.GetComponentsInChildren<SpriteRenderer>();
			}
		}
	}

	public void SetColors(Color baseColor, Color layerColor)
    {
        BaseColor = baseColor;
		LayerColor = layerColor;
        //Debug.Log("Base color");
        //Debug.Log(baseColor);
	}

    /// <summary>
    /// Randomize colors for planet
    /// </summary>
    /// <param name="first">first color</param>
    /// <param name="second">second color</param>
    /// <param name="order">which is the planet</param>
    private void RandomizeColors(int first, int second, int order)
    {
        SetColors(Procedural.GetRandomColor((ushort)first, order), Procedural.GetRandomColor((ushort)second, order));
    }

    /// <summary>
    /// Only called for stars
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    private void RandomizeColors(int first, int second)
    {
        SetColors(Procedural.GetRandomStarColor((ushort)first), Procedural.GetRandomStarColor((ushort)second));
    }

	private void RandomizeColors(uint i) {
		if (!isStar) {
            // need to get two random numbers from this
            byte[] org = System.BitConverter.GetBytes(i);
            ushort first = System.BitConverter.ToUInt16(org, 0);
            ushort second = System.BitConverter.ToUInt16(org, 2);
            SetColors(Utility.GetRandomColor(first), Utility.GetRandomColor(second));
		}
		else
        {
            byte[] org = System.BitConverter.GetBytes(i);
            ushort first = System.BitConverter.ToUInt16(org, 0);
            ushort second = System.BitConverter.ToUInt16(org, 2);
            //SetColors(Procedural.GetRandomColor(first), Procedural.GetRandomColor(second));
            SetColors(Procedural.GetRandomStarColor(first), Procedural.GetRandomStarColor(second));
		}
	}

    private void RandomizeColors(uint i, ref System.Random sourceRNG, string favor)
    {
        Color c1;
        Color c2;
        switch (favor)
        {
            case "red":
                c1 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 150) / 255f,
                    sourceRNG.Next(0, 150) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 150) / 255f,
                    sourceRNG.Next(0, 150) / 255f
                );
                break;

            case "green":
                c1 = new Color(
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 200) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(100, 255) / 255f,
                    sourceRNG.Next(0, 200) / 255f
                );
                break;

            case "blue":
                c1 = new Color(
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(100, 255) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(0, 200) / 255f,
                    sourceRNG.Next(100, 255) / 255f
                );
                break;
            default:
                c1 = new Color(
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f
                );
                c2 = new Color(
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f,
                    sourceRNG.Next(0, 255) / 255f
                );
                break;
        }
        SetColors(c1, c2);
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

	private void RandomizeShowLayers(uint i) {

        // N layers to show
        // currently generates boolean for each layer
        //

        BitArray b = new BitArray(new int[] { (int)i });

        if (b.Length < _spriteLayers.Length)
            Debug.Log("ERROR TOO MANY LAYERS");

        for (int j = 0; j < _spriteLayers.Length; j++)
        {
			ShowLayer(j, b[j]);
		}
	}
}
