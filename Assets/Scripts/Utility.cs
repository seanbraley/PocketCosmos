using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public enum ResourceType { Power, People, Spacebux, Unknown }

    public enum OwnershipType { Player, Enemy, Unknown }

	public static Color GetRandomColor() {
		return new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
	}

    public static Color GetRandomColor(ushort i)
    {
        Debug.Log("Getting random colour from number: " + i);
        int r = i % 255;
        int g = (int)(i / 2f % 255);
        int b = (int)(i * 2f % 255);
        Debug.Log(System.String.Format("RGB: {0}, {1}, {2}", r, g, b));
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public static Color GetRandomColor(uint i)
    {
        Debug.Log("Getting random colour from seed");
        uint r = i % 255;
        int g = (int)(i / 2 % 255);
        int b = (int)(i * 2 % 255);
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
