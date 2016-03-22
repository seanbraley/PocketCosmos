using UnityEngine;
using System.Collections;
using System;

public class Resource : MonoBehaviour {

    //Public variables

    //Variables accessible by subclass
    protected int _capacity;
    protected int _amountIncrease;
    protected Utility.ResourceType _resourceType;
    protected Planet _planet;
    /*** Game Engine methods, all can be overridden by subclass ***/

    // Use this for initialization
    //These methods are virtual and thus can be overriden in child classes
    protected virtual void Start() {
        _capacity = 0;
        _amountIncrease = 0;
        _resourceType = Utility.ResourceType.Unknown;
    }

    // Update is called once per frame
    protected virtual void Update() {
    }

    /*** Public methods ***/

    public virtual void Gather() {        
    }    

    public virtual Utility.ResourceType GetResourceType() {
        return _resourceType;
    }
    

}
