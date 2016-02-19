using UnityEngine;
using System.Collections;

public class Population : Resource {

    protected float _speed;
    protected float _progress;
    protected bool _ready;


    // Use this for initialization
    protected override void Start () {
        _amountIncrease = 1f;
        _progress = 0f;
        _ready = false;
        _planet = this.gameObject.GetComponent<Planet>();
        _speed = _planet.orbitSpeed;
        _resourceType = Utility.ResourceType.People;
    }

    // Update is called once per frame
    protected override void Update () {
        _progress += UpdateResourceProgress();
        if (_progress >= 100) {
            capacity += _amountIncrease;
            Debug.Log(_planet.name + " population is now " + capacity); //testing
            _progress = 0f;
        }
    }    

    protected override float UpdateResourceProgress()
    {
        // TODO: calculate progress based on orbit speed and path (0 - 100%)
        return _speed * Time.deltaTime;
    }

}
