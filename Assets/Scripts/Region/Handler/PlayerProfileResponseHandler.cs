using Completed;
using ExitGames.Client.Photon;

//A PlayerProfileResponseHandler to deal with login responses from the server

public class PlayerProfileResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Profile ; }
    }

    public PlayerProfileResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        //NetworkManager view = _controller.ControlledView as NetworkManager;
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for PROFILE");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Spacebux].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Homestar].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.LastLocX].ToString());
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.LastLocY].ToString());

            // Update local player data according to server
            PlayerData.instance.UpdateLocalData(
                (int)response.Parameters[(byte)ClientParameterCode.Spacebux], 
                (long)response.Parameters[(byte)ClientParameterCode.Homestar],
                (int)response.Parameters[(byte)ClientParameterCode.LastLocX],
                (int)response.Parameters[(byte)ClientParameterCode.LastLocY]
                );
           
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
