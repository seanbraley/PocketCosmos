using Completed;
using ExitGames.Client.Photon;
//A PlayerShipsResponseHandler to deal with knwon star responses from the server
public class PlayerShipsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.PlayerShips; }
    }
    public PlayerShipsResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN SHIPS");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.PlayerShips].ToString());
            // Update local data
            //PlayerData.instance.UpdateKnownStars((long[])response.Parameters[(byte)ClientParameterCode.KnownStars]);
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
        }
    }
}