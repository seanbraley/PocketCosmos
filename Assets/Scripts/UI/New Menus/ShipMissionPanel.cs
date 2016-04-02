using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;  // scene management at run-time.
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
		gameObject.SetActive(false);
	}

	public void SetInfo(ShipInfo info) {
		_ship = info;

		_okayButton = transform.Find("OkayButton").GetComponent<Button>();
		_cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		_messageDisplayText = transform.Find("MessageDisplay").GetComponent<Text>();

		_cancelButton.onClick.AddListener(() => CancelShipLaunch());
		_okayButton.onClick.AddListener(() => LaunchShip());

		if (SceneManager.GetActiveScene().buildIndex == GameManager.SectorLevel) {
			Origin = GameManager.instance.FindStar(_ship.origin_star).gameObject;
		}
		else {
			Origin = GameManager.instance.FindPlanet(_ship.origin_planet).gameObject;
		}
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
        GameObject ship = Instantiate(Ship_Prefab,Vector3.zero,Quaternion.identity) as GameObject;
		ship.GetComponent<Ship>().origin = Origin;
		ship.GetComponent<Ship>().destination = Destination;
        ship.GetComponent<Ship>().id = (int)_ship.id;
        if (origin.GetComponent<Star>()) {
			origin.GetComponent<Star>().KeepLoaded();
			destination.GetComponent<Star>().KeepLoaded();
		}

        Star destinationStar = destination.GetComponent<Star>();
        Planet destinationPlanet = destination.GetComponent<Planet>();
        if (destinationStar)
        {
            // It's a star
            _ship.destination_star = (uint)destinationStar.myNumber;
            _ship.destination_planet = 1;
        }
        else if (destinationPlanet)
        {
            // It's a planet
            _ship.destination_star = (uint)destinationPlanet.homeStar.myNumber;
            _ship.destination_planet = destinationPlanet.planetNum;
        }        
        ship.GetComponent<Ship>().SetInfo(_ship);
        gameObject.SetActive(false);

        // Send stuff to server
        var t = System.DateTime.Now;
        _ship.departure_time = t;
        _ship.arrival_time = t.AddSeconds(ship.GetComponent<Ship>().timeToDestination);
        NetworkManager.instance._controller.SendShipOnMission(_ship);
    }
}
