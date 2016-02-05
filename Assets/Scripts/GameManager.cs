﻿using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;


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
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        private Vector2 virtualPosition = Vector2.zero;

        private int movementCounterX = 0;
        private int movementCounterY = 0;

        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public GameObject[] starPrefabs;

        public List<GameObject> stars = new List<GameObject>();

        public Dictionary<uint, GameObject> starsDic = new Dictionary<uint, GameObject>();

        public GameObject[,] allStars = new GameObject[80, 80];

        static List<GameObject[]> starsList = new List<GameObject[]>();

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
            // rows iterate -40 --> 40 (inner)
            // columns iterate 40 --> -40
            // Hold y constant while iterating through x's
            for (int y = 40; y > -40; y--) // Y value
            {
                GameObject[] tmp = new GameObject[80];
                for (int x = -40; x < 40; x++) // X value
                {
                    if (Procedural.StarExists(x, y))
                    {
                        tmp[x + 40] = (GameObject)Instantiate(starPrefabs[0], new Vector2(x, y), Quaternion.identity);
                        //Debug.Log(string.Format("Star Created at: <{0}, {1}>", x, y));
                    }

                    //uint num = Procedural.PointToNumber(j, i);
                    //BitArray b = new BitArray(new int[] { (int)num });

                    //bool starExists = Procedural.StarExists(x, y);

                    //bool starExists = (b[5] & b[4]) ^ b[18];

                    /* off-grid
                    byte[] tmp = System.BitConverter.GetBytes(num);
                    ushort first = System.BitConverter.ToUInt16(tmp, 0);
                    ushort second = System.BitConverter.ToUInt16(tmp, 1);

                    float x = (first % 100) / 100f;
                    float y = (second % 100) / 100f;
                    */
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
            GameObject[] newStars = new GameObject[80];
            for (int i = -40; i < 40; i++)
                if (Procedural.StarExists(i, virtualPos))
                    newStars[i+40] = (GameObject)Instantiate(starPrefabs[0], new Vector2(i, y), Quaternion.identity);

            return newStars;
        }


        GameObject[] GetColumnOfStars(int x, int virtualPos)
        {
            GameObject[] newStars = new GameObject[80];
            int helper = 0;
            for (int i = 40; i > -40; i--) // iterate from up/down positive y to negative y
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
                        s.transform.Translate(direction);
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
            GameObject[] newStars = GetRowOfStars(-39, (int)virtualPosition.y - 40); // -41
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
            GameObject[] newStars = GetColumnOfStars(-39, (int)virtualPosition.x - 40);
            int helper = 0;
            foreach (GameObject[] starRow in starsList)
            {
                if (starRow[79] != null)
                    Destroy(starRow[79]);
                GameObject[] newArr = new GameObject[starRow.Length];
                System.Array.Copy(starRow, 0, newArr, 1, starRow.Length - 1);
                newArr[0] = newStars[helper];
                starsList[helper++] = newArr;
            }
            ShiftAllStars(Vector2.right);
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
                watch.Reset();
                watch.Start();
                ShiftUp();
                watch.Stop();
                Debug.Log(string.Format("Shift Up took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                watch.Reset();
                watch.Start();
                ShiftDown();
                watch.Stop();
                Debug.Log(string.Format("Shift Down took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                watch.Reset();
                watch.Start();
                ShiftLeft();
                watch.Stop();
                Debug.Log(string.Format("Shift Left took: {0}ms", watch.ElapsedMilliseconds));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                watch.Reset();
                watch.Start();
                ShiftRight();
                watch.Stop();
                Debug.Log(string.Format("Shift Right took: {0}ms", watch.ElapsedMilliseconds));
            }
        }
    }
}