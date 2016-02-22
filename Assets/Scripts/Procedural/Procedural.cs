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
        float choice = i % 100 / 100f;
        int r = 0;
        int g = 0;
        int b = 0;

        if (choice > .75) // RedGreen star
        {
            //Debug.Log("Red star");
            r = 255 - (i % (255 / 20));
            g = 255 - ((i * 2) % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else if (choice > .5) // not
        {
            //Debug.Log("Not Red star");
            r = 255 - (i % (255 / 20));
            g = 0 + ((i / 2) % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else // all
        {
            //Debug.Log("Other");
            r = 255 - (i % (255 / 20));
            g = 255 - ((i * 2) % (255 / 20));
            b = 255 - ((i / 2) % (255 / 20));
        }
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static Color GetRandomColor(ushort i, int order)
    {
        int r;
        int g;
        int b;

        if (order <= 2)  // Favour reds
        {
            r = 255 - (i % (255 / 20));
            g = 255 - ((i * 2) % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else if (order > 2 & order <= 4)  // Favour greens 
        {
            r = 255 - ((i * 2) % (255 / 20));
            g = 255 - (i % (255 / 20));
            b = 0 + ((i / 2) % (255 / 10));
        }
        else  // Favour blues
        {
            r = 0 + ((i / 2) % (255 / 10));
            g = 255 - ((i * 2) % (255 / 20));
            b = 255 - (i % (255 / 20));
        }
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static Color GetRandomColor(ushort i)
    {
        //Debug.Log("Getting random colour from number: " + i);
        int r = i % 255;
        int g = (int)(i / 2f % 255);
        int b = (int)(i * 2f % 255);
        //Debug.Log(System.String.Format("RGB: {0}, {1}, {2}", r, g, b));
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public static Color GetRandomColor(uint i)
    {
        //Debug.Log("Getting random colour from seed");
        uint r = i % 255;
        int g = (int)(i / 2 % 255);
        int b = (int)(i * 2 % 255);
        return new Color(r, g, b);
    }

    public static bool StarExists(int x, int y)
    {
        if (x%3==0 && y%3==0)
        {
            uint num = PointToNumber(x, y);

            float num2 = num / Mathf.PI;
            float num3 = num2 - Mathf.RoundToInt(num2);
            //Debug.Log(num3);
            return (Mathf.Abs(num3) > 0.46f);
        }
        return false;

        //BitArray b = new BitArray(new int[] { (int)num });

        /* Does not work
        byte[] bytes = System.BitConverter.GetBytes(num);
        char a1 = (char)bytes[0];
        char a2 = (char)bytes[1];
        char a3 = (char)bytes[2];
        char a4 = (char)bytes[3];

        Debug.Log(string.Format("Chars: {0}, {1}, {2}, {3}", a1, a2, a3, a4));
        return (char.IsLetter(a1) || char.IsLetter(a2) || char.IsLetter(a3) || char.IsLetter(a3));
        //Debug.Log(b.ToString());
        */



        //return (Mathf.Pow(num, x) % y == 0) & (Mathf.Pow(num, y) % x == 0);  // This looks nice for the positive quadrant(s)
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


