using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipMenuItem : MonoBehaviour {

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
				return null;
			else
				return _statusText.text;
		}
		set {
			if (_statusText != null)
				return;
			else
				_statusText.text = value;
		}
	}
	public Color StatusColor {
		get {
			if (_statusText != null)
				return new Color(0,0,0,0);
			else
				return _statusText.color;
		}
		set {
			if (_statusText != null)
				return;
			else
				_statusText.color = value;
		}
	}

	private Image _shipClassImage;
	public string ShipClass {
		get {
			if (_shipClassImage.sprite == ResearchRacerSprite) {
				return "Research Racer";
			}
			else if (_shipClassImage.sprite == ColonyCarrierSprite) {
				return "Colony Carrier";
			}
			else {
				return null;
			}
		}
		set {
			if (value == "Research Racer") {
				_shipClassImage.sprite = ResearchRacerSprite;
			}
			if (value == "Colony Carrier") {
				_shipClassImage.sprite = ColonyCarrierSprite;
			}
		}
	}

	public Sprite ResearchRacerSprite;
	public Sprite ColonyCarrierSprite;

	void Initialize(ShipInfo info) {
		_shipClassImage = transform.Find("Image").GetComponent<Image>();
		_nameText = transform.Find("Name").GetComponent<Text>();
		_statusText = transform.Find("Status").GetComponent<Text>();


		NameText = info.name;
		ShipClass = info.ship_class;

		if (info.origin_planet != 0 && info.destination_planet != 0) {
			StatusText = "On Route";
			StatusColor = Color.yellow;
		}
		else if (info.origin_planet != 0 && info.destination_planet == 0) {
			StatusText = "Arrived";
			StatusColor = Color.green;
		}
		else if (info.origin_planet == 0 && info.destination_planet != 0) {
			StatusText = "Waiting";
			StatusColor = Color.red;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
