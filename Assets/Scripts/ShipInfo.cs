using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipInfo
{
    //Probably put this class in its own file
    public uint id;
    public string name;
    public string ship_class;
    public uint origin_planet;
    public uint destination_planet;
    public DateTime departure_time;
    public DateTime arrival_time;

    public ShipInfo(uint id, string name, string ship_class, uint origin_planet, uint destination_planet, DateTime departure_time, DateTime arrival_time)
    {
        this.id = id;
        this.name = name;
        this.ship_class = ship_class;
        this.origin_planet = origin_planet;
        this.destination_planet = destination_planet;
        this.departure_time = departure_time;
        this.arrival_time = arrival_time;
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
        string[] classes = new string[] { "Colony Carrier", "Research Racer" };
        this.ship_class = classes[UnityEngine.Random.Range(0, classes.Length)];
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

