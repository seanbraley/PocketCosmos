using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarMenu : ContextMenu {

	protected Text totalPlanetsText;
	protected Text discoveredPlanetsText;
	protected Text ownedPlanetsText;

	protected Text energyBarText;


	public override void Awake() {
		base.Awake();
		GameObject totalPlanets = transform.Find("TotalPlanets").gameObject;
		GameObject discoveredPlanets = transform.Find("DiscoveredPlanets").gameObject;
		GameObject ownedPlanets = transform.Find("OwnedPlanets").gameObject;
		GameObject energyBar = transform.Find("EnergyBar/Count").gameObject;

		totalPlanetsText = totalPlanets.GetComponent<Text>();
		discoveredPlanetsText = discoveredPlanets.GetComponent<Text>();
		ownedPlanetsText = ownedPlanets.GetComponent<Text>();
		energyBarText = energyBar.GetComponent<Text>();
	}

	public void SetInfo(Star star) {
		if (star.Discovered)
		{
			SetTitle("Star #" + star.myNumber.ToString());
			SetDescription("Big Giant Hot Thing (TO-DO)");
			SetTotalPlanetsText("TO-DO");
			SetDiscoveredPlanetsText("TO-DO");
			SetOwnedPlanetsText("TO-DO");
			SetEnergyBarText("TO-DO");
		}
		else {
			SetTitle("Unknown Star");
			SetDescription("Research this star to learn more about it!");
			SetTotalPlanetsText("???");
			SetDiscoveredPlanetsText("0");
			SetOwnedPlanetsText("0");
			SetEnergyBarText("???");
		}
	}

	public void SetInfo(SystemStar star) {
		SetTitle("Star #" + star.myNumber.ToString());
		SetDescription("Big Giant Hot Thing (TO-DO)");
		SetTotalPlanetsText("TO-DO");
		SetDiscoveredPlanetsText("TO-DO");
		SetOwnedPlanetsText("TO-DO");
		SetEnergyBarText("TO-DO");
	}

	public void SetTotalPlanetsText(string numPlanets) {
		totalPlanetsText.text = "Total Planets: " + numPlanets;
	}

	public void SetDiscoveredPlanetsText(string numPlanets) {
		discoveredPlanetsText.text = "Discovered Planets: " + numPlanets;
	}

	public void SetOwnedPlanetsText(string numPlanets) {
		ownedPlanetsText.text = "Owned Planets: " + numPlanets;
	}

	public void SetEnergyBarText(string energyCount) {
		energyBarText.text = energyCount;
	}

}
