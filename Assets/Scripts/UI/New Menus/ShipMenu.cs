using UnityEngine;
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
		Debug.Log(_layoutGroup != null);
	}

	void AddShipMenuItem() {
		GameObject item = Instantiate(ShipMenuItem_Prefab) as GameObject;
		item.transform.parent = _layoutGroup.transform;
		item.transform.localScale = new Vector3(1,1,1);
		_shipMenuItems.Add(item.GetComponent<ShipMenuItem>());
	}

	void PopulateShipMenu() {
		// TODO - Actually populate with real ships
		for (int i = 0; i < Random.Range(3,10); i++) {
			AddShipMenuItem();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}


}
