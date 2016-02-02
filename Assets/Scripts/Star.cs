using UnityEngine;
using System.Collections;

public class Star : PlanetaryBody {
    // Add specific star properties

    public uint myNumber;

    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        myNumber = Procedural.GetNumber(Procedural.PointToNumber((int)transform.position.x, (int)transform.position.y));
        Generate();
    }

    private void Generate()
    {
        Size = 1f;
        _layeredSprite.Randomize(myNumber);
    }
}
