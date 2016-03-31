using System;
using UnityEngine;
using System.Collections;

public class Population : Resource {
    
    protected bool _ready;
    protected double _orbitperiod;
    // Use this for initialization
    protected override void Start () {
        _ready = false;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.People;
        _amountIncrease = Mathf.RoundToInt((float)_planet.populationRate);
        _orbitperiod = 360 / _planet.orbitSpeed;
    }

    // Update is called once per frame
    protected override void Update () {
        if ((DateTime.Now - _planet.lastPopulationIncrease).TotalSeconds > _orbitperiod)
        {
            _capacity += _amountIncrease;
            Debug.Log(_planet.name + " population is now " + _capacity); //testing
            //GetComponent<Planet>().SetWaypoint("spacebux");
        }
    }    
    

}
