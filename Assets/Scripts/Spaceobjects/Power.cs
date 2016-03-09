using UnityEngine;
using System.Collections;

public class Power : Resource {

    // Use this for initialization
    protected override void Start () {
        _capacity = 2;
        _amountIncrease = 0;
        _planet = this.gameObject.GetComponent<Planet>();
        _resourceType = Utility.ResourceType.Power;
    }

    // Update is called once per frame
    protected override void Update () {
	
	}
}
