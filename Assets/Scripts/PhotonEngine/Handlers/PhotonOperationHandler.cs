using ExitGames.Client.Photon;


public abstract class PhotonOperationHandler : IPhotonOperationHandler
{
    protected readonly ViewController _controller;
    public abstract byte Code { get; }

    protected PhotonOperationHandler(ViewController controller)
    {
        _controller = controller;
    }

    public delegate void BeforeResponseRecieved();

    public BeforeResponseRecieved beforeResponseRecieved;

    public delegate void AfterResponseReceived();

    public AfterResponseReceived afterResponseReceived;

    public void HandleResponse(OperationResponse response)
    {
        if (beforeResponseRecieved != null)
        {
            beforeResponseRecieved();
        }
        OnHandleResponse(response);
        if (afterResponseReceived != null)
        {
            afterResponseReceived();
        }
    }

    public abstract void OnHandleResponse(OperationResponse response);

}
