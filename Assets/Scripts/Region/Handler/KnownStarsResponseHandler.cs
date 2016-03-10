using Completed;
using ExitGames.Client.Photon;

//A KnownStarsResponseHandler to deal with knwon star responses from the server

public class KnownStarsResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.KnownStars; }
    }

    public KnownStarsResponseHandler(NetworkController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {

        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN STARS");
        if (response.ReturnCode == 0)
        {
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.KnownStars].ToString());

            // Update local data
            PlayerData.instance.UpdateKnownStars((long[])response.Parameters[(byte)ClientParameterCode.KnownStars]);
        }
        else
        {
            view.LogDebug("WHY ARE WE HERE");
        }
    }
}
