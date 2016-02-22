using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    // Set these in the inspector
    //public Text populationNumber;
   // public Text powerNumber;
    public Text spacebuxNumber;

	// Use this for initialization
	void Start () {
        // TO DO literally just copy and paste this for population and power
        spacebuxNumber.text = PlayerData.playdata.Spacebux.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        // TO DO make this less spammy maybe idgaf
        spacebuxNumber.text = PlayerData.playdata.Spacebux.ToString();
    }
}
