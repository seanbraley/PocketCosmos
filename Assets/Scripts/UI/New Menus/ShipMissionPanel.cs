using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Completed;

public class ShipMissionPanel : MonoBehaviour {

	public GameObject Ship_Prefab;

	public static ShipMissionPanel Instance;

	private Button _okayButton;
	private Button _cancelButton;

	private Text _messageDisplayText;

	private GameObject _origin;
	public GameObject Origin {
		get {
			return _origin;
		}
		set {
			_origin = value;
		}
	}

	private GameObject _destination;
	public GameObject Destination {
		get {
			return _destination;
		}
		set {
			_destination = value;
		}
	}

	private ShipInfo _ship;


	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this) {
		    Destroy(Instance.gameObject);
		}
		Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
						// I'm not even mad rn.
		_okayButton = transform.Find("OkayButton").GetComponent<Button>();
		_cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		_messageDisplayText = transform.Find("MessageDisplay").GetComponent<Text>();
		_cancelButton.onClick.AddListener(() => CancelShipLaunch());
		_okayButton.onClick.AddListener(() => LaunchShip());

	}

	public void SetInfo(ShipInfo info) {
		_ship = info;

		_okayButton = transform.Find("OkayButton").GetComponent<Button>();
		_cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		_messageDisplayText = transform.Find("MessageDisplay").GetComponent<Text>();

		_cancelButton.onClick.AddListener(() => CancelShipLaunch());
		_okayButton.onClick.AddListener(() => LaunchShip());
		Origin = GameManager.instance.FindStar(339770494).gameObject;
		_destination = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (_destination == null) {
			_okayButton.interactable = false;
		}
		else {
			_okayButton.interactable = true;
		}


	}

	void CancelShipLaunch() {
		ShipMenu.Instance.gameObject.SetActive(true);
		ShipSelectMenu.Instance.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}

	public void LaunchShip() {
		if (Origin == null) {
			Debug.Log("Origin is null");
			return;
		}

		if (Destination == null) {
			Debug.Log("Destination is null");
			return;
		}
		LaunchShip(Origin,Destination);
	}

	public void LaunchShip(GameObject origin, GameObject destination) {
		Debug.Log("Launching Ship!");
        PlayerData.instance.spacebux -= 5;
        NetworkManager.instance._controller.SpendSpacebux(5); // TESTING
        GameObject ship = Instantiate(Ship_Prefab,Vector3.zero,Quaternion.identity) as GameObject;
		ship.GetComponent<Ship>().origin = Origin;
		ship.GetComponent<Ship>().destination = Destination;
        origin.GetComponent<Star>().KeepLoaded();
        destination.GetComponent<Star>().KeepLoaded();
    }
}
