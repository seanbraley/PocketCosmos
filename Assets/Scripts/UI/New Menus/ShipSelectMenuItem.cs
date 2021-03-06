﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipSelectMenuItem : MonoBehaviour {

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

	private Image _shipClassImage;
    // FIX THIS LATER
	public int ShipClass {
		get {
			if (_shipClassImage.sprite == ResearchRacerSprite) {
				return 0;
			}
			else if (_shipClassImage.sprite == ColonyCarrierSprite) {
				return 1;
			}
			else {
				return 0;
			}
		}
		set {
			if (value == 0) {
				_shipClassImage.sprite = ResearchRacerSprite;
			}
			if (value == 1) {
				_shipClassImage.sprite = ColonyCarrierSprite;
			}
		}
	}

	public Sprite ResearchRacerSprite;
	public Sprite ColonyCarrierSprite;

	public void SetInfo(ShipInfo info) {
		_shipClassImage = transform.Find("Image").GetComponent<Image>();
		_nameText = transform.Find("Name").GetComponent<Text>();
		_statusText = transform.Find("Status").GetComponent<Text>();

		NameText = info.name;
		ShipClass = info.ship_class;

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

		GetComponent<Button>().onClick.AddListener(() => OpenShipMenu(info));
		Debug.Log(_statusText.text);
	}

	void OpenShipMenu(ShipInfo info) {
		ShipMenu.Instance.gameObject.SetActive(true);
		ShipMenu.Instance.transform.SetAsLastSibling();
		ShipMenu.Instance.SetInfo(info);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
