using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;
using UnityEngine.SceneManagement;  // scene management at run-time.
using UnityEngine.EventSystems;     // handles input, raycasting, and sending events.

namespace Completed
{
    /// <summary>
    /// Galaxy generation and movement
    /// </summary>
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
        public uint selectedID = 0;         // Selected star's number

        public int SectorLevel = 2;         // These should match scene and sector level numbers in build                    
        public int SystemLevel = 3;

        public Vector2 virtualPosition;
        public static Vector2 lastKnownPosition;     // so players can return to last position when re-entering sector view

        public static System.DateTime destinationStarDiscoveryTime;

        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public GameObject[] starPrefabs;

        static List<GameObject> allStars;
        public static List<GameObject> keepLoadedStars;

        BigInteger virtualX = new BigInteger();
        BigInteger virtualY = new BigInteger();
        

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

            NetworkManager.instance._controller.RetrieveKnownStars(); // TESTING

            keepLoadedStars = new List<GameObject>();

            foreach (GameObject star in keepLoadedStars) {
                if (PlayerData.instance.discoveredStarSystems.Contains(star.GetComponent<Star>().myNumber))
                {
                    star.GetComponent<Star>().Discovered = true;
                }
            }

            if (lastKnownPosition == Vector2.zero)  // no last known position
                instance.virtualPosition = PlayerData.instance.lastPosition;    // TODO: update to respond to server call
            else
                instance.virtualPosition = lastKnownPosition;

            virtualPosition = instance.virtualPosition;

            virtualX = new BigInteger((long)virtualPosition.x);
            virtualX = new BigInteger((long)virtualPosition.y);

            //virtualPosition = instance.virtualPosition;

            //Call the InitGame function to initialize the starting level 
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
                InitGame();

            // TODO: update to respond to server call
            // First-time player initialization - Get first star, add to discovered star list
            if (PlayerData.instance.discoveredStarSystems.Count == 0 && SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // First star system
                GameObject firstStar = Player.instance.FindGameObjectAtPosition(Vector3.zero);  // TODO: update to respond to server call??

                //PlayerData.instance.Spacebux += 100;  // TODO: update to respond to server call

                // Discover the star
                firstStar.GetComponent<Star>().Discovered = true;
                //firstStar.GetComponent<Star>().SetDiscoveryTime(System.DateTime.Now);
                PlayerData.instance.discoveredStarSystems.Add(firstStar.GetComponent<Star>().myNumber);
            }
        }
        

        // Start is called once every scene start
        void Start()
        {
        
        }


        //Update is called every frame.
        void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                if (Input.GetKey(KeyCode.W) || SwipeManager.swipeDirection == Swipe.Up)
                {
                    ShiftUp();
                }
                else if (Input.GetKey(KeyCode.S) || SwipeManager.swipeDirection == Swipe.Down)
                {
                    ShiftDown();
                }
                else if (Input.GetKey(KeyCode.A) || SwipeManager.swipeDirection == Swipe.Left)
                {
                    ShiftLeft();
                }
                else if (Input.GetKey(KeyCode.D) || SwipeManager.swipeDirection == Swipe.Right)
                {
                    ShiftRight();
                }
            }
            //CLICK HANDLER
            Player.instance.checkMouseDoubleClick();

            // Mobile platform touch input handler
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Player.instance.checkTouchDoubleClick();
            }
#endif
            
        } // end Update()

        public void SetDiscovery(System.DateTime time)
        {
            destinationStarDiscoveryTime = time;
        }

        //Initializes the game level.
        void InitGame()
        {
            Debug.Log(string.Format("Starting sector view at coordinates: <{0},{1}>", instance.virtualPosition.x, instance.virtualPosition.y));
            // rows iterate -40 --> 40 (inner)
            // columns iterate 40 --> -40
            // Hold y constant while iterating through x's
            allStars = new List<GameObject>();
            for (int y = (int)instance.virtualPosition.y + 40; y >= (int)instance.virtualPosition.y - 40; y--) // Y value (virtual)
            {
                for (int x = (int)instance.virtualPosition.x - 40; x <= (int)instance.virtualPosition.x + 40; x++) // X value (virtual)
                {
                    if (Procedural.StarExists(x, y))
                    {
                        GameObject star = CreateStarAt(new Vector2(x, y));
                        if (star != null)
                            allStars.Add(star);

                        //tmp[x + 40] = CreateStarAt(new Vector2((int)virtualPosition.x + x, (int)virtualPosition.y + y));
                        //tmp[x + 40].GetComponent<Star>().SetNumber((int)virtualPosition.x + x, (int)virtualPosition.y + y);
                        //Debug.Log(string.Format("Star Created at: <{0}, {1}>", x, y));
                    }
                }
            }
        }

        public GameObject CreateStarAt(Vector2 virtualPosition)
        {

            GameObject star = (GameObject)Instantiate(starPrefabs[0], virtualPosition - instance.virtualPosition, Quaternion.identity);
            star.GetComponent<Star>().SetNumber((int)virtualPosition.x, (int)virtualPosition.y);
            // Check if star already loaded
            if (keepLoadedStars.Count > 0)  // No kept stars means dont check
            {
                bool loaded = false;
                foreach (GameObject s in keepLoadedStars)
                    if (star.GetComponent<Star>().GetNumber() == s.GetComponent<Star>().GetNumber())
                        loaded = true;
                if (loaded)
                {
                    Destroy(star);
                    return null;
                }
                else
                    return star;
            }
            else
            {
                return star;
            }
        }

        /// <summary>
        /// Gets row of stars at value y
        /// </summary>
        /// <param name="y">y value to generate array of stars for</param>
        /// <returns></returns>
        List<GameObject> GetRowOfStars(int virtualY)
        {
            List<GameObject> newStars = new List<GameObject>();
            for (int x = (int)instance.virtualPosition.x - 40; x <= (int)instance.virtualPosition.x + 40; x++)
                if (Procedural.StarExists(x,  virtualY))
                {
                    GameObject star = CreateStarAt(new Vector2(x, virtualY));
                    if (star != null)
                        newStars.Add(star);
                }
            return newStars;
        }


        List<GameObject> GetColumnOfStars(int virtualX)
        {
            List<GameObject> newStars = new List<GameObject>();
            for (int y = (int)instance.virtualPosition.y + 40; y >= (int)instance.virtualPosition.y - 40; y--) // iterate from up/down positive y to negative y
            {
                if (Procedural.StarExists(virtualX, y))
                {
                    GameObject star = CreateStarAt(new Vector2(virtualX, y));
                    if (star != null)
                        newStars.Add(star);
                }
            }
            return newStars;
        }

        /// <summary>
        /// Moves all the stars in some direction
        /// </summary>
        /// <param name="direction">movement vector</param>
        void ShiftAllStars(Vector2 direction)
        {
            List<GameObject> garbage = new List<GameObject>();
            PlayerData.instance.lastPosition = instance.virtualPosition;
            foreach (GameObject s in allStars)
            {
                if ((s.transform.position.x < -41 || s.transform.position.x > 41 || s.transform.position.y < -41 || s.transform.position.y > 41) && s.GetComponent<Star>().CheckUnload())
                    garbage.Add(s);
                else
                    s.transform.position += (Vector3)direction;
            }
            foreach (GameObject s in garbage)
            {
                Destroy(s.gameObject);
                allStars.Remove(s);
            }
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
            ShiftAllStars(Vector2.down);              // shift all
            //virtualPosition.y++;  // y = 1
            instance.virtualPosition.y++;  // y = 1

            List<GameObject> newStars = GetRowOfStars((int)instance.virtualPosition.y + 40);
            foreach (GameObject s in newStars)
                allStars.Add(s);

            /*
            GameObject[] newStars = GetRowOfStars((int)virtualPosition.y, 40);
            foreach (GameObject s in starsList[starsList.Count - 1])  // Last row
                if (s != null)
                    Destroy(s);
            starsList.RemoveAt(starsList.Count - 1);  // remove last row entirely
            starsList.Insert(0, newStars);            // insert new row on top
            */
        }

        /// <summary>
        /// Generate new row on bottom and cleanup top row
        /// </summary>
        void ShiftDown()
        {
            ShiftAllStars(Vector2.up);
            //virtualPosition.y--;  // y = -1
            instance.virtualPosition.y--;  // y = -1

            List<GameObject> newStars = GetRowOfStars((int)instance.virtualPosition.y - 40);
            foreach (GameObject s in newStars)
                allStars.Add(s);

            /*
            GameObject[] newStars = GetRowOfStars((int)virtualPosition.y, -40);
            foreach (GameObject s in starsList[0])  // first row
                if (s != null)
                    Destroy(s);
            starsList.RemoveAt(0);
            starsList.Add(newStars);
            */
        }


        void ShiftRight()
        {
            ShiftAllStars(Vector2.left);
            //virtualPosition.x++;
            instance.virtualPosition.x++;
            List<GameObject> newStars = GetColumnOfStars((int)instance.virtualPosition.x + 40);

            foreach (GameObject s in newStars)
                allStars.Add(s);

            // Removes old stars from beginning

                
            
            /*
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
            */
        }

        void ShiftLeft()
        {
            ShiftAllStars(Vector2.right);
            //virtualPosition.x--;
            instance.virtualPosition.x--;

            List<GameObject> newStars = GetColumnOfStars((int)instance.virtualPosition.x - 40);

            foreach (GameObject s in newStars)
                allStars.Add(s);

            /*
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
            */
        }





        // ----- Handles UI directional buttons
        public void LeftButton()
        {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftLeft();
            }
        }

        public void RightButton()
        {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftRight();
            }
        }

        public void UpButton()
        {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftUp();
            }
        }

        public void DownButton()
        {
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                // Sector view
                ShiftDown();
            }
        }

        public void ReturnButton()
        {
            if (SceneManager.GetActiveScene().buildIndex == SystemLevel)
            {
                ToSectorView();
            }
            if (SceneManager.GetActiveScene().buildIndex == SectorLevel)
            {
                ToSystemView();
            }
        }


        public void ToSystemView() {
            // Go back to system view
            lastKnownPosition = instance.virtualPosition;
            SceneManager.LoadScene(SystemLevel);
        }

        public void ToSectorView() {
            // Go back to sector view
            instance.virtualPosition = lastKnownPosition;
            SceneManager.LoadScene(SectorLevel);
        }

    }
       
   
}