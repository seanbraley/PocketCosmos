using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ShipMenu : MonoBehaviour {

	public static ShipMenu Instance;

	private Text _nameText;
	public string NameText {
		get {
			return _nameText.text;
		}
		set {
			_nameText.text = value;
		}
	}

	private Text _statusText;
	public string StatusText {
		get {
			if (_statusText != null)
				return _statusText.text;
			else {
				return null;
			}
		}
		set {
			if (_statusText != null)
				_statusText.text = value;
		}
	}
	public Color StatusColor {
		get {
			if (_statusText != null)
				return _statusText.color;
			else
				return new Color(0,0,0,0);
		}
		set {
			if (_statusText != null)
				_statusText.color = value;
		}
	}

	public Sprite ResearchRacerSprite;
	public Sprite ColonyCarrierSprite;

	private Image _shipClassImage;
	private Text _shipClassText;
	public string ShipClass {
		get {
			return _shipClassText.text;
		}
		set {
			_shipClassText.text = value;
			if (value == "Research Racer") {
				_shipClassImage.sprite = ResearchRacerSprite;
				_shipSpecificButtonText.text = "Research";
				_shipSpecificButtonImage.sprite = ResearchSprite;
			}
			if (value == "Colony Carrier") {
				_shipClassImage.sprite = ColonyCarrierSprite;
				_shipSpecificButtonText.text = "Colonize";
				_shipSpecificButtonImage.sprite = ColonizeSprite;

			}
		}
	}

	private Image _originImage;
	private Text _originText;
	public uint OriginID {
		get {
			return uint.Parse(_originText.text);
		}
		set {
			Debug.Log("Origin: " + value.ToString());
			if (value != 0) {
				ShowOrigin(true);
				_originText.text = value.ToString();
			}
			else {
				ShowOrigin(false);
			}
		}
	}

	private Image _destinationImage;
	private Text _destinationText;
	public uint DestinationID {
		get {
			return uint.Parse(_destinationText.text);
		}
		set {
			Debug.Log("DESTINATION: " + value.ToString());
			if (value != 0) {
				ShowDestination(true);
				_destinationText.text = value.ToString();
			}
			else {
				ShowDestination(false);
			}
		}
	}

	private Button _travelButton;
	private Button _destroyButton;

	private Button _shipSpecificButton;
	private Image _shipSpecificButtonImage;
	private Text _shipSpecificButtonText;

	public Sprite ResearchSprite;
	public Sprite ColonizeSprite;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
		                 // I'm not even mad rn.
	}

	public void SetInfo(ShipInfo info) {
		_shipClassText = transform.Find("ShipClassText").GetComponent<Text>();
		_shipClassImage = transform.Find("ShipClassImage").GetComponent<Image>();
		_nameText = transform.Find("Name").GetComponent<Text>();
		_statusText = transform.Find("Status").GetComponent<Text>();

		_originImage = transform.Find("OriginImage").GetComponent<Image>();
		_originText = transform.Find("OriginText").GetComponent<Text>();
		_destinationImage = transform.Find("DestinationImage").GetComponent<Image>();
		_destinationText = transform.Find("DestinationText").GetComponent<Text>();

		_travelButton = transform.Find("TravelButton").GetComponent<Button>();
		_destroyButton = transform.Find("DestroyButton").GetComponent<Button>();

		_shipSpecificButton = transform.Find("ShipSpecificButton").GetComponent<Button>();
		_shipSpecificButtonImage = transform.Find("ShipSpecificButton/Image").GetComponent<Image>();
		_shipSpecificButtonText = transform.Find("ShipSpecificButton/Text").GetComponent<Text>();

		NameText = info.name;
		ShipClass = info.ship_class;

		OriginID = info.origin_planet;
		DestinationID = info.destination_planet;

		if (info.origin_planet != 0 && info.destination_planet != 0) {
			StatusText = "On Route";
			StatusColor = Color.red;
		}
		else if (info.origin_planet == 0 && info.destination_planet != 0) {
			StatusText = "Arrived";
			StatusColor = Color.blue;
		}
		else if (info.origin_planet != 0 && info.destination_planet == 0) {
			StatusText = "Ready";
			StatusColor = Color.green;
		}
		Debug.Log(_statusText.text);
	}

	void ShowOrigin(bool show) {
		_originText.gameObject.SetActive(show);
		_originImage.gameObject.SetActive(show);
	}

	void ShowDestination(bool show) {
		_destinationText.gameObject.SetActive(show);
		_destinationImage.gameObject.SetActive(show);
	}
}
