using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

//A PlayerShipsResponseHandler to deal with ship responses from the server
public class CreateShipsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.CreateShip; }
    }
    public CreateShipsResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for CREATING SHIP");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString());
            DisplayManager.Instance.DisplayMessage("Ship Created!");

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlShipList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlShipList shipCollection = (XmlShipList)obj;
            reader.Close();

            // Update local data - add this one ship
            foreach (SanShip s in shipCollection.Ships)
                PlayerData.instance.AddNewShip(new ShipInfo(s));

        }
        else if (response.ReturnCode == 6)
        {
            // Not enough population
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
        else if (response.ReturnCode == 7)
        {
            // You don't own this planet
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
