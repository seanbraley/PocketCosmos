using Completed;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

//A DiscoveredStarsResponseHandler to deal with discovered star responses from the server

public class DiscoveredStarsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.DiscoverStar; }
    }

    public DiscoveredStarsResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for DISCOVERED STAR");
        if (response.ReturnCode == 0)
        {
            view.LogDebug("Star successfully discovered.");

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.KnownStars].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlStarPlayerList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlStarPlayerList starCollection = (XmlStarPlayerList)obj;
            reader.Close();

            // Update local data
            foreach (SanStarPlayer s in starCollection.StarPlayers)
                PlayerData.instance.AddDiscoveredStar(new KnownStar(s));
            
        }      
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
        }
    }
}
