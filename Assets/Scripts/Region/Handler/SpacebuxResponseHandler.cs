using Completed;
using ExitGames.Client.Photon;

//A PlayerProfileResponseHandler to deal with spacebux responses from the server

public class SpacebuxResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Spacebux; }
    }

    public SpacebuxResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for SPACEBUX");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Spacebux].ToString());

            // Update local data
            PlayerData.instance.UpdateSpacebux((int)response.Parameters[(byte)ClientParameterCode.Spacebux]);
        }
        if (response.ReturnCode == 5)
        {
            view.LogDebug("Not enough spacebux to spend!");
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
        }
    }
}
