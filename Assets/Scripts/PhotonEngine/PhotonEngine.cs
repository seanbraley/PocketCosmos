using System;
using System.Net.NetworkInformation;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.Rendering;

//The main connection instance for the program. Is a singleton class
//Monitors and maintains the connection with the sever
//On Update will call the photon dll to check for any recieved or unsent messages
public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public PhotonPeer Peer { get; protected set; }
    public GameState State { get; protected set; }
    public ViewController Controller { get; set; }

    public string ServerAddress;
    public string ApplicationName;

    private static PhotonEngine _instance;

    public void Awake()
    {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a PhotonEngine.
            Destroy(gameObject);
        
        
    }

    public void Start()
    {
        DontDestroyOnLoad(this);
        _instance = this;
        State = new Disconnected(_instance);
        Application.runInBackground = true;
        Initialize();
    }

    public static PhotonEngine Instance { get { return _instance; } }

    public void Initialize()
    {
            Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            Peer.Connect(ServerAddress, ApplicationName);
            State = new WaitingForConnection(_instance);
    }

    public void Disconnect()
    {
        if (Peer != null)
        {
            Peer.Disconnect();
        }
        State = new Disconnected(_instance);
    }

    public void Update()
    {
        State.OnUpdate();
    }

    public void SendOp(OperationRequest request, bool sendReliable, byte channelId, bool encrypt)
    {
        State.SendOperation(request, sendReliable, channelId, encrypt);
    }

    public static void UseExistingOrCreateNewPhotonEngine(string serverAddres, string applicationName)
    {

        GameObject tempEngine;
        PhotonEngine myEngine;

        tempEngine = GameObject.Find("PhtonEngine");
        if (tempEngine == null)
        {
            tempEngine = new GameObject("PhotonEngine");
            tempEngine.AddComponent<PhotonEngine>();
        }

        myEngine = tempEngine.GetComponent<PhotonEngine>();

        myEngine.ApplicationName = applicationName;
        myEngine.ServerAddress = serverAddres;
    }
    #region Implemnetation of IPhotonPeerListener
    public void DebugReturn(DebugLevel level, string message)
    {
        Controller.DebugReturn(level, message);
    }

    public void OnEvent(EventData eventData)
    {
        Controller.OnEvent(eventData);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Controller.OnOpertaionResponse(operationResponse);
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        print(statusCode.ToString());
        switch (statusCode)
        {
            case StatusCode.Connect:
                Peer.EstablishEncryption();
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.ExceptionOnConnect:
            case StatusCode.TimeoutDisconnect:
                Controller.OnDisconnect("" + statusCode);
                State = new Disconnected(_instance);
                break;
            case StatusCode.EncryptionEstablished:
                State = new Connected(_instance);
                break;
            default:
                Controller.OnUnexpectedStatusCode(statusCode);
                State = new Disconnected(_instance);
                break;
        }
    }
    #endregion
}

