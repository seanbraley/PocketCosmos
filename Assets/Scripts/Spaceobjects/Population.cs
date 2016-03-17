using UnityEngine;
using System.Collections;

public class Population : Resource {
    
    protected bool _ready;

    // Use this for initialization
    protected override void Start () {
        _amountIncrease = 1;
        _ready = false;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.People;
    }

    // Update is called once per frame
    protected override void Update () {
        if (_planet.transform.position.y > 0 && (int)_planet.transform.position.x == 0)
        {
            //_capacity += _amountIncrease;
            Debug.Log(_planet.name + " population is now " + _capacity); //testing
            //GetComponent<Planet>().SetWaypoint("spacebux");
        }
    }    
    

}
