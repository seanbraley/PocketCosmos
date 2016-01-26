/*
 * Manages procedural generation and placement of galaxies
 * Code modified from: https://unity3d.com/learn/tutorials/projects/2d-roguelike/boardmanager?playlist=17150
 */

using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Completed

{

	public class CosmosManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum;             //Minimum value for our Count class.
			public int maximum;             //Maximum value for our Count class.


			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}


		public int columns = 8;                                         //Number of columns in cosmos board.
		public int rows = 8;                                            //Number of rows in cosmos board.
		public GameObject[] systemTiles;                                //Array of galaxy prefabs.
        public ulong randSeed;                                          //64-bit unsigned integer seed for RNG

        private Transform cosmosHolder;                                  //A variable to store a reference to the transform of Cosmos object.
		private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.


		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();

			//Loop through x axis (columns).
			for(int x = 1; x < columns-1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < rows-1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}


		//Sets up the outer walls and floor (background) of the space board.
		void CosmosSetup ()
		{
            //Instantiate Cosmos and set cosmosHolder to its transform.
            cosmosHolder = new GameObject ("Cosmos").transform;

			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -1; x < columns + 1; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = -1; y < rows + 1; y++)
				{
                    // TODO: this needs to be changed to use random seed and xxHash
                    // http://blogs.unity3d.com/2015/01/07/a-primer-on-repeatable-random-numbers/
                    //Choose a random tile from our array of galaxy tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = systemTiles[Random.Range (0, systemTiles.Length)];

					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to cosmosHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent (cosmosHolder);
				}
			}
		}
              

		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
            //Creates the grid with galaxies
            CosmosSetup();

			//Reset our list of gridpositions.
			InitialiseList ();            
		}
    } // end CosmosManager
}