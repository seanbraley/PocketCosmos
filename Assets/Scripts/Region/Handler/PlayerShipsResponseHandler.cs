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
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString());

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlShipList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlShipList shipCollection = (XmlShipList)obj;
            reader.Close();

            // Update local data
            foreach (SanShip s in shipCollection.Ships)
                PlayerData.instance.AddNewShip(new ShipInfo(s));
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
        }
    }
}


public class SanShip
{
    public virtual int ShipId { get; set; }
    public virtual int Class { get; set; }
    public virtual int HomePlanet { get; set; }
    public virtual long HomeStar { get; set; }
    public virtual int DestPlanet { get; set; }
    public virtual long DestStar { get; set; }
    public virtual int Population { get; set; }
    public virtual int Power { get; set; }
}

[XmlRoot("ShipList")]
[XmlInclude(typeof(SanShip))] // include type class Person
public class XmlShipList
{
    [XmlElement("ShipArray")]
    //[XmlArrayItem("SanShip", typeof(SanShip))]
    public List<SanShip> Ships { get; set; }
        
}