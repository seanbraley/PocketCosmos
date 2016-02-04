using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Input.touchCount > 0)  // Android
        {
            Debug.Log("In Android for movement");
        }
        else
        {
            Debug.Log("Desktop");
        }
    }
	
    void Update()  // LateUpdate() ?
    {
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
