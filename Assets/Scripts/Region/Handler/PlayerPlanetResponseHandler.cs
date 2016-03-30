using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//A PlayerPlanetResponseHandler to deal with planet responses from the server
public class PlayerPlanetResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.KnownPlanet; }
    }
    public PlayerPlanetResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN Planets");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Planets].ToString());

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.Planets].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlPlanetList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlPlanetList planetCollection = (XmlPlanetList)obj;
            reader.Close();

            // Update local data
            foreach (SanPlanet p in planetCollection.Planets)
                PlayerData.instance.AddKnownPlanet(new OwnedPlanet(p));
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
        }
    }
}



public class SanPlanet
{
    public int PlanetNum { get; set; }
    public long StarId { get; set; }
    public bool PlayerOwned { get; set; }
    public long Population { get; set; }
    public int Power { get; set; }
    public DateTime LastCollected { get; set; }
}


[XmlRoot("PlanetList")]
[XmlInclude(typeof(SanPlanet))] // include type class Person
public class XmlPlanetList
{
    [XmlElement("PlanetArray")]
    public List<SanPlanet> Planets { get; set; }
    
}