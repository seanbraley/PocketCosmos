using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ShipSelectMenu : MonoBehaviour {

	public static ShipSelectMenu Instance;

	public GameObject ShipSelectMenuItem_Prefab;

	private List<ShipSelectMenuItem> _ShipSelectMenuItems;

	private GameObject _layoutGroup;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
		                 // I'm not even mad rn.

		_ShipSelectMenuItems = new List<ShipSelectMenuItem>();
		FindLayoutGroup();
		PopulateShipSelectMenu();
	}

	void FindLayoutGroup() {
		_layoutGroup = transform.Find("ScrollViewContainer/ScrollView/LayoutGroup").gameObject;
	}

	void AddShipSelectMenuItem() {
		GameObject item = Instantiate(ShipSelectMenuItem_Prefab) as GameObject;
		item.transform.parent = _layoutGroup.transform;
		item.transform.localScale = new Vector3(1,1,1);
		ShipSelectMenuItem script = item.GetComponent<ShipSelectMenuItem>();
		script.Initialize(new ShipInfo());
		_ShipSelectMenuItems.Add(script);
	}

	void OrderShipSelectMenuItems() {
		_ShipSelectMenuItems = _ShipSelectMenuItems.OrderBy(a => a.ShipClass).ThenBy(b => b.StatusText).ThenBy(c => c.NameText).ToList();
		for (int i = 0; i < _ShipSelectMenuItems.Count; i++) {
			ShipSelectMenuItem ship = _ShipSelectMenuItems[i];
			ship.transform.SetSiblingIndex(i);
		}
	}

	void PopulateShipSelectMenu(/*uint id*/) {
		// TODO - Actually populate with real ships
		for (int i = 0; i < Random.Range(10,100); i++) {
			AddShipSelectMenuItem();
		}
		OrderShipSelectMenuItems();
	}
	
	// Update is called once per frame
	void Update () {

	}


}
