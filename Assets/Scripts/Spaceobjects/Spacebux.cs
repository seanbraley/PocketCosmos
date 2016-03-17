using UnityEngine;
using System.Collections;
using System;

public class Spacebux : Resource {
    
    protected bool _ready;
    protected bool _needToUpdate;

    // Use this for initialization
    protected override void Start()
    {
        _amountIncrease = 1;
        _ready = false;
        _needToUpdate = true;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.Spacebux;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Status changed - update resource parameters from local save data
        if (_needToUpdate) {
            _needToUpdate = false;
        }

        if (_planet.transform.position.y > 0 && (int)_planet.transform.position.x == 0)
        {
            _ready = true;
            GetComponent<Planet>().SetWaypoint("spacebux");
        }        
    }
    
    public override void Gather()
    {
        if (_ready) {
            var currTime = DateTime.Now;
            PlayerData.instance.spacebux += _amountIncrease; // update locally first
            NetworkManager.instance._controller.CollectSpacebux(_amountIncrease); // collect spacebux
            //PlayerData.instance.ownedPlanets.Find(x => x.planetID == _planet.myNumber && x.planetID == _planet.planetNum).lastCollectedTime = currTime; // update the local data state
            _ready = false;
            _needToUpdate = true;
            GetComponent<Planet>().SetWaypoint(null);
        }
        else {
            Debug.Log("Can't gather spacebux from " + _planet.name + " yet!");
        }
    }

}
