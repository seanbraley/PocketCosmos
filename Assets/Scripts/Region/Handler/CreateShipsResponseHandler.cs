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
            //view.LogDebug(response.Parameters[(byte)ClientParameterCode.ShipId].ToString());
            var value = response.Parameters[(byte)ClientParameterCode.ShipId].ToString();
            view.LogDebug("ship id: " + value);
            // Update local data
            PlayerData.instance.shipList.LastOrDefault().id = (uint) int.Parse(value);
        }
        else if (response.ReturnCode == 6)
        {
            // Not enough population
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            // Delete the ship that was added
            PlayerData.instance.shipList.RemoveAt(PlayerData.instance.shipList.Count);
        }
        else if (response.ReturnCode == 7)
        {
            // You don't own this planet
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            // Delete the ship that was added
            PlayerData.instance.shipList.RemoveAt(PlayerData.instance.shipList.Count);
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
        }
    }
}
