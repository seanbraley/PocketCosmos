using UnityEngine;
using System.Collections;       //Allows us to use Lists. 
using System.Collections.Generic;
using UnityEngine.SceneManagement;  // scene management at run-time.
using UnityEngine.EventSystems;     // handles input, raycasting, and sending events.
using UnityEngine.UI;

// TO-DO : fill this in later

public class NetworkManager : View {

    public bool LoginSuccess = false;

    public Text messageBox;                                              // Set in inspector - message to user text display

    public static NetworkManager instance = null;              //Static instance of NetworkManager which allows it to be accessed by any other script.
    public string ServerAddress;                                        //The address of the photon server 
    public string ApplicationName;                                      //The photon application you are connecting to

    public NetworkController _controller;

    public override IViewController Controller
    {
        get
        {
            return (IViewController)_controller;
        }

        protected set { _controller = value as NetworkController; }
    }

    void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Use the login controller to handle message passing to the server
        Controller = new NetworkController(this);
    }

    // Use this for initialization
    void Start () {
        //Grab the Photon Egnine and have it connect to the server
        PhotonEngine.UseExistingOrCreateNewPhotonEngine(ServerAddress, ApplicationName);
        PhotonEngine.Instance.Controller = Controller as ViewController;
        Debug.Log(PhotonEngine.Instance.Controller);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoginSuccessfull()
    {
        //We got a successfull login response from the server
        Debug.Log("Logging in...");
        messageBox.color = Color.green;
        messageBox.text = "Login Successful";

        LoginSuccess = true;

        this._controller.RetrieveKnownStars(); // TESTING
        //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
        //Invoke("LoadDelayed", fadeColorAnimationClip.length * .5f);

        //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
        //animColorFade.SetTrigger("fade");
    }
    public void LoginFailure(short returnCode)
    {
        //We failed to login
        Debug.Log(string.Format("Login failed. Got return code {0}", returnCode));

        messageBox.color = Color.red;
        messageBox.text = "Login Failure";
        switch (returnCode)
        {
            case 1:
                messageBox.text += "\nName in use";
                break;
            case 2:
                messageBox.text += "\nIncorrect User/Pass";
                break;
            case 3:
                messageBox.text += "\nUser already logged in";
                break;
            default:
                messageBox.text += "\nFailed";
                break;
        }

    }

}
