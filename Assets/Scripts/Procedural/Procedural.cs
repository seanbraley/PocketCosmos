using UnityEngine;
using System.Collections;

public static class Procedural
{
    private static XXHash randomHash = new XXHash(12345);


    public static uint PointToNumber(int x, int y)
    {
        return CantorPairing(PositiveMapping(x), PositiveMapping(y));
    }

    private static uint CantorPairing(uint f1, uint f2)
    {
        return ((f1 + f2) * (f1 + f2 + 1) / 2 + f2);
    }

    private static uint PositiveMapping(int n)
    {
        if (n >= 0)
            return System.Convert.ToUInt32(n * 2);
        else
            return System.Convert.ToUInt32(-(n * 2 - 1));
    }

    public static Color GetColor(ushort i)
    {
        int r = i % 255;
        int g = (int)(i / 2f % 255);
        int b = (int)(i * 2f % 255);
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public static Color GetColor(uint i)
    {
        uint r = i % 255;
        int g = (int)(i / 2 % 255);
        int b = (int)(i * 2 % 255);
        return new Color(r, g, b);
    }

    public static uint GetNumber(uint input)
    {
        return randomHash.GetHash(input);
    }

    public static Color GetRandomStarColor(ushort i)
    {
        float rand = Random.Range(0f, 1f);
        float choice = i % 100 / 100f;
        int r = 0;
        int g = 0;
        int b = 0;

        if (choice > .75) // RedGreen star
        {
            Debug.Log("Red star");
            r = 255 - (i % (255 / 20));
            g = 255 - ((i * 2) % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else if (choice > .5) // not
        {
            Debug.Log("Not Red star");
            r = 255 - (i % (255 / 20));
            g = 0 + ((i / 2) % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else // all
        {
            Debug.Log("Other");
            r = 255 - (i % (255 / 20));
            g = 255 - ((i * 2) % (255 / 20));
            b = 255 - ((i / 2) % (255 / 20));
        }
        return new Color(r/255f, g/255f, b/255f);
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
    /*
    public static void SetPlanetaryBody(ref PlanetaryBody b, uint i)
    {
        // Speed
        b.SetRotationSpeed((i % 40));

        // Rotation Direction
        if (i % 2 == 0)
            b.SetRotationDirection(RotationDirection.Clockwise);
        else
            b.SetRotationDirection(RotationDirection.CounterClockwise);

        // Size
        b.SetSize(((i % 10) / 10.0f) + .5f);

        // Sprites
        // After
    }
    */
}

public enum RotationDirection
{
    Clockwise = 1,
    CounterClockwise = -1
}


