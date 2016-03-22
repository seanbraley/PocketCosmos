using UnityEngine;
using System.Collections;

public class Power : Resource {

    // Use this for initialization
    protected override void Start () {
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.Power;
        _capacity = Mathf.RoundToInt((float)_planet.energyModifier * _planet.homeStar.baseEnergyLevel);
    }

    // Update is called once per frame
    protected override void Update () {
	
	}
}
