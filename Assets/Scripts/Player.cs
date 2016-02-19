using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;  // scene management at run-time.
using UnityEngine.EventSystems;     // handles input, raycasting, and sending events.
using Completed;

/// <summary>
/// Handles player events and information - touches and mouse clicks.
/// </summary>
public class Player : MonoBehaviour {

    public static Player plyr = null;      //Static instance of GameManager which allows it to be accessed by any other script.    

    // for clicking on an object
    private GameObject _selected;
    public GameObject selected {
        get {
            return _selected;
        }
        set {
            Component halo;
            if (selected != null) {
                halo = selected.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            }

            _selected = value;

            if (selected != null) {
                halo = selected.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            }
        }
    }

    private int mouseClicks = 0;
    private float mouseTimer = 0f;
    private float mouseTimerLimit = .25f;


    // Awake is always called before any Start functions
    // Only called once.
    void Awake()
    {
        //Check if instance already exists
        if (plyr == null)

            //if not, set instance to this
            plyr = this;

        //If instance already exists and it's not this:
        else if (plyr != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

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

    /* ----- Mouse click/touch actions ----- */

    // Double and single mouse clicks
    // Source: http://answers.unity3d.com/answers/315649/view.html
    public void checkMouseDoubleClick()
    {
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            mouseClicks++;
            Debug.Log("Num of mouse clicks ->" + mouseClicks);
        }

        if (mouseClicks >= 1 && mouseClicks < 3)
        {
            mouseTimer += Time.fixedDeltaTime;

            if (mouseClicks == 2)
            {
                if (mouseTimer - mouseTimerLimit < 0)
                {
                    Debug.Log("Mouse Double Click");
                    mouseTimer = 0;
                    mouseClicks = 0;
                    /*Here you can add your double click event*/
                    if (selected != null && SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                    {
                        // Save last known position
                        GameManager.instance.lastKnownPosition = GameManager.instance.virtualPosition;
                        // Loads selected star's system
                        SceneManager.LoadScene(GameManager.instance.SystemLevel);
                    }
                    if (selected != null && SceneManager.GetActiveScene().buildIndex == GameManager.instance.SystemLevel)
                    {
                        // Testing
                        ClickSystemObject();
                    }
                }
                else {
                    Debug.Log("Timer expired");
                    mouseClicks = 0;
                    mouseTimer = 0;
                    /*Here you can add your single click event*/
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                    {
                        ClickSectorObject();
                    }
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SystemLevel)
                    {
                        ClickSystemObject();
                    }
                }
            }

            if (mouseTimer > mouseTimerLimit)
            {
                Debug.Log("Timer expired");
                mouseClicks = 0;
                mouseTimer = 0;
                /*Here you can add your single click event*/
                if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                {
                    ClickSectorObject();
                }
                if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SystemLevel)
                {
                    ClickSystemObject();
                }
            }
        }
    }

    // Handles click events in the sector scene
    public void ClickSectorObject()
    {
        // Check if the mouse was clicked over a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Not on a UI element so we can proceed
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (FindGameObjectAtPosition(clickPos) != null)
            {
                // de-select previous object and turn off halo if applicable
                GameObject prevSelect = selected;
                if (prevSelect != null)
                {
                    Component hlo = prevSelect.GetComponent("Halo");
                    hlo.GetType().GetProperty("enabled").SetValue(hlo, false, null);
                }
                // Set selected object
                selected = FindGameObjectAtPosition(clickPos);
                GameManager.instance.selectedID = selected.GetComponent<Star>().myNumber;
                // Highlight selection by turning on the halo
                Component halo = selected.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            }
            else {
                // Turn off the halo
                selected = null;
                GameManager.instance.selectedID = 0;
            }
        }

    }

    // Handles click events in the system scene
    public void ClickSystemObject()
    {
        // Check if the mouse was clicked over a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Not on a UI element so we can proceed
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (FindGameObjectAtPosition(clickPos) != null)
            {
                // de-select previous object and turn off halo if applicable
                GameObject prevSelect = selected;
                // Set selected object
                selected = FindGameObjectAtPosition(clickPos);
                Debug.Log("Clicked " + selected.name);  //testing
                //Collect(selected.GetComponent<Population>());  // testing
                Collect(selected.GetComponent<Spacebux>());  // testing
            }
            else {
                selected = null;
            }
        }

    }

    // Double and single taps
    // Source: http://sushanta1991.blogspot.ca/2013/10/how-to-detect-double-tap-on-touch.html
    public void checkTouchDoubleClick()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 1)
                {
                    /*Here you can add your single click event*/
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                    {
                        TouchSectorObject();
                    }
                    if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SystemLevel)
                    {
                        TouchSystemObject();
                    }
                    
                }
                if (Input.GetTouch(i).tapCount == 2)
                {
                    /*Here you can add your double click event*/
                    if (selected != null && SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                    {                        
                        if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SectorLevel)
                        {
                            // Save last known position
                            GameManager.instance.lastKnownPosition = GameManager.instance.virtualPosition;
                            // Loads selected star's system
                            SceneManager.LoadScene(GameManager.instance.SystemLevel);
                        }
                        if (SceneManager.GetActiveScene().buildIndex == GameManager.instance.SystemLevel)
                        {
                            TouchSystemObject();
                        }
                    }
                }
            } // end if
        } // end for            
    }

    // Handles touch events in the sector scene
    void TouchSectorObject()
    {
        // Finger is not over a UI element
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (FindGameObjectAtPosition(touchPos) != null)
            {
                // de-select previous object and turn off halo if applicable
                GameObject prevSelect = selected;
                if (prevSelect != null)
                {
                    Component hlo = prevSelect.GetComponent("Halo");
                    hlo.GetType().GetProperty("enabled").SetValue(hlo, false, null);
                }
                // Set selected object
                selected = FindGameObjectAtPosition(touchPos);
                GameManager.instance.selectedID = selected.GetComponent<Star>().myNumber;
                // Highlight selection by turning on the halo
                Component halo = selected.GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            }
            else {
                // Turn off the halo
                selected = null;
                GameManager.instance.selectedID = 0;
            }
        }
    }

    // Handles touch events in the system scene
    void TouchSystemObject()
    {
        // Finger is not over a UI element
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (FindGameObjectAtPosition(touchPos) != null)
            {
                GameObject prevSelect = selected;
                // Set selected object
                selected = FindGameObjectAtPosition(touchPos);
                // Do something
                Debug.Log("Touched "+ selected.name);  //testing
                //Collect(selected.GetComponent<Population>());  // testing
                Collect(selected.GetComponent<Spacebux>());  // testing
            }
            else
            {
                selected = null;
            }
        }        
    }

    // Collect resource
    private void Collect(Resource resource)
    {
        if (resource != null)
            resource.Gather();
    }

}
