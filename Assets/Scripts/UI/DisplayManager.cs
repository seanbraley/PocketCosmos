using UnityEngine;
using Completed;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // scene management at run-time.
using System.Collections;

public class DisplayManager : MonoBehaviour {

    public static DisplayManager Instance;

    // Set these in the inspector
    public Text virtualPosition;

    private ResourceBar populationBar;
    private ResourceBar energyBar;
    private ResourceBar spacebuxBar;

    public void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(Instance.gameObject);
        }
        Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
                         // I'm not even mad rn.
    }

	// Use this for initialization
	void Start () {
        populationBar = transform.Find("PopulationBar").GetComponent<ResourceBar>();
        populationBar.Initialize();
        populationBar.Hide(0);

        energyBar = transform.Find("EnergyBar").GetComponent<ResourceBar>();
        energyBar.Initialize();
        if (SceneManager.GetActiveScene().buildIndex != GameManager.instance.SystemLevel) {
            energyBar.Hide(0);
        }
        else {
            energyBar.Show(0);
        }

        spacebuxBar = transform.Find("SpacebuxBar").GetComponent<ResourceBar>();
        spacebuxBar.Initialize();
        spacebuxBar.Show(0);

        // TO DO literally just copy and paste this for population and power
        spacebuxBar.Value = PlayerData.instance.Spacebux;
        if (virtualPosition != null)
            virtualPosition.text = "Position: <" + Mathf.FloorToInt(GameManager.instance.virtualPosition.x) + " , " + Mathf.FloorToInt(GameManager.instance.virtualPosition.y) + ">";
    }
	
	// Update is called once per frame
	void Update () {
        // TO DO make this less spammy maybe idgaf
        spacebuxBar.Value = PlayerData.instance.Spacebux;
        if (virtualPosition != null)
            virtualPosition.text = "Position: <" + Mathf.FloorToInt(GameManager.instance.virtualPosition.x) + " , " + Mathf.FloorToInt(GameManager.instance.virtualPosition.y) + ">";
    }

    public void ShowPopulationBar(bool show) {
        populationBar.IsShowing = show;
    }

    public void ShowEnergyBar(bool show) {
        energyBar.IsShowing = show;
    }

    public void ShowSpacebuxBar(bool show) {
        spacebuxBar.IsShowing = show;
    }
}
