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
	private LineRenderer destinationOutlineRenderer;
	private LineRenderer destinationLineRenderer;
	private LineRenderer originLineRenderer;
	private LineRenderer originOutlineRenderer;

	// Use this for initialization
	void Start () {

		destinationLineRenderer = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		destinationLineRenderer.name = "Destination Line";
		destinationLineRenderer.gameObject.transform.parent = this.transform;
		destinationOutlineRenderer = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		destinationOutlineRenderer.name = "Destination Outline";
		destinationOutlineRenderer.gameObject.transform.parent = this.transform;

		originLineRenderer = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		originLineRenderer.name = "Origin Line";
		originLineRenderer.gameObject.transform.parent = this.transform;
		originOutlineRenderer = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
		originOutlineRenderer.name = "Destination Outline";
		originOutlineRenderer.gameObject.transform.parent = this.transform;

		Material redMaterial = new Material(render); 
		Color red = new Color (1, 0, 0);
		redMaterial.color = red;
		destinationOutlineRenderer.material = redMaterial;
		destinationLineRenderer.material = redMaterial;

		Material greenMaterial = new Material(render);
		Color green = new Color (0, 1, 0);
		greenMaterial.color = green;
		originOutlineRenderer.material = greenMaterial;
		originLineRenderer.material = greenMaterial;

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
			Destroy(this.gameObject);
		}

		DrawLines ();
	}

	void DrawLines() {
		float width = 0.025f * Camera.main.orthographicSize;
		destinationOutlineRenderer.SetWidth (width,width);
		destinationLineRenderer.SetWidth (width/2,width/2);
		originOutlineRenderer.SetWidth (width,width);
		originLineRenderer.SetWidth (width/2,width/2);

		int numSegments = 128;
		float oRadius = origin.transform.localScale.x / 2;
		float dRadius = destination.transform.localScale.x/2;

		destinationOutlineRenderer.SetVertexCount(numSegments + 1);
		originOutlineRenderer.SetVertexCount(numSegments + 1);
		
		float deltaTheta = (2.0f *  Mathf.PI) / numSegments;
		float theta = 0;
		
		for (int i = 0; i < numSegments + 1; i++)
		{
			float xD = dRadius * Mathf.Cos(theta);
			float yD = dRadius * Mathf.Sin(theta);
			float xO = oRadius * Mathf.Cos(theta);
			float yO = oRadius * Mathf.Sin(theta);
			Vector3 dPos = new Vector3(xD, yD, 20) + destination.transform.position;
			Vector3 oPos = new Vector3(xD, yD, 20) + origin.transform.position;
			destinationOutlineRenderer.SetPosition(i, dPos);
			originOutlineRenderer.SetPosition(i,oPos);
			theta += deltaTheta;
		}

		destinationLineRenderer.SetVertexCount (2);
		Vector3 depth = new Vector3 (0, 0, 20);
		destinationLineRenderer.SetPosition (0, transform.position + depth);
		destinationLineRenderer.SetPosition (1, destination.transform.position + depth);

		originLineRenderer.SetVertexCount (2);
		originLineRenderer.SetPosition (0, transform.position + depth);
		originLineRenderer.SetPosition (1, origin.transform.position + depth);

	}

	void OrbitAround(GameObject orbitObject) {

	}

	void FlyTowards(GameObject dest) {

	}
}
