using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;

namespace Completed
{

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        private Vector2 virtualPosition = Vector2.zero;

        private int movementCounterX = 0;
        private int movementCounterY = 0;

        public GameObject[] starPrefabs;

        public List<GameObject> stars = new List<GameObject>();

        public Dictionary<uint, GameObject> starsDic = new Dictionary<uint, GameObject>();

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
            for (int i = -40; i < 40; i++)
            {
                for (int j = -40; j < 40; j++)
                {
                    uint num = Procedural.PointToNumber(i, j);
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
                        starsDic.Add(num, (GameObject)Instantiate(starPrefabs[0], new Vector2(i, j), Quaternion.identity));
                    }
                }
            }

        }

        void TryAddStar(int x, int y)
        {
            if (Procedural.StarExists(x, y))
            {
                uint num = Procedural.PointToNumber(x, y);
                starsDic.Add(num, (GameObject)Instantiate(starPrefabs[0], new Vector2(x, y), Quaternion.identity));
            }
        }

        void TryRemoveStar(int x, int y)
        {
            uint num = Procedural.PointToNumber(x, y);
            GameObject tmpStar = null;
            starsDic.TryGetValue(num, out tmpStar);
            if (tmpStar != null)
            {
                starsDic.Remove(num);
                Destroy(tmpStar, 0f);
            }
        }

        //Update is called every frame.
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                movementCounterY--;
                virtualPosition.y++;
                //if (movementCounterY < -10)
                {
                    movementCounterY = 0;
                    // Generate row of 10 accross top and remove row along bottom
                    for (int i = -40; i < 40; i++)
                    {
                        TryAddStar((int)virtualPosition.x + i, (int)virtualPosition.y + 40);
                        TryRemoveStar((int)virtualPosition.x + i, (int)virtualPosition.y - 40);
                    }
                }
                foreach (GameObject s in starsDic.Values)
                {
                    s.transform.Translate(Vector2.down);
                }

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movementCounterY++;
                virtualPosition.y--;
                //if (movementCounterY > 10)
                {
                    movementCounterY = 0;
                    // Generate row of 10 accross botton and remove row along top
                    for (int i = -40; i < 40; i++)
                    {
                        TryAddStar((int)virtualPosition.x + i, (int)virtualPosition.y - 40);
                        TryRemoveStar((int)virtualPosition.x + i, (int)virtualPosition.y + 40);
                    }
                }
                foreach (GameObject s in starsDic.Values)
                {
                    s.transform.Translate(Vector2.up); // y=1
                }

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                movementCounterX++;
                virtualPosition.x--;
                //if (movementCounterX > 10)
                {
                    movementCounterX = 0;
                    // Generate column of 10 accross right and remove column along left
                    for (int i = -40; i < 40; i++)
                    {
                        TryAddStar((int)virtualPosition.x - 40, (int)virtualPosition.y + i);
                        TryRemoveStar((int)virtualPosition.x + 41, (int)virtualPosition.y + i);
                    }
                }
                foreach (GameObject s in starsDic.Values)
                {
                    s.transform.Translate(Vector2.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movementCounterX--;
                virtualPosition.x++;
                //if (movementCounterY < -10)
                {
                    movementCounterX = 0;
                    // Generate column of 10 accross right and remove column along left
                    for (int i = -40; i < 40; i++)
                    {
                        TryAddStar((int)virtualPosition.x + 40, (int)virtualPosition.y + i);
                        TryRemoveStar((int)virtualPosition.x - 41, (int)virtualPosition.y + i);
                    }
                }
                foreach (GameObject s in starsDic.Values)
                {
                    s.transform.Translate(Vector2.left);
                }
            }
        }
    }
}