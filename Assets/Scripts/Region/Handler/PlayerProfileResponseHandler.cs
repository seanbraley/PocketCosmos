using Completed;
using ExitGames.Client.Photon;

//A PlayerProfileResponseHandler to deal with login responses from the server

public class PlayerProfileResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Profile ; }
    }

    public PlayerProfileResponseHandler(PlayerProfileController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {
        
        GameManager view = _controller.ControlledView as GameManager;
        view.LogDebug("GOT A RESPONSE for PROFILE");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Spacebucks].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Homestar].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.LastLocX].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.LastLocY].ToString());
              
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
        }
    }
}
