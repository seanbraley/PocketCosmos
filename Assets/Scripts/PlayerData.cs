using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour {

    public static PlayerData playdata;

    public Boolean initialPlay;      // false if they've played already

    // Player variables that will be saving
    public uint spacebux;
    public Vector2 lastPosition;
    public List<OwnedPlanet> ownedPlanets;
    public List<DiscoveredStar> discoveredStarSystems;


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

        // Initialize
        ownedPlanets = new List<OwnedPlanet> ();
        discoveredStarSystems = new List<DiscoveredStar>();
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
        data.discoveredStarSystems = discoveredStarSystems;

        // Write serializable data to file
        bf.Serialize(file, data);
        file.Close();
    }

    // Load game data - works on all platforms except Web
    public void Load() {
        // Accessing load data
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            // Returning user
            Debug.Log("Returning user... accessing previous game data from " + Application.persistentDataPath + "/playerInfo.dat");
            initialPlay = false;

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
            discoveredStarSystems = data.discoveredStarSystems;
        }
        else {
            // No previous load data exists - they are a new player.
            Debug.Log("First-time user dectected.");
            initialPlay = true;
        }       
    }    

}


// ----- Serializable data containers-----
[Serializable]
public class PlayerInfo {

    public uint spacebux;
    [NonSerialized]
    public Vector2 lastPosition;
    public List<OwnedPlanet> ownedPlanets;
    public List<DiscoveredStar> discoveredStarSystems;
    
}

[Serializable]
public class DiscoveredStar {

    [NonSerialized]
    public DateTime discoveryTime;
    [NonSerialized]
    public GameObject starObj;

    public DiscoveredStar(GameObject g) {
        starObj = g;
        starObj.GetComponent<Star>().Discovered = true;
        // TO DO get time from server
        discoveryTime = System.DateTime.Now;
    }

}


[Serializable]
public class OwnedPlanet {

    [NonSerialized]
    public DateTime discoveryTime;
    [NonSerialized]
    public GameObject planetObj;

    public OwnedPlanet(GameObject p) {
        planetObj = p;
        // TO DO get time from server
        discoveryTime = System.DateTime.Now;
    }
}