using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

//The spefic ViewController to handle login event
//This clss is responsible for crafting the login mesage to send to the server
//It also has a list of the relevent Login handlers. 
public class NetworkController : ViewController
{
    public NetworkController(View controlledView, byte subOperationCode = 0) : base(controlledView, subOperationCode)
    {
        LoginResponseHandler loginHandler = new LoginResponseHandler(this); // register handler!
        OperationHandlers.Add((byte)loginHandler.Code, loginHandler);

        PlayerProfileResponseHandler profileHandler = new PlayerProfileResponseHandler(this);
        OperationHandlers.Add((byte)profileHandler.Code, profileHandler);

        SpacebuxResponseHandler spacebuxHandler = new SpacebuxResponseHandler(this);
        OperationHandlers.Add((byte)spacebuxHandler.Code, spacebuxHandler);   

        KnownStarsResponseHandler knownstarsHandler = new KnownStarsResponseHandler(this);
        OperationHandlers.Add((byte)knownstarsHandler.Code, knownstarsHandler);

        DiscoveredStarsResponseHandler discoveredstarsHandler = new DiscoveredStarsResponseHandler(this);
        OperationHandlers.Add((byte)discoveredstarsHandler.Code, discoveredstarsHandler);
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
        ControlledView.LogDebug("SENDING PROFILE REGISTRATION REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, false);
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

    public void CollectSpacebux()
    {
        //encrtypt this later
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.Spacebux}
        };

        ControlledView.LogDebug("SENDING SPACEBUX REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Region, Parameters = param }, true, 0, false);
    }

    public void SpendSpacebux(int value)
    {
        //encrtypt this later
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.Spacebux, value},
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.SpendSpacebux}
        };
        ControlledView.LogDebug("SENDING SPACEBUX SPEND REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Region, Parameters = param }, true, 0, false);
    }

    public void RetrieveKnownStars()
    {
        //encrtypt this later
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.KnownStars}
        };

        ControlledView.LogDebug("SENDING KNOWN STARS REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Region, Parameters = param }, true, 0, false);
    }

    public void SendDiscoveredStar(long starID)
    {
        var param = new Dictionary<byte, object>()
        {
            {(byte) ClientParameterCode.DiscoverStar, starID},
            {(byte) ClientParameterCode.SubOperationCode, (int) MessageSubCode.DiscoverStar}
        };
        ControlledView.LogDebug("SENDING DISCOVERED STAR REQUEST");
        //PhotonEngine.Instance.Peer.OpCustom(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, true);
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Region, Parameters = param }, true, 0, false);
    }

    public void SendDiscoveredPlanet(long starID, int planetID)
    {
    }

}
