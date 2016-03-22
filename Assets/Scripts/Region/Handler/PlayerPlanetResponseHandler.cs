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
            //view.LogDebug(response.Parameters[(byte)ClientParameterCode.Planets].ToString());

            // Deserialize
            var xmlData = @response.Parameters[(byte)ClientParameterCode.Planets].ToString();
            var planetCollection = XmlPlanetList.LoadFromText(xmlData);

            // Update local data
            // PlayerData.instance.UpdateOwnedPlanets((long[])response.Parameters[(byte)ClientParameterCode.KnownStars]); // TODO: FIX THIS
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
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
    [XmlArray("PlanerArray")]
    [XmlArrayItem("Planet")]
    public List<SanPlanet> Planets { get; set; }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(XmlPlanetList));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static XmlPlanetList Load(string path)
    {
        var serializer = new XmlSerializer(typeof(XmlPlanetList));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as XmlPlanetList;
        }
    }

    public static XmlPlanetList LoadFromText(string text) {
        var serializer = new XmlSerializer(typeof(XmlPlanetList));
        return serializer.Deserialize(new StringReader(text)) as XmlPlanetList;
    }
}