using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {

		lRend = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		lRend.name = "Travel Path";
		lRend.gameObject.transform.parent = this.transform;
		dRend = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		dRend.name = "Destination Outline";
		dRend.gameObject.transform.parent = this.transform;


		Color c = new Color (1, 0, 0);
		render.color = c;
		dRend.material = render;
		lRend.material = render;

		transform.position = Vector3.MoveTowards(origin.transform.position,
		                                         destination.transform.position,
		                                         (origin.transform.localScale.x/2) + (transform.localScale.z/2));
		Vector3 lookPos = destination.transform.position;
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		speed = Vector3.Distance (origin.transform.position, destination.transform.position) / timeToDestination;
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
            // Discover the gameobject this planet was sent to
            destination.GetComponent<Star>().discovered = true;
            PlayerData.playdata.discoveredStarSystems.Add(new DiscoveredStar(destination));
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
