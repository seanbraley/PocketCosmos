using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

//A PlayerShipsResponseHandler to deal with ship responses from the server
public class MissionCompleteResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.CompleteShip; }
    }
    public MissionCompleteResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for COMPLETING MISSION.");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.ShipId].ToString());

            // Find the ship by its shipID and remove from ships ilst and lsit of active missions
            var s = PlayerData.instance.shipList.Find(x => x.id == (int)response.Parameters[(byte)ClientParameterCode.ShipId]);
            PlayerData.instance.EndMission(s);
            
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
