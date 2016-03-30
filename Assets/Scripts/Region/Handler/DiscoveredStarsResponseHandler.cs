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

            List<KnownStar> stars = new List<KnownStar>();
            foreach (SanStarPlayer s in starCollection.StarPlayers)
                stars.Add(new KnownStar(s));

            // Update local data
            //PlayerData.instance.UpdateKnownStars(response.Parameters[(byte)ClientParameterCode.KnownStars]); //TODO DESERIALIZE
            PlayerData.instance.UpdateKnownStars(stars);
        }      
        else
        {
            view.LogDebug("An error has occured.");
        }
    }
}
