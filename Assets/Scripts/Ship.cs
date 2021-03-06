using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;  // scene management at run-time.
using Completed;
using System.Xml.Serialization;
using System.IO;

public class Ship : MonoBehaviour {

	public GameObject origin;
	private Vector3 startPos;
	private Vector3 endPos;
	public GameObject destination;
	private float travelTime;
	public float timeToDestination;
	private float speed;

	public Material render;
	private LineRenderer dRend;
	private LineRenderer lRend;
    public int id;

    private int _shipClass;
    public int ShipClass {
    	get {
    		return _shipClass;
    	}
    	set {
    		_shipClass = value;
    		if (value == 0) {
    			GetComponent<SpriteRenderer>().sprite = ResearchRacerSprite;
    		}
    		else {
    			GetComponent<SpriteRenderer>().sprite = ColonyCarrierSprite;
    		}
    	}
    }

    public Sprite ResearchRacerSprite;
    public Sprite ColonyCarrierSprite;

    private ShipInfo _info;

    // Use this for initialization
    void Start () {

		lRend = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		lRend.name = "Travel Path";
		lRend.gameObject.transform.parent = this.transform;
		dRend = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		dRend.name = "Destination Outline";
		dRend.gameObject.transform.parent = this.transform;


		Color c =  new Color (0.8f,0.1f,0.1f,0.5f);
		dRend.material = render;
		dRend.SetColors(c,c);
		lRend.material = render;
		lRend.SetColors(c,c);

		timeToDestination = Vector3.Distance(origin.transform.position,destination.transform.position) / 5;

		transform.position = Vector3.MoveTowards(origin.transform.position,
		                                         destination.transform.position,
		                                         (origin.transform.localScale.x/2) + (transform.localScale.z/2));
		Vector3 lookPos = destination.transform.position;
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        timeToDestination = Vector3.Distance(destination.transform.position, origin.transform.position) / 2f;

		speed = Vector3.Distance (origin.transform.position, destination.transform.position) / timeToDestination;

		if (SceneManager.GetActiveScene().buildIndex == GameManager.SystemLevel) {
			transform.localScale = new Vector3(5,5,5);
		}

    }


    public void SetInfo(ShipInfo info) {
    	_info = info;
    	ShipClass = info.ship_class;
        timeToDestination = Vector3.Distance(origin.transform.position, destination.transform.position) / 5;
    }
	
	// Update is called once per frame
	void Update () {
		travelTime += Time.deltaTime;
		startPos = Vector3.MoveTowards(origin.transform.position,
		                                         destination.transform.position,
		                                         (origin.transform.localScale.x/2) + (transform.localScale.z/2));
		endPos = Vector3.MoveTowards(destination.transform.position,
		                               origin.transform.position,
		                               (destination.transform.localScale.x/2) + (transform.localScale.z/2));
		Vector3 lookPos = destination.transform.position;
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2(lookPos.x, lookPos.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

		transform.position = Vector3.MoveTowards (startPos, endPos, speed * travelTime);

		if (transform.position == endPos) {
            
            Star destinationStar = destination.GetComponent<Star>();
            Planet destinationPlanet = destination.GetComponent<Planet>();

            XmlSerializer serializer = new XmlSerializer(typeof(DateTime));
            var xmlCurrentTime = "";
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, DateTime.Now);
                xmlCurrentTime = textWriter.ToString();
            }
            NetworkManager.instance._controller.SendMissionComplete(id, xmlCurrentTime); // NETWORK STUFF

            if (destinationStar)
            {
                // It's a star - discover the gameobject this ship was sent to
                destinationStar.Discovered = true;
                //PlayerData.instance.discoveredStarSystems.Add(new DiscoveredStar(destination, System.DateTime.Now));
                //NetworkManager.instance._controller.SendDiscoveredStar(destination.gameObject.GetComponent<Star>().myNumber);
                NetworkManager.instance._controller.RetrieveKnownStars();
                //PlayerData.instance.discoveredStarSystems.Add(destination.GetComponent<Star>().myNumber);
                destinationStar.SetDiscoveryTime(System.DateTime.Now);
                origin.GetComponent<Star>().Unload();
                destinationStar.Unload();
            }
            if (destinationPlanet)
            {
                // It's a planet - perform an action on the planet depending on ship class
                // Carrier ship: colonize the ship
                destinationPlanet.personalOwnership = true;
                destinationPlanet.orbitPath.SetColors(Color.green,Color.green);
                destinationPlanet.ownershipState = true;
                var owned = new OwnedPlanet(destination);
                owned.lastcollectedtime = DateTime.Now;
                owned.planetpower = Mathf.RoundToInt((float)destinationPlanet.energyModifier * destinationPlanet.homeStar.baseEnergyLevel);
                owned.planetpopulation = 1000;
                PlayerData.instance.AddOwnedPlanet(owned); // TESTING - add a planet
            }
            PlayerData.instance.shipList.Remove(_info);
            Destroy(this.gameObject);            
        }

		DrawLines ();
	}

	void DrawLines() {
		float width = 0.025f * Camera.main.orthographicSize;
		dRend.SetWidth (width,width);
		lRend.SetWidth (width/2,width/2);

		int numSegments = 128;
		float dRadius = origin.transform.localScale.x / 2;

		dRend.SetVertexCount(numSegments + 1);
		
		float deltaTheta = (2.0f *  Mathf.PI) / numSegments;
		float theta = 0;
		
		for (int i = 0; i < numSegments + 1; i++)
		{
			float xD = dRadius * Mathf.Cos(theta);
			float yD = dRadius * Mathf.Sin(theta);
			Vector3 pos = new Vector3(xD, yD, 20) + destination.transform.position;
			dRend.SetPosition(i, pos);
			theta += deltaTheta;
		}

		lRend.SetVertexCount (2);
		Vector3 depth = new Vector3 (0, 0, 20);
		lRend.SetPosition (0, transform.position + depth);
		lRend.SetPosition (1, destination.transform.position + depth);


	}

	void OrbitAround(GameObject orbitObject) {

	}


}
