using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;


//This class is the bridge between our view/scene and the photon messages
//Like the server it has a set of message handlers that will take events and responses
//from photon. Different handlers will be implemented with specific functionality
public class ViewController : IViewController
{

    private readonly View _controlledView;
    private readonly byte _subOperationCode;
    public View ControlledView { get { return _controlledView; } }
    private readonly Dictionary<byte, IPhotonOperationHandler> _operationHandlers = new Dictionary<byte, IPhotonOperationHandler>();
    private readonly Dictionary<byte, IPhotonEventHandler> _eventHandlers = new Dictionary<byte, IPhotonEventHandler>();

    public ViewController(View controlledView, byte subOperationCode = 0)
    {
        _controlledView = controlledView;
        _subOperationCode = subOperationCode;
        if (PhotonEngine.Instance == null)
        {
            // Application.loadedLevel(0); start the 1st screen
        }
        else
        {
            PhotonEngine.Instance.Controller = this;
        }
    }
    public Dictionary<byte, IPhotonOperationHandler> OperationHandlers { get { return _operationHandlers; } }
    public Dictionary<byte, IPhotonEventHandler> EventHandlers { get { return _eventHandlers; } }

    #region Implementation of IViewController
    public bool IsConnected
    {
        get { return PhotonEngine.Instance.State is Connected; }
    }


    public void ApplicationQuit()
    {
        PhotonEngine.Instance.Disconnect();
    }

    public void Connect()
    {
        if (!IsConnected)
        {
            PhotonEngine.Instance.Initialize();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        _controlledView.LogDebug(string.Format("{0} - {1}", level, message));
    }

    public void OnDisconnect(string message)
    {
        _controlledView.Disconnected(message);
    }

    public void OnEvent(EventData eventData)
    {
        IPhotonEventHandler handler;
        if (EventHandlers.TryGetValue(eventData.Code, out handler))
        {
            handler.HandleEvent(eventData);
        }
        else
        {
            OnUnexpectedEvent(eventData);
        }
    }

    public void OnOpertaionResponse(OperationResponse operationResponse)
    {
        IPhotonOperationHandler handler;
        bool hasSubcode = operationResponse.Parameters.ContainsKey((byte) _subOperationCode);
        ControlledView.LogDebug(string.Format("looing for Key {0} and {1} ", operationResponse.Parameters.Keys.Count, true) );
        if (hasSubcode && OperationHandlers.TryGetValue(Convert.ToByte(operationResponse.Parameters[(byte)_subOperationCode]),
                out handler))
        {
           
            handler.HandleResponse(operationResponse);
        }
        else
        {
            OnUnexpectedOperationResponse(operationResponse);
        }
    }

    public void OnUnexpectedEvent(EventData eventData)
    {
        _controlledView.LogError(string.Format("Unecpected Event {0}", eventData.Code));
    }

    public void OnUnexpectedOperationResponse(OperationResponse operationResponse)
    {

        _controlledView.LogError(string.Format("Unecpected operation error {0} from operation {1}", operationResponse.ReturnCode, operationResponse.OperationCode));
    }

    public void OnUnexpectedStatusCode(StatusCode statusCode)
    {
        _controlledView.LogError(string.Format("Unecpected Status {0}", statusCode));
    }

    public void SendOperation(OperationRequest request, bool sendReliable, byte channelId, bool encrypt)
    {
        PhotonEngine.Instance.SendOp(request, sendReliable, channelId, encrypt);


    }

    #endregion
}

