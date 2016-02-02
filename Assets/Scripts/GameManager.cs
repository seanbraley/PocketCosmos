using UnityEngine;
using System.Collections;

namespace Completed
{
    using System.Collections.Generic;       //Allows us to use Lists. 

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        private GalaxyManager galaxyScript;                       //Store a reference to our GalaxyManager which will set up the level.
                                                                  //private int level = 1;                                  //Current sector number, expressed in game as "Sector 1".
        private int offsetX;
        private int offsetY;

        public GameObject[] starPrefabs;

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

            //Get a component reference to the attached GalaxyManager script
            galaxyScript = GetComponent<GalaxyManager>();

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            offsetX = 0;
            offsetY = 0;

            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    uint num = Procedural.PointToNumber(i + offsetX, j + offsetY);
                    BitArray b = new BitArray(new int[] { (int)num });
                    //Debug.Log(num);
                    bool starExists = b[5] ^ b[4];

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
                        Instantiate(starPrefabs[0], new Vector2(i, j), Quaternion.identity);
                    }
                }
            }


            //Call the SetupScene function of the GalaxyManager script, pass it current level number.
            //galaxyScript.SetupScene(level);

        }



        //Update is called every frame.
        void Update()
        {

        }
    }

}