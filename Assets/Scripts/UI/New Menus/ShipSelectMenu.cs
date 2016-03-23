using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ShipSelectMenu : MonoBehaviour {

	public static ShipSelectMenu Instance;

	public GameObject ShipSelectMenuItem_Prefab;

	private List<ShipSelectMenuItem> _ShipSelectMenuItems;

	private GameObject _layoutGroup;

    private uint _starID;
    private int _planetID;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
		                 // I'm not even mad rn.

		_ShipSelectMenuItems = new List<ShipSelectMenuItem>();
		FindLayoutGroup();
		gameObject.SetActive(false);
	}

	void FindLayoutGroup() {
		_layoutGroup = transform.Find("ScrollViewContainer/ScrollView/LayoutGroup").gameObject;
	}

	void AddShipSelectMenuItem() {
		GameObject item = Instantiate(ShipSelectMenuItem_Prefab) as GameObject;
		item.transform.parent = _layoutGroup.transform;
		item.transform.localScale = new Vector3(1,1,1);
		ShipSelectMenuItem script = item.GetComponent<ShipSelectMenuItem>();
		script.SetInfo(new ShipInfo());
		_ShipSelectMenuItems.Add(script);
	}

    void AddShipSelectMenuItem(ShipInfo s)
    {
        GameObject item = Instantiate(ShipSelectMenuItem_Prefab) as GameObject;
        item.transform.parent = _layoutGroup.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        ShipSelectMenuItem script = item.GetComponent<ShipSelectMenuItem>();
        script.SetInfo(s);
        _ShipSelectMenuItems.Add(script);
    }


    void OrderShipSelectMenuItems() {
		_ShipSelectMenuItems = _ShipSelectMenuItems.OrderBy(a => a.ShipClass).ThenBy(b => b.StatusText).ThenBy(c => c.NameText).ToList();
		for (int i = 0; i < _ShipSelectMenuItems.Count; i++) {
			ShipSelectMenuItem ship = _ShipSelectMenuItems[i];
			ship.transform.SetSiblingIndex(i);
		}
	}

	public void PopulateShipSelectMenu(uint starID)
    {

        foreach (ShipSelectMenuItem s in _ShipSelectMenuItems)
            Destroy(s.gameObject);
        _ShipSelectMenuItems = new List<ShipSelectMenuItem>();
        _starID = starID;
        _planetID = 0;
        foreach (ShipInfo s in PlayerData.instance.shipList)
            if (s.origin_star == starID)
                AddShipSelectMenuItem(s);
        OrderShipSelectMenuItems();
	}

    public void PopulateShipSelectMenu(uint starID, int planetNum)
    {

        foreach (ShipSelectMenuItem s in _ShipSelectMenuItems)
            Destroy(s.gameObject);
        _ShipSelectMenuItems = new List<ShipSelectMenuItem>();
        _starID = starID;
        _planetID = planetNum;
        foreach (ShipInfo s in PlayerData.instance.shipList)
            if (s.origin_star == starID)
                if (s.origin_planet == planetNum)
                    AddShipSelectMenuItem(s);
        OrderShipSelectMenuItems();
    }

    public void Refresh()
    {
        foreach (ShipSelectMenuItem s in _ShipSelectMenuItems)
            Destroy(s.gameObject);
        _ShipSelectMenuItems = new List<ShipSelectMenuItem>();
        if (_planetID == 0)
            PopulateShipSelectMenu(_starID);
        else
            PopulateShipSelectMenu(_starID, _planetID);
    }
	
	// Update is called once per frame
	void Update () {

	}


}
