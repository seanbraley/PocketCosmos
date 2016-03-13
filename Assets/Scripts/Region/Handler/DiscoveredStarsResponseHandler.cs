using Completed;
using ExitGames.Client.Photon;

//A DiscoveredStarsResponseHandler to deal with discovered star responses from the server

public class DiscoveredStarsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.DiscoverStar; }
    }

    public DiscoveredStarsResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for DISCOVERED STAR");
        if (response.ReturnCode == 0)
        {
            view.LogDebug("Star successfully discovered.");
            // Update local data
            //PlayerData.instance.UpdateSpacebux((int)response.Parameters[(byte)ClientParameterCode.DiscoverStar]);
            PlayerData.instance.UpdateKnownStars((long[])response.Parameters[(byte)ClientParameterCode.KnownStars]);
        }
        else
        {
            view.LogDebug("Error: unable to handle DISCOVERED STARS response.");
        }
    }
}
