using ExitGames.Client.Photon;

//A LoginResponseHandler to deal with login responses from the server

public class LoginResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Login ; }
    }

    public LoginResponseHandler(LoginController controller) : base(controller)
    {
    }

    public override void OnHandleResponse(OperationResponse response)
    {
        StartOptions view = _controller.ControlledView as StartOptions;
        if (response.ReturnCode == 0)
        {
            view.LoginSuccessfull();   
        }
        else
        {
            view.LoginFailure(response.ReturnCode);
        }
    }
}
