using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    /* KNOWN ISSUES:
	 * 		- Some planets may be unclickable, dependent on size of planet and
	 * 	 	  Orthographic size of object. Consider revising FindGameObjectAtPosition(pos).
	 */

    public GameObject focus;
    public GameObject action;
    private float clickTimer = 0;
    private Vector3 hitPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 cameraPosition = Vector3.zero;
    public float cameraZoom = 100;

    private LineRenderer outliner;

    public bool selectDestination = false;

    // Use this for initialization
    void Start()
    {
        outliner = GetComponent<LineRenderer>();
        Color c1 = new Color(0, 1, 0);
        outliner.materials[0].color = c1;
    }

    void Update()
    {
        float width = 0.025f * Camera.main.orthographicSize;
        outliner.SetWidth(width, width);
        // DRAG INITIALIZER
        if (Input.GetMouseButtonDown(0))
        {
            hitPosition = Input.mousePosition;
            cameraPosition = transform.position;
        }
        // DRAG HANDLER
        if (Input.GetMouseButton(0))
        {
            clickTimer += Time.deltaTime;
            currentPosition = Input.mousePosition;
            LeftMouseDrag();
            focus = null;
        }
        //CLICK HANDLER
        if (Input.GetMouseButtonUp(0))
        {
            if (clickTimer < 0.25f)
            {
                Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                focus = FindGameObjectAtPosition(clickPos);
                if (FindGameObjectAtPosition(clickPos) != null)
                {
                    action = FindGameObjectAtPosition(clickPos);
                }
                else
                    action = null;
            }
            clickTimer = 0;
        }

        // SCROLLING
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.UpArrow))
        {
            float newSize = cameraZoom / 1.5f;
            cameraZoom = Mathf.Max(5, newSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey(KeyCode.DownArrow))
        {
            float newSize = cameraZoom * 1.5f;
            cameraZoom = Mathf.Min(50000, newSize);
        }

        if (cameraZoom != Camera.main.orthographicSize)
        {
            float delta = cameraZoom - Camera.main.orthographicSize;
            if (Mathf.Abs(delta) < 0.1f)
            {
                Camera.main.orthographicSize = cameraZoom;
            }
            else
            {
                Camera.main.orthographicSize += delta * 10 * Time.deltaTime;
            }
        }

        //FOCUS OBJECT
        if (focus != null)
        {
            Vector3 focusPos = new Vector3(focus.transform.position.x,
                                           focus.transform.position.y,
                                           transform.position.z);
            float speed = Vector3.Distance(focusPos, transform.position);
            Vector3 newPos;
            if (speed < 0.1f)
            {
                newPos = focusPos;
            }
            else
            {
                newPos = Vector3.MoveTowards(transform.position, focusPos,
                                                 speed * 5 * Time.deltaTime);
            }
            transform.position = newPos;
            DrawOutline(focus);
        }
        else if (action != null)
        {
            DrawOutline(action);
        }
        else {
            outliner.enabled = false;
        }

        //Return to action object;
        if (Input.GetKeyDown(KeyCode.A) && action != null)
        {
            focus = action;
        }

    }

    void LeftMouseDrag()
    {
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        currentPosition.z = hitPosition.z = cameraPosition.y;
        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(currentPosition-hitPosition)
        // anyways.  
        Vector3 direction = Camera.main.ScreenToWorldPoint(currentPosition) - Camera.main.ScreenToWorldPoint(hitPosition);
        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;
        Vector3 position = cameraPosition + direction;
        transform.position = position;
    }

    public void DrawOutline(GameObject obj)
    {
        if (obj != null)
        {
            outliner.enabled = true;
            int numSegments = 128;
            float radius = obj.transform.localScale.x / 2;

            /*
			if (Camera.main.orthographicSize > 500) {
				if (obj.tag == "Star") {
					radius = obj.GetComponent<Light>().range;
				}
				if (obj.tag == Planet.TAG) {
					DrawOutline(obj.GetComponent<Planet>().homeStar.gameObject);
					return;
				}
			}
			*/

            outliner.SetVertexCount(numSegments + 1);

            float deltaTheta = (2.0f * Mathf.PI) / numSegments;
            float theta = 0;

            for (int i = 0; i < numSegments + 1; i++)
            {
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, y, -5) + obj.transform.position;
                outliner.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }

    public GameObject FindGameObjectAtPosition(Vector3 posn)
    {
        // get all colliders that intersect pos:
        Collider[] cols;
        if (Camera.main.orthographicSize > 30)
            cols = Physics.OverlapSphere(posn, Camera.main.orthographicSize / 5);
        else {
            cols = Physics.OverlapSphere(posn, 10);
        }
        // find the nearest one:
        float dist = Mathf.Infinity;
        GameObject nearest = null;
        foreach (Collider col in cols)
        {
            // find the distance to pos:
            float d = Vector3.Distance(posn, col.transform.position);
            if (d < dist)
            { // if closer...
                dist = d; // save its distance... 
                nearest = col.gameObject; // and its gameObject
            }
        }
        return nearest;
    }



    void OnGUI()
    {

    }
}
