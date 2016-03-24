using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

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
    private Planet _planet;
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
        _shipsButton.onClick.AddListener(() => ShowShipMenu());

        _buildResearchRacerButton = transform.Find("BuildResearchRacer").GetComponent<Button>();
        _buildResearchRacerButton.onClick.AddListener(() => BuildResearchRacer());

        _buildColonyCarrierButton = transform.Find("BuildColonyCarrier").GetComponent<Button>();
        _buildColonyCarrierButton.onClick.AddListener(() => BuildColonyCarrier());

        gameObject.SetActive(false);

	}

	public void SetInfo(Planet planet) {
        _planet = planet;
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
    
    void BuildResearchRacer()
    {
        // Build ship
        ShipInfo s = new ShipInfo(0, _planet.planetNum, _planet.homeStar.myNumber);
        PlayerData.instance.shipList.Add(s);
        NetworkManager.instance._controller.SendNewShip(s); // Send ship creation request to server
        ShipSelectMenu.Instance.Refresh();
    }

    void BuildColonyCarrier()
    {
        ShipInfo s = new ShipInfo(1, _planet.planetNum, _planet.homeStar.myNumber);
        PlayerData.instance.shipList.Add(s);
        NetworkManager.instance._controller.SendNewShip(s); // Send ship creation request to server
        ShipSelectMenu.Instance.Refresh();
    }
    
}
