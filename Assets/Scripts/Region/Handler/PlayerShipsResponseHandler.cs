using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//A PlayerShipsResponseHandler to deal with ship responses from the server
public class PlayerShipsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.PlayerShips; }
    }
    public PlayerShipsResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN SHIPS");
        if (response.ReturnCode == 0)
        {
            // view.LogDebug(response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString());

            // Deserialize
            var xmlData = @response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString();
            var shipCollection = XmlShipList.LoadFromText(xmlData);
            foreach (SanShip s in shipCollection.Ships)
                PlayerData.instance.shipList.Add(new ShipInfo(s));

            // Update local data
            //PlayerData.instance.UpdateKnownStars((long[])response.Parameters[(byte)ClientParameterCode.KnownStars]);
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
        }
    }
}


[XmlRoot("SHIP")]
public class SanShip
{
    public virtual int ShipId { get; set; }
    [XmlIgnoreAttribute]
    public virtual int UserId { get; set; }
    public virtual int Class { get; set; }
    public virtual int HomePlanet { get; set; }
    public virtual long HomeStar { get; set; }
    public virtual int DestPlanet { get; set; }
    public virtual long DestStar { get; set; }
    public virtual int Population { get; set; }
    public virtual long Power { get; set; }
    public virtual DateTime StartTime { get; set; }
    public virtual DateTime EndTime { get; set; }
}

[XmlRoot("ShipList")]
[XmlInclude(typeof(SanShip))] // include type class Person
public class XmlShipList
{
    [XmlArray("ShipArray")]
    [XmlArrayItem("SanShip")]
    public List<SanShip> Ships { get; set; }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(XmlShipList));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static XmlShipList Load(string path)
    {
        var serializer = new XmlSerializer(typeof(XmlShipList));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as XmlShipList;
        }
    }

    public static XmlShipList LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(XmlShipList));
        return serializer.Deserialize(new StringReader(text)) as XmlShipList;
    }
}