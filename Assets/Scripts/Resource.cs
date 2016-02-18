using UnityEngine;
using System.Collections;
using System;

public class Resource : MonoBehaviour {

    //Public variables
    public float capacity;

    //Variables accessible by subclass
    protected float amountIncrease;
    protected Utility.ResourceType resourceType;
    protected float _progress;
    protected bool _ready;
    
    /*** Game Engine methods, all can be overridden by subclass ***/
    // Use this for initialization
    void Start()
    {
        amountIncrease = 1f;
        capacity = 0f;
        resourceType = Utility.ResourceType.Unknown;
        _progress = 0f;
        _ready = false;
        // TODO: should load saved data on start
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceProgress();
    }

    /*** Public methods ***/

    public void Gather()
    {
        if (_ready) {
            // TODO Gather resource
            Debug.Log("Gathered smth.");
            _ready = false;
        }
        Debug.Log("Not ready to gather yet.");
    }    

    public Utility.ResourceType GetResourceType()
    {
        return resourceType;
    }

    // TODO implement
    void UpdateResourceProgress()
    {
        int i = 0;
        while (i < 30) {
            i++;
        }
        if (i == 30) {
            _ready = true;
            i = 0;
        }
    }

}
