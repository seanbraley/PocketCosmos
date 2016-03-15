using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenu : ContextMenu {

	protected Text energyBarText;
	protected Text populationBarText;
	protected Text spacebuxBarText;


	public override void Awake() {
		base.Awake();

		GameObject energyBar = transform.Find("EnergyBar/Count").gameObject;
		GameObject populationBar = transform.Find("PopulationBar/Count").gameObject;
		GameObject spacebuxBar = transform.Find("SpacebucksBar/Count").gameObject;

		energyBarText = energyBar.GetComponent<Text>();
		populationBarText = populationBar.GetComponent<Text>();
		spacebuxBarText = spacebuxBar.GetComponent<Text>();
	}

	public void SetInfo(Planet planet) {
		if (planet) // planets cant be "discovered" yet
		{
			SetTitle(planet.name);

			string composition;
			if (planet.PlanetType == 1) {
				composition = "Gas";
			}
			else if (planet.PlanetType == 2) {
				composition = "Rocky";
			}
			else {
				composition = "Ocean";
			}
			SetDescription(composition + " Planet");

			SetEnergyBarText("TO-DO");
			SetSpaceBuxBarText("TO-DO");
			SetPopulationBarText("TO-DO");
		}
		else {
			SetTitle("Unknown Planet");
			SetDescription("Research this planet to learn more about it!");
			SetEnergyBarText("???");
			SetSpaceBuxBarText("???");
			SetPopulationBarText("???");
		}
	}

	public void SetEnergyBarText(string energyCount) {
		energyBarText.text = energyCount;
	}

	public void SetSpaceBuxBarText(string spacebuxCount) {
		spacebuxBarText.text = spacebuxCount;
	}

	public void SetPopulationBarText(string popCount) {
		populationBarText.text = popCount;
	}

}
