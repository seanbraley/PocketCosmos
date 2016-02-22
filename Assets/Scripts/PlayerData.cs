using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour {

    public static PlayerData playdata;

    // Public player variables
    uint spacebux;
    Vector2 lastPosition;
    GameObject[] ownedPlanets;

    // ----- Accessors -----
    public uint Spacebux
    {
        get { return spacebux; }
        set { spacebux = value; }
    }


    // Awake is always called before any Start functions
    // Only called once.
    void Awake()
    {
        //Check if instance already exists
        if (playdata == null)

            //if not, set instance to this
            playdata = this;

        //If instance already exists and it's not this:
        else if (playdata != this)

            //Then destroy this. This enforces our singleton pattern
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Save game data - works on all platforms except Web
    public void Save() {
        // Create binary formatter and file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        // Instantiate new instance of data container
        PlayerInfo data = new PlayerInfo();
        
        // Put data into the container
        data.spacebux = spacebux;
        data.lastPosition = lastPosition;
        data.ownedPlanets = ownedPlanets;

        // Write serializable data to file
        bf.Serialize(file, data);
        file.Close();
    }

    // Load game data - works on all platforms except Web
    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            // Create binary formatter and file stream
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            // Deserialize and cast from generic object to player data
            PlayerInfo data = (PlayerInfo)bf.Deserialize(file);
            file.Close();

            // Update game data
            spacebux = data.spacebux;
            lastPosition = data.lastPosition;
            ownedPlanets = data.ownedPlanets;
        }       
    }    

}


// ----- Serializable data container -----
[Serializable]
class PlayerInfo {

    public uint spacebux;
    [NonSerialized]
    public Vector2 lastPosition;
    public GameObject[] ownedPlanets;

}