using UnityEngine;
using System.Collections;

public class Resource : PlanetaryBody {
    
    //Public variables
    public float available;              // how much of this resource is avaiable right now
    public float amountGenerated;      // resource amount generated per cycle. can be positive, negative or 0
    public Utility.ResourceType _resourceType;

    protected Time _lastGathered;       // time of last gathered
    private float _progress;            // percent of cycle completed - ready to gather at 100%
    private bool _OK = false;
    

    // Use this for initialization
    void Start()
    {
        _resourceType = Utility.ResourceType.Unknown;
        available = 0;
        _progress = 0;
    }
    
    void FixedUpdate()
    {
        CalculateProgress();
        if (_progress >= 100) {
            _OK = true;     // good to gather
            Debug.Log("Resource ready now.");
        }

    }


    // ----- Accessors -----
    public Utility.ResourceType GetResourceType() {
        return _resourceType;
    }
    
    public Time GetLastGatheredTime() {
        return _lastGathered;
    }
    
    public float GetProgress() {
        return _progress;
    }


    // ----- Functions ------
    protected void CalculateProgress() {
        // Calculate progress based on rotation speed and modifiers
        _progress += 0.5f;
    }


    // ----- Public functions -----
    public void Gather() {
        if (_OK)
        {
            // Update the server when gathering
            Debug.Log("Gathered " + amountGenerated);
            available += amountGenerated;
            _OK = false;  // reset
            _progress = 0;
        }
        else {
            // Bring up a menu or something.
            Debug.Log("Not ready to gather yet. ");
        }
        
    }


    // ----- Override functions -----

 
}
