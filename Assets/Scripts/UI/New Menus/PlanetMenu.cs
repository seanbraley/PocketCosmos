using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenu : MonoBehaviour {

	public static PlanetMenu Instance;

	private Text _nameText;
	public string NameText {
		get {
			return _nameText.text;
		}
		set {
			_nameText.text = value;
		}
	}

	private Button _shipsButton;
	private Button _buildResearchRacerButton;
	private Button _buildColonyCarrierButton;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
		                 // I'm not even mad rn.

        _nameText = transform.Find("Name").GetComponent<Text>();
        _shipsButton = transform.Find("ShipsButton").GetComponent<Button>();
        _buildResearchRacerButton = transform.Find("BuildResearchRacer").GetComponent<Button>();
        _buildColonyCarrierButton = transform.Find("BuildColonyCarrier").GetComponent<Button>();
        _shipsButton.onClick.AddListener(() => ShowShipMenu());
        gameObject.SetActive(false);

	}

	public void SetInfo(Planet planet) {
		_nameText = transform.Find("Name").GetComponent<Text>();

		_shipsButton = transform.Find("ShipsButton").GetComponent<Button>();
		_buildResearchRacerButton = transform.Find("BuildResearchRacer").GetComponent<Button>();
		_buildColonyCarrierButton = transform.Find("BuildColonyCarrier").GetComponent<Button>();

		_shipsButton.onClick.AddListener(() => ShowShipMenu());
	}

	public void ShowShipMenu() {
		ShipSelectMenu.Instance.gameObject.SetActive(true);
		ShipSelectMenu.Instance.transform.SetAsLastSibling();
		transform.SetAsFirstSibling();
	}
}
