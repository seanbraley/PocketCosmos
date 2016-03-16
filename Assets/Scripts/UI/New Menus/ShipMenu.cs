using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ShipMenu : MonoBehaviour {

	public static ShipMenu Instance;

	public GameObject ShipMenuItem_Prefab;

	private List<ShipMenuItem> _shipMenuItems;

	private GameObject _layoutGroup;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
		                 // I'm not even mad rn.

		_shipMenuItems = new List<ShipMenuItem>();
		FindLayoutGroup();
		PopulateShipMenu();
	}

	void FindLayoutGroup() {
		_layoutGroup = transform.Find("ScrollViewContainer/ScrollView/LayoutGroup").gameObject;
	}

	void AddShipMenuItem() {
		GameObject item = Instantiate(ShipMenuItem_Prefab) as GameObject;
		item.transform.parent = _layoutGroup.transform;
		item.transform.localScale = new Vector3(1,1,1);
		ShipMenuItem script = item.GetComponent<ShipMenuItem>();
		script.Initialize(new ShipInfo());
		_shipMenuItems.Add(script);
	}

	void OrderShipMenuItems() {
		_shipMenuItems = _shipMenuItems.OrderBy(a => a.ShipClass).ThenBy(b => b.StatusText).ThenBy(c => c.NameText).ToList();
		for (int i = 0; i < _shipMenuItems.Count; i++) {
			ShipMenuItem ship = _shipMenuItems[i];
			ship.transform.SetSiblingIndex(i);
		}
	}

	void PopulateShipMenu() {
		// TODO - Actually populate with real ships
		for (int i = 0; i < Random.Range(10,100); i++) {
			AddShipMenuItem();
		}
		OrderShipMenuItems();
	}
	
	// Update is called once per frame
	void Update () {

	}


}
