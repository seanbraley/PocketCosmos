using UnityEngine;
using System.Collections;

public class ContextMenuManager : MonoBehaviour {

	public static ContextMenuManager Instance;

	private StarMenu _starMenu;
	private PlanetMenu _planetMenu;
	//private ShipMenu _shipMenu;

	public void Awake() {
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
						 // I'm not even mad rn.
	}

	// Use this for initialization
	void Start () {
		GameObject star = transform.Find("StarMenu").gameObject;
		GameObject planet = transform.Find("PlanetMenu").gameObject;
		//GameObject ship = transform.Find("ShipMenu").gameObject;

		_starMenu = star.GetComponent<StarMenu>();
		_planetMenu = planet.GetComponent<PlanetMenu>();
		//_shipMenu = ship.GetComponent<ShipMenu>();

		ShowStarMenu(false);
		ShowPlanetMenu(false);
		//ShowShipMenu(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowStarMenu(bool show) {
		_starMenu.gameObject.SetActive(show);
		if (show) {
			ShowPlanetMenu(false);
			//ShowShipMenu(false);
		}
	}

	public void SetStarMenuInfo(Star star) {
		_starMenu.SetInfo(star);
	}

	public void SetStarMenuInfo(SystemStar star) {
		_starMenu.SetInfo(star);
	}

	public void ShowPlanetMenu(bool show) {
		_planetMenu.gameObject.SetActive(show);
		if (show) {
			ShowStarMenu(false);
			//ShowShipMenu(false);
		}
	}

	public void SetPlanetMenuInfo(Planet planet) {
		_planetMenu.SetInfo(planet);
	}

	/*
	public void ShowShipMenu(bool show) {
		//_shipMenu.gameObject.SetActive(show);
		if (show) {
			ShowPlanetMenu(false);
			ShowStarMenu(false);
		}
	}

	public void SetShipMenuInfo(Ship ship) {
		
	}
	*/
}
