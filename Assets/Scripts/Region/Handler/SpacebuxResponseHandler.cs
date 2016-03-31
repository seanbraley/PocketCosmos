using Completed;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using ExitGames.Client.Photon;
using System.Globalization;

//A PlayerProfileResponseHandler to deal with spacebux responses from the server

public class SpacebuxResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Spacebux; }
    }

    public SpacebuxResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for SPACEBUX");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Time].ToString());
            DisplayManager.Instance.DisplayMessage("Spacebuck Collected");

            // Deserialize
            XmlSerializer serializer = new XmlSerializer(typeof(DateTime));
            DateTime collectionTime;
            using (StringReader textReader = new StringReader(response.Parameters[(byte)ClientParameterCode.Time].ToString()))
            {
                collectionTime = (DateTime)serializer.Deserialize(textReader);
            }

            // Update local data
            //PlayerData.instance.UpdateSpacebux((int)); // TODO: FIX THIS
        }
        else if (response.ReturnCode == 5)
        {
            view.LogDebug("Not enough spacebux to spend!");
            DisplayManager.Instance.DisplayMessage("Not Enough Spacebux!");
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
