using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

//The spefic ViewController to handle login event
//This clss is responsible for crafting the login mesage to send to the server
//It also has a list of the relevent Login handlers. 
public class PlayerProfileController : ViewController
{
    public PlayerProfileController(View controlledView, byte subOperationCode = 0) : base(controlledView, subOperationCode)
    {
        PlayerProfileResponseHandler profileHandler = new PlayerProfileResponseHandler(this);
        OperationHandlers.Add((byte)profileHandler.Code, profileHandler);
    }

    public void RetrieveProfile()
    {
        
        //encrtypt this later
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.Profile}
        };

        ControlledView.LogDebug("SENDING PROFILE REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Region, Parameters = param }, true, 0, false);
        
    }
   

}
