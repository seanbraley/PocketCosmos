using UnityEngine;
using Completed;
using UnityEngine.UI;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    // Set these in the inspector
    //public Text populationNumber;
   // public Text powerNumber;
    public Text spacebuxNumber;
    public Text virtualPosition;

	// Use this for initialization
	void Start () {
        // TO DO literally just copy and paste this for population and power
        spacebuxNumber.text = PlayerData.playdata.Spacebux.ToString();
        if (virtualPosition != null)
            virtualPosition.text = "Position: <" + Mathf.FloorToInt(GameManager.instance.virtualPosition.x) + " , " + Mathf.FloorToInt(GameManager.instance.virtualPosition.y) + ">";
    }
	
	// Update is called once per frame
	void Update () {
        // TO DO make this less spammy maybe idgaf
        spacebuxNumber.text = PlayerData.playdata.Spacebux.ToString();
        if (virtualPosition != null)
            virtualPosition.text = "Position: <" + Mathf.FloorToInt(GameManager.instance.virtualPosition.x) + " , " + Mathf.FloorToInt(GameManager.instance.virtualPosition.y) + ">";
    }
}
