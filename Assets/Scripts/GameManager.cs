using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;

namespace Completed
{
    
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        private int offsetX;
        private int offsetY;

        private Vector2 virtualPosition = Vector2.zero;

        private int movementCounterX = 0;
        private int movementCounterY = 0;
        
        public GameObject[] starPrefabs;

        public List<GameObject> stars = new List<GameObject>();

        //Awake is always called before any Start functions
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

        //Initializes the game level.
        void InitGame()
        {
            offsetX = 0;
            offsetY = 0;

            for (int i = -40; i < 40; i++)
            {
                for (int j = -40; j < 40; j++)
                {
                    uint num = Procedural.PointToNumber(i + offsetX, j + offsetY);
                    BitArray b = new BitArray(new int[] { (int)num });

                    bool starExists = Procedural.StarExists(i, j);
                    
                    //bool starExists = (b[5] & b[4]) ^ b[18];

                    /* off-grid
                    byte[] tmp = System.BitConverter.GetBytes(num);
                    ushort first = System.BitConverter.ToUInt16(tmp, 0);
                    ushort second = System.BitConverter.ToUInt16(tmp, 1);

                    float x = (first % 100) / 100f;
                    float y = (second % 100) / 100f;
                    */
                    if (starExists)
                    {
                        Debug.Log(string.Format("Star Created at: <{0}, {1}>", i, j));
                        stars.Add((GameObject)Instantiate(starPrefabs[0], new Vector2(i, j), Quaternion.identity));
                    }
                }
            }

        }



        //Update is called every frame.
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                foreach (GameObject s in stars)
                {
                    s.transform.Translate(Vector2.down);
                }
                movementCounterY--;
                if (movementCounterY < -10)
                {
                    movementCounterY = 0;
                    // Generate row of 10 accross top and remove row along bottom
                    
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (GameObject s in stars)
                {
                    s.transform.Translate(Vector2.up);
                }
                movementCounterY++;
                if (movementCounterY > 10)
                {
                    movementCounterY = 0;
                    // Generate row of 10 accross top and remove row along bottom

                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (GameObject s in stars)
                {
                    s.transform.Translate(Vector2.right);
                }
                movementCounterX++;
                if (movementCounterX > 10)
                {
                    movementCounterX = 0;
                    // Generate row of 10 accross top and remove row along bottom

                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                foreach (GameObject s in stars)
                {
                    s.transform.Translate(Vector2.left);
                }
                movementCounterX--;
                if (movementCounterY < -10)
                {
                    movementCounterX = 0;
                    // Generate row of 10 accross top and remove row along bottom

                }
            }
        }
    }

}