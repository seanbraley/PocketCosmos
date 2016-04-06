using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

//A KnownStarsResponseHandler to deal with knwon star responses from the server

public class KnownStarsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.KnownStars; }
    }

    public KnownStarsResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN STARS");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.KnownStars].ToString());

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.KnownStars].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlStarPlayerList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlStarPlayerList starCollection = (XmlStarPlayerList)obj;
            reader.Close();

            List<KnownStar> stars = new List<KnownStar>();
            foreach (SanStarPlayer s in starCollection.StarPlayers)
                stars.Add(new KnownStar(s));

            // Update local data
            PlayerData.instance.UpdateKnownStars(stars);
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}


public class SanStarPlayer
{
    public long StarId { get; set; }
    public DateTime LastVisited { get; set; }
}


[XmlRoot("StarPlayerList")]
[XmlInclude(typeof(SanStarPlayer))] // include type class Person
public class XmlStarPlayerList
{
    [XmlElement("StarPlayerArray")]
    //[XmlArrayItem("Planet", typeof(SanPlanet))]
    public List<SanStarPlayer> StarPlayers { get; set; }
}