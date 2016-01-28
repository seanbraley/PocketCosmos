using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Color GetRandomColor() {
		return new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
	}

    public static Color GetRandomColor(uint i)
    {
        uint r = i % 255;
        int g = (int)(i / 2f % 255);
        int b = (int)(i * 2f % 255);
        return new Color(r, g, b);
    }

	public static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
	    float red = (float)color.r;
	    float green = (float)color.g;
	    float blue = (float)color.b;

	    red *= correctionFactor;
	    green *= correctionFactor;
	    blue *= correctionFactor;

	    return new Color(red,green,blue);
	}
}
