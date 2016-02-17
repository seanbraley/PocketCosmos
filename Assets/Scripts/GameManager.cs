using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;
using UnityEngine.SceneManagement;  // scene management at run-time.
using UnityEngine.EventSystems;     // handles input, raycasting, and sending events.


namespace Completed
{

    public class GameManager : MonoBehaviour
    {

        /*  Logical star map..
         * [
         *  [ (-40, 40), ... (40, 40)
         *    ...
         *    (-40, -40) ... (40, -40)
         *  ]
         * ]
         */
        public static GameManager instance = null;      //Static instance of GameManager which allows it to be accessed by any other script.

        // for clicking on an object
        public GameObject selected;         // Selected star
        public uint selectedID = 0;         // Selected star's number

        public int SectorLevel = 2;         // These should match scene and sector level numbers in build                    
        public int SystemLevel = 3;

        public Vector2 virtualPosition = Vector2.zero;
        private Vector2 lastKnownPosition = Vector2.zero;     // so players can return to last position when re-entering sector view

        private int movementCounterX = 0;
        private int movementCounterY = 0;

        private int mouseClicks = 0;
        private float mouseTimer = 0f;
        private float mouseTimerLimit = .25f;

        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public GameObject[] starPrefabs;

        static List<GameObject[]> starsList = new List<GameObject[]>();

        // Awake is always called before any Start functions
        // Only called once.
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the starting level 
            InitGame();
        }

        // Start is called once every scene start
        void Start() {
            // TODO: handle scene changes

            // Load last known position into virtual position
            virtualPosition = lastKnownPosition;
        }

        //Update is called every frame.
        void Update()
        {

            if (Input.GetKey(KeyCode.W) || SwipeManager.swipeDirection == Swipe.Up)
            {
                watch.Reset();
                watch.Start();
                ShiftUp();
                watch.Stop();
                Debug.Log(string.Format("Shift Up took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKey(KeyCode.S) || SwipeManager.swipeDirection == Swipe.Down)
            {
                watch.Reset();
                watch.Start();
                ShiftDown();
                watch.Stop();
                Debug.Log(string.Format("Shift Down took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKey(KeyCode.A) || SwipeManager.swipeDirection == Swipe.Left)
            {
                watch.Reset();
                watch.Start();
                ShiftLeft();
                watch.Stop();
                Debug.Log(string.Format("Shift Left took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKey(KeyCode.D) || SwipeManager.swipeDirection == Swipe.Right)
            {
                watch.Reset();
                watch.Start();
                ShiftRight();
                watch.Stop();
                Debug.Log(string.Format("Shift Right took: {0}ms", watch.ElapsedMilliseconds));
            }

            //CLICK HANDLER
            checkMouseDoubleClick();

            // Mobile platform touch input handler
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                checkTouchDoubleClick();
            }
#endif
            // TBD - testing code
            if (Input.GetKey(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == SystemLevel)
            {
                // Go back to sector view
                SceneManager.LoadScene(SectorLevel);
            }
        } // end Update()


        //Initializes the game level.
        void InitGame()
        {
            // rows iterate -40 --> 40 (inner)
            // columns iterate 40 --> -40
            // Hold y constant while iterating through x's
            for (int y = 40; y >= -40; y--) // Y value
            {
                GameObject[] tmp = new GameObject[81];
                for (int x = -40; x <= 40; x++) // X value
                {
                    if (Procedural.StarExists(x, y))
                    {
                        tmp[x + 40] = (GameObject)Instantiate(starPrefabs[0], new Vector2(x, y), Quaternion.identity);
                        //Debug.Log(string.Format("Star Created at: <{0}, {1}>", x, y));
                    }
                }
                starsList.Add(tmp);
            }
        }

        /// <summary>
        /// Gets row of stars at value y
        /// </summary>
        /// <param name="y">y value to generate array of stars for</param>
        /// <returns></returns>
        GameObject[] GetRowOfStars(int y, int virtualPos)
        {
            GameObject[] newStars = new GameObject[81];
            for (int i = -40; i <= 40; i++)
                if (Procedural.StarExists(i, virtualPos))
                    newStars[i+40] = (GameObject)Instantiate(starPrefabs[0], new Vector2(i, y), Quaternion.identity);

            return newStars;
        }


        GameObject[] GetColumnOfStars(int x, int virtualPos)
        {
            GameObject[] newStars = new GameObject[81];
            int helper = 0;
            for (int i = 40; i >= -40; i--) // iterate from up/down positive y to negative y
            {
                if (Procedural.StarExists(i, virtualPos))
                    newStars[helper] = (GameObject)Instantiate(starPrefabs[0], new Vector2(x, i), Quaternion.identity);
                helper++;
            }
            return newStars;
        }

        /// <summary>
        /// Moves all the stars in some direction
        /// </summary>
        /// <param name="direction">movement vector</param>
        void ShiftAllStars(Vector2 direction)
        {
            foreach (GameObject[] row in starsList)
                foreach (GameObject s in row)
                    if (s != null)
                        s.transform.position += (Vector3) direction;
        }

        /// <summary>
        /// Generate new row on top and cleanup last row (do first)
        /// trying to scroll up
        /// generate new top row
        /// remove bottom row
        /// shift all stars... down (top row closer to viewpoint)
        /// </summary>
        void ShiftUp()
        {
            virtualPosition.y++;  // y = 1
            GameObject[] newStars = GetRowOfStars(40, (int)virtualPosition.y + 40); // 41 (is this getting way ahead of the other?)
            foreach (GameObject s in starsList[starsList.Count - 1])  // Last row
                if (s != null)
                    Destroy(s);
            starsList.RemoveAt(starsList.Count - 1);  // remove last row entirely
            starsList.Insert(0, newStars);            // insert new row on top
            ShiftAllStars(Vector2.down);              // shift all
        }

        /// <summary>
        /// Generate new row on bottom and cleanup top row
        /// </summary>
        void ShiftDown()
        {
            virtualPosition.y--;  // y = -1
            GameObject[] newStars = GetRowOfStars(-40, (int)virtualPosition.y - 40); // -41
            foreach (GameObject s in starsList[0])  // first row
                if (s != null)
                    Destroy(s);
            starsList.RemoveAt(0);
            starsList.Add(newStars);
            ShiftAllStars(Vector2.up);
        }


        void ShiftRight()
        {
            virtualPosition.x++;
            GameObject[] newStars = GetColumnOfStars(40, (int)virtualPosition.x + 40);
            int helper = 0;
            foreach (GameObject[] starRow in starsList)
            {
                if (starRow[0] != null)
                    Destroy(starRow[0]);
                GameObject[] newArr = new GameObject[starRow.Length];
                System.Array.Copy(starRow, 1, newArr, 0, starRow.Length - 1);
                newArr[starRow.Length - 1] = newStars[helper];
                starsList[helper++] = newArr;
            }
            ShiftAllStars(Vector2.left);
        }

        void ShiftLeft()
        {
            virtualPosition.x--;
            GameObject[] newStars = GetColumnOfStars(-40, (int)virtualPosition.x - 40);
            int helper = 0;
            foreach (GameObject[] starRow in starsList)
            {
                if (starRow[80] != null)
                    Destroy(starRow[80]);
                GameObject[] newArr = new GameObject[starRow.Length];
                System.Array.Copy(starRow, 0, newArr, 1, starRow.Length - 1);
                newArr[0] = newStars[helper];
                starsList[helper++] = newArr;
            }
            ShiftAllStars(Vector2.right);
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


        // ----- Handles UI directional buttons
        public void LeftButton() {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel) {
                // Sector view
                ShiftLeft();
            }
        }

        public void RightButton() {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftRight();
            }            
        }

        public void UpButton() {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftUp();
            }            
        }

        public void DownButton() {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftDown();
            }
        }

        // Double and single mouse clicks
        // Source: http://answers.unity3d.com/answers/315649/view.html
        void checkMouseDoubleClick() {
            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0){
                mouseClicks++;
                Debug.Log("Num of mouse clicks ->" + mouseClicks);
            }

            if (mouseClicks >= 1 && mouseClicks < 3) {
                mouseTimer += Time.fixedDeltaTime;

                if (mouseClicks == 2) {
                    if (mouseTimer - mouseTimerLimit < 0) {
                        Debug.Log("Mouse Double Click");
                        mouseTimer = 0;
                        mouseClicks = 0;
                        /*Here you can add your double click event*/
                        if (selected != null && SceneManager.GetActiveScene().buildIndex == SectorLevel)
                        {
                            // Save last known position
                            lastKnownPosition = virtualPosition;
                            // Loads selected star's system
                            SceneManager.LoadScene(SystemLevel);
                        }
                    } else {
                        Debug.Log("Timer expired");
                        mouseClicks = 0;
                        mouseTimer = 0;
                        /*Here you can add your single click event*/
                        ClickSectorObject();
                    }
                }

                if (mouseTimer > mouseTimerLimit) {
                    Debug.Log("Timer expired");
                    mouseClicks = 0;
                    mouseTimer = 0;
                    /*Here you can add your single click event*/
                    ClickSectorObject();
                }
            }
        }

        // Handles click events in the sector scene
        public void ClickSectorObject() {
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
                    selectedID = selected.GetComponent<Star>().myNumber;
                    // Highlight selection by turning on the halo
                    Component halo = selected.GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                }
                else {
                    // Turn off the halo
                    selected = null;
                    selectedID = 0;
                }
            }  

        }

        // Double and single taps
        // Source: http://sushanta1991.blogspot.ca/2013/10/how-to-detect-double-tap-on-touch.html
        void checkTouchDoubleClick() {
            for (var i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    if (Input.GetTouch(i).tapCount == 1) {
                        /*Here you can add your single click event*/
                        TouchSectorObject();
                    }
                    if (Input.GetTouch(i).tapCount == 2) {                    
                        /*Here you can add your double click event*/
                        if (selected != null && SceneManager.GetActiveScene().buildIndex == SectorLevel) {
                            // Save last known position
                            lastKnownPosition = virtualPosition;
                            // Loads selected star's system
                            SceneManager.LoadScene(SystemLevel);
                        }
                    }
                } // end if
            } // end for            
        }

        // Handles touch events in the sector scene
        void TouchSectorObject() {
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
                    selectedID = selected.GetComponent<Star>().myNumber;
                    // Highlight selection by turning on the halo
                    Component halo = selected.GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                }
                else {
                    selected = null;
                }
            }
        }

        
    }
   
}