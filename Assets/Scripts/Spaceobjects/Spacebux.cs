﻿using UnityEngine;
using System.Collections;
using System;

public class Spacebux : Resource {
    
    protected bool _ready;
    protected bool _needToUpdate;
    protected double _orbitperiod;

    // Use this for initialization
    protected override void Start()
    {
        _ready = false;
        _needToUpdate = true;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.Spacebux;
        _amountIncrease = 1;
        _orbitperiod = 360 / _planet.orbitSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Status changed - update resource parameters from local save data
        if (_needToUpdate) {
            // Update it
            _needToUpdate = false;
        }

        //var elapsedTime = DateTime.Now - PlayerData.instance.ownedPlanets
        if ((DateTime.Now - _planet.lastResourceCollection).TotalSeconds > _orbitperiod)
        {
            _ready = true;
            GetComponent<Planet>().SetWaypoint("spacebux");
        }
    }
    
    public override void Gather()
    {
        if (this.enabled) {
            if (_ready)
            {
                //PlayerData.instance.ownedPlanets.Find(
                //    x => x.starID == (long)_planet.homeStar.myNumber && x.planetID == _planet.planetNum).LastCollectedTime = currTime; // TO DO - update the local data state
                PlayerData.instance.spacebux += _amountIncrease; // update locally first
                NetworkManager.instance._controller.CollectSpacebux(_amountIncrease); // collect spacebux
                _ready = false;
                _needToUpdate = true;
                _planet.lastResourceCollection = DateTime.Now;
                GetComponent<Planet>().SetWaypoint(null);
            }
            else {
                Debug.Log("Can't gather spacebux from " + _planet.name + " yet!");
            }
        }
        
    }

}
