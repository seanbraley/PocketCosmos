using UnityEngine;
using System.Collections;

public class Spacebux : Resource {

    protected float _speed;
    protected float _progress;
    protected bool _ready;


    // Use this for initialization
    protected override void Start()
    {
        _amountIncrease = 1;
        _progress = 0f;
        _ready = false;
        _planet = this.gameObject.GetComponent<Planet>();
        _speed = _planet.orbitSpeed;
        _resourceType = Utility.ResourceType.Spacebux;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (_planet.transform.position.y > 0 && (int)_planet.transform.position.x == 0)
        {
            _progress = 0f;
            _ready = true;
            GetComponent<Planet>().SetWaypoint("spacebux");
        }
        /*
        _progress += UpdateResourceProgress();
        if (_progress >= 100)
        {
            _progress = 0f;
            _ready = true;
            GetComponent<Planet>().SetWaypoint("spacebux");
        }
        */
    }

    protected override float UpdateResourceProgress()
    {
        // TODO: calculate progress based on orbit speed and path (0 - 100%)
        return _speed * Time.deltaTime;
    }

    public override void Gather()
    {
        if (_ready) {
            capacity += _amountIncrease;
            PlayerData.playdata.Spacebux+= _amountIncrease;
            _ready = false;
            GetComponent<Planet>().SetWaypoint(null);
        }
        else {
            Debug.Log("Can't gather spacebux from " + _planet.name + " yet!");
        }
    }

}
