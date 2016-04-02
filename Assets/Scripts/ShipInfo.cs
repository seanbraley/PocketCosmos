using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipInfo
{
    //Probably put this class in its own file
    public uint id { get; set; }
    public string name;
    public int ship_class;
    public int origin_planet;
    public uint origin_star;
    public int destination_planet;
    public uint destination_star;
    public int population;
    public long power;
    public DateTime departure_time { get; set; }
    public DateTime arrival_time { get; set; }

    public ShipInfo(int ship_class, int origin_planet, uint origin_star)
    {
        this.id = (uint)origin_planet + origin_star + (uint)DateTime.Now.Ticks;
        this.ship_class = ship_class;
        this.origin_planet = origin_planet;
        this.origin_star = origin_star;
    }

    public ShipInfo(SanShip s)
    {
        this.id = (uint)s.ShipId;
        //this.name = name;
        this.ship_class = s.Class;
        this.origin_planet = s.HomePlanet;
        this.origin_star = (uint)s.HomeStar;
        this.destination_planet = s.DestPlanet;
        this.destination_star = (uint)s.DestStar;
    }

    public ShipInfo()
    {
        //RANDOM INFO FOR DEBUG ONLY
        this.id = (uint)UnityEngine.Random.Range(0, 1000000);
        string name = "USS ";
        // RANDOM CHARACTER STRING
        for (int i = 0; i < UnityEngine.Random.Range(1, 10); i++)
        {
            int num = UnityEngine.Random.Range(0, 26); // Zero to 25
            char let = (char)('a' + num);
            name += let;
        }
        this.name = name;
        this.ship_class = UnityEngine.Random.Range(0, 1);
        float rand = UnityEngine.Random.value;
        if (rand < (1f / 3f))
        {
            this.origin_planet = 0;
            this.destination_planet = 123;
            departure_time = DateTime.MinValue;
            arrival_time = DateTime.MinValue;
        }
        else if (rand < (2f / 3f))
        {
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
                                                        UnityEngine.Random.Range(0, 60),
                                                        0)); //Random Start Time (for now);
            arrival_time = DateTime.Now.Add(new TimeSpan(0,
                                                    0, //UnityEngine.Random.Range(0,1),
                                                    0, //UnityEngine.Random.Range(0,60),
                                                    UnityEngine.Random.Range(0, 60),
                                                    0)); //Random Start Time (for now);
        }
    }
}

