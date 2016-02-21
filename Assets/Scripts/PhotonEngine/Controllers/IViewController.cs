using ExitGames.Client.Photon;


public interface IViewController
{

    bool IsConnected { get; }

    void Connect();
    void SendOperation(OperationRequest request, bool sendReliable, byte channelId, bool encrypt);

    #region Implementation of IPhotonPeerListener

    void DebugReturn(DebugLevel level, string message);
    void OnOpertaionResponse(OperationResponse operationResponse);
    void OnEvent(EventData eventData);

    void OnUnexpectedEvent(EventData eventData);
    void OnUnexpectedOperationResponse(OperationResponse operationResponse);
    void OnUnexpectedStatusCode(StatusCode statusCode);

    void OnDisconnect(string message);

    void ApplicationQuit();



    #endregion
}

