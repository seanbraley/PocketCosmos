using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Camera _camera;

	// Use this for initialization
	void Start () {
        _camera = GetComponent<Camera>();
        if (Input.touchCount > 0)  // Android
        {
            Debug.Log("In Android for movement");
        }
        else
        {
            Debug.Log("Desktop");
        }
        _camera.orthographicSize = 10;
    }
	
    void Update()  // LateUpdate() ?
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && _camera.orthographicSize < 20) {
            _camera.orthographicSize *= 1.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && _camera.orthographicSize > 5) {
            _camera.orthographicSize /= 1.5f;
        }
        /*
        if (Input.touchCount > 0)  // Android
        {

        }
        else  // Desktop
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector2(1, 0));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector2(-1, 0));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector2(0, 1));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector2(0, -1));
            }
        }
        */
    }
}
