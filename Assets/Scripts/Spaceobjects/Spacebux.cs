using UnityEngine;
using System.Collections;

public class Spacebux : Resource {
    
    protected bool _ready;

    // Use this for initialization
    protected override void Start()
    {
        _amountIncrease = 1;
        _ready = false;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.Spacebux;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (_planet.transform.position.y > 0 && (int)_planet.transform.position.x == 0)
        {
            _ready = true;
            GetComponent<Planet>().SetWaypoint("spacebux");
        }        
    }
    
    public override void Gather()
    {
        if (_ready) {
            //capacity += _amountIncrease;
            //PlayerData.instance.Spacebux+= _amountIncrease;
            NetworkManager.instance._controller.CollectSpacebux(1);
            _ready = false;
            GetComponent<Planet>().SetWaypoint(null);
        }
        else {
            Debug.Log("Can't gather spacebux from " + _planet.name + " yet!");
        }
    }

}
