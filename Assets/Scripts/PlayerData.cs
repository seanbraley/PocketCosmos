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
    public List<KnownStar> discoveredStarSystems;
    public List<ShipInfo> shipList;


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

        shipList = new List<ShipInfo>();

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

    // update list of all known stars for a player
    public void UpdateKnownStars(List<KnownStar> stars)
    {
        discoveredStarSystems = new List<KnownStar>(stars);
    }
    
    // add a newly discovered star
    public void AddDiscoveredStar(KnownStar s)
    {
        discoveredStarSystems.Add(s); ;
    }

    // update list of all known ships for a player
    public void AddNewShip(ShipInfo s)
    {
        shipList.Add(s);
    }

    // update list of owned planets for a player
    public void AddOwnedPlanet(OwnedPlanet p)
    {
        p.playerOwned = 1;
        ownedPlanets.Add(p);
    }

    // update list of known planets for a player
    public void AddKnownPlanet(OwnedPlanet p)
    {
        ownedPlanets.Add(p);
    }


    // Checks whether the ownership status of this planet
    public int CheckPlanetStatus(long starID, int planetID)
    {
        var p = ownedPlanets.Find(x => x.starID == starID && x.planetID == planetID);
        if (p == null) {
            // not a known planet
            return 2;
        }
        if (p.playerOwned == 1)
        {
            // player owns it
            return 1;
        }
        else {
            // someone else owns it
            return 0;
        }
    }

    public long GetPlanetPopulation(long starID, int planetID)
    {
        var p = ownedPlanets.Find(x => x.starID == starID && x.planetID == planetID);
        if (p == null)
        {
            // not a known planet
            return 0;
        }
        else
        {
            return p.planetpopulation;
        }
    }

    public void SetPlanetPopulation(long starID, int planetID, long amount)
    {
        var p = ownedPlanets.Find(x => x.starID == starID && x.planetID == planetID);
        if (p != null)
        {
            p.planetpopulation = amount;
        }
    }

    public DateTime GetPlanetLastCollectedTime(long starID, int planetID)
    {
        var p = ownedPlanets.Find(x => x.starID == starID && x.planetID == planetID);
        if (p == null)
        {
            // not a known planet
            return DateTime.Now;
        }
        else
        {
            return p.lastcollectedtime;
        }
    }

    public void SetPlanetLastCollectedTime(long starID, int planetID)
    {
        var p = ownedPlanets.Find(x => x.starID == starID && x.planetID == planetID);
        if (p != null)
        {
            // not a known planet
            p.lastcollectedtime = DateTime.Now;
        }
    }

    public DateTime GetLastVisitedTime(long starID)
    {
        var s = discoveredStarSystems.Find(x => x.starID == starID);
        if (s == null)
        {
            // not a known planet
            return DateTime.Now;
        }
        else
        {
            return s.lastVisited;
        }
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
    public List<KnownStar> discoveredStarSystems;
    
}


[Serializable]
public class OwnedPlanet {

    public DateTime lastcollectedtime { get; set; }

    public long planetpopulation { get; set; }
    public int planetpower { get; set; }

    public long starID { get; set; } // which star it orbits
    public int planetID { get; set; }
    public int playerOwned { get; set; } // 0 = enemy owned, 1 = you own, 2 = unknown/unoccupied

    public OwnedPlanet(GameObject p)
    {
        starID = p.GetComponent<Planet>().homeStar.myNumber;
        planetID = p.GetComponent<Planet>().planetNum;
        lastcollectedtime = new DateTime();
    }


    public OwnedPlanet(SanPlanet p)
    {
        starID = p.StarId;
        planetID = p.PlanetNum;
        if (p.PlayerOwned)
        {
            playerOwned = 1;  // 1 = you own
        }
        else {
            playerOwned = 0;  // 0 = enemy owned
        }
        planetpopulation = p.Population;
        planetpower = p.Power;
        lastcollectedtime = p.LastCollected;
    }

}



[Serializable]
public class KnownStar
{
    public long starID { get; set; } // which star it orbits
    public DateTime lastVisited { get; set; }
    
    public KnownStar(SanStarPlayer s)
    {
        starID = s.StarId;
        lastVisited = s.LastVisited;
    }

}