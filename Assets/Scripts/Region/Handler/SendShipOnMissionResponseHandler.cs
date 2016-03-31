using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

//A PlayerShipsResponseHandler to deal with ship responses from the server
public class SendShipOnMissionResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.SendShip; }
    }
    public SendShipOnMissionResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for CREATING SHIP");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.ShipId].ToString());           
            DisplayManager.Instance.DisplayMessage("Ship Launched!");
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
