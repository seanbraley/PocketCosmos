using UnityEngine;
using System.Collections;

namespace Completed
{
    using System.Collections.Generic;       //Allows us to use Lists. 

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        private GalaxyManager galaxyScript;                       //Store a reference to our GalaxyManager which will set up the level.
        private int level = 1;                                  //Current sector number, expressed in game as "Sector 1".

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
            //Call the SetupScene function of the GalaxyManager script, pass it current level number.
            galaxyScript.SetupScene(level);

        }



        //Update is called every frame.
        void Update()
        {

        }
    }

}