using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

//The spefic ViewController to handle login event
//This clss is responsible for crafting the login mesage to send to the server
//It also has a list of the relevent Login handlers. 
public class LoginController : ViewController
{
    public LoginController(View controlledView, byte subOperationCode = 0) : base(controlledView, subOperationCode)
    {
        LoginResponseHandler loginHandler = new LoginResponseHandler(this);
        OperationHandlers.Add((byte)loginHandler.Code, loginHandler);
    }

    public void SendLogin(string username, string password)
    {
        //encrtypt this later
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.UserName, username},
            {(byte) ClientParameterCode.Password, password},
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.Login}
        };

        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, false);
        
    }

    public void SendRegister(string username, string password, string email)
    {
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.UserName, username},
            {(byte) ClientParameterCode.Password, password},
            {(byte) ClientParameterCode.Email, email},
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.Register}
        };

        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, false);

    }

}
