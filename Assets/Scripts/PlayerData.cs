using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    public Boolean initialPlay;      // false if they've played already

    // Player variables that will be saving
    public int spacebux;
    public long homestarID;
    public Vector2 lastPosition;
    public List<OwnedPlanet> ownedPlanets;
    public List<long> discoveredStarSystems;


    // ----- Accessors -----
    public int Spacebux
    {
        get { return spacebux; }
        set { spacebux = value; }
    }


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
        data.discoveredStarSystems = discoveredStarSystems;

        // Write serializable data to file
        bf.Serialize(file, data);
        file.Close();
    }

    // Update local data from server data
    public void UpdateLocalData(int s, long id, int x, int y)
    {
        spacebux = s;
        homestarID = id;
        lastPosition = new Vector2(x, y);
        // TODO should we update the known stars and planets too?
    }

    // update the amount of spacebux held
    public void UpdateSpacebux(int value) {
        spacebux = value;
    }

    public void UpdateLastCollected(DateTime value, long starID, int planetID) {
    }

    // update list of all known stars for a player
    public void UpdateKnownStars(long[] value) {
        discoveredStarSystems = new List<long>(value); ;
    }

    // add a newly discovered star
    public void AddDiscoveredStar(long value)
    {
        discoveredStarSystems.Add(value); ;
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
            homestarID = data.homestarID;
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

    public Boolean initialPlay;      // false if they've played already

    // Player variables that will be saving
    public int spacebux;
    public long homestarID;

    private int _positionX;
    private int _positionY;

    //[NonSerialized]
    public Vector2 lastPosition
    {
        get
        {
            return new Vector2(_positionX, _positionY);
        }
        set
        {
            _positionX = (int)value.x;
            _positionY = (int)value.y;
        }
    }


    public List<OwnedPlanet> ownedPlanets;
    public List<long> discoveredStarSystems;
    
}


[Serializable]
public class OwnedPlanet {

    private DateTime lastcollectedtime;
    public DateTime LastCollectedTime
    {
        get { return lastcollectedtime; }
        set { lastcollectedtime = value; }
    }

    public long starID { get; private set; } // which star it orbits
    public int planetID { get; private set; }

    public OwnedPlanet(GameObject p)
    {
        // p.GetComponent<Planet>().Discovered = true;  // TODO - implement this
        // TO DO get time from server
        starID = p.GetComponent<Planet>().homeStar.myNumber;
        planetID = p.GetComponent<Planet>().planetNum;
        LastCollectedTime = new DateTime();

    }
    
}

[Serializable]
public class ShipInfo {
    //Probably put this class in its own file
    public uint id;
    public string name;
    public string ship_class;
    public uint origin_planet;
    public uint destination_planet;
    public DateTime departure_time;
    public DateTime arrival_time;

    public ShipInfo(uint id, string name, string ship_class, uint origin_planet, uint destination_planet, DateTime departure_time, DateTime arrival_time) {
        this.id = id;
        this.name = name;
        this.ship_class = ship_class;
        this.origin_planet = origin_planet;
        this.destination_planet = destination_planet;
        this.departure_time = departure_time;
        this.arrival_time = arrival_time;
    }

    public ShipInfo() {
        //RANDOM INFO FOR DEBUG ONLY
        this.id = (uint)UnityEngine.Random.Range(0,1000000);
        string name = "USS ";
        // RANDOM CHARACTER STRING
        for(int i = 0; i < UnityEngine.Random.Range(1,10); i++) {
            int num = UnityEngine.Random.Range(0, 26); // Zero to 25
            char let = (char)('a' + num);
            name += let;
        }
        this.name = name;
        string[] classes = new string[] {"Colony Carrier", "Research Racer"};
        this.ship_class = classes[UnityEngine.Random.Range(0,classes.Length)];
        float rand = UnityEngine.Random.value;
        if (rand < (1f/3f)) {
            this.origin_planet = 0;
            this.destination_planet = 123;
            departure_time = DateTime.MinValue;
            arrival_time = DateTime.MinValue;
        }
        else if (rand < (2f/3f)) {
            this.origin_planet = 123;
            this.destination_planet = 0;
            departure_time = DateTime.MinValue;
            arrival_time = DateTime.MinValue;
        }
        else {
            this.origin_planet = 123;
            this.destination_planet = 456;
            departure_time = DateTime.Now.Subtract(new TimeSpan(0,
                                                        0, //UnityEngine.Random.Range(0,1),
                                                        0, //UnityEngine.Random.Range(0,60),
                                                        UnityEngine.Random.Range(0,60),
                                                        0)); //Random Start Time (for now);
            arrival_time  = DateTime.Now.Add(new TimeSpan(0,
                                                    0, //UnityEngine.Random.Range(0,1),
                                                    0, //UnityEngine.Random.Range(0,60),
                                                    UnityEngine.Random.Range(0,60),
                                                    0)); //Random Start Time (for now);
        }
    }
}