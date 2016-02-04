using UnityEngine;
using System.Collections;

// TO-DO : fill this in later.
public class NetworkManager : MonoBehaviour {

    public static NetworkManager instance = null;              //Static instance of NetworkManager which allows it to be accessed by any other script.

    void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
