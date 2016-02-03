using UnityEngine;
using System.Collections;

public class Star : PlanetaryBody {
    // Add specific star properties

    public uint myNumber;

    // TO-DO: get/set?
    private GameObject[] _children;

    void Start()
    {
        _layeredSprite = GetComponent<LayeredSprite>();
        myNumber = Procedural.GetNumber(Procedural.PointToNumber((int)transform.position.x, (int)transform.position.y));
        Generate();
        SetChildren();
    }

    // Procedurally generate the star
    private void Generate()
    {
        Size = 1f;
        _layeredSprite.Randomize(myNumber);
    }

    // TO-DO
    private void SetChildren()
    {
        
    }

}
