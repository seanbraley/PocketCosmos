using UnityEngine;
using System.Collections;

public static class Procedural
{


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
}

public enum RotationDirection
{
    Clockwise = 1,
    CounterClockwise = -1
}


