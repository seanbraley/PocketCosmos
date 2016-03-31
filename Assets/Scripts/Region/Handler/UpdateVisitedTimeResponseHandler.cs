using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//A PlayerPlanetResponseHandler to deal with planet responses from the server
public class UpdateVisitedTimeResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.UpdateTime; }
    }
    public UpdateVisitedTimeResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for UPDATE TIME");
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

            // Update local data
            foreach (SanStarPlayer s in starCollection.StarPlayers)
                PlayerData.instance.UpdateLastVisitedTime(new KnownStar(s));
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
