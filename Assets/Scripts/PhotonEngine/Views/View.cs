using System;

using UnityEngine;

//The class that should be extended instead of MonoBehaviour for anything that wants to communicate
public abstract class View : MonoBehaviour, IView
{
    public virtual void Awake()
    {
        Controller = new ViewController(this);

    }
    #region Implementation of IView
    public abstract IViewController Controller { get; protected set; }

    public virtual void OnApplicationQuit()
    {
        Controller.ApplicationQuit();
    }
    public void Disconnected(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Debug.Log(message);
        }
        /* Go back to login?
        if (Application.loadedLevel != 0)
        {
            Application.loadedLevel(0);
        }
        */
    }

    public void LogDebug(string message)
    {
        Debug.Log(message);
    }

    public void LogError(Exception e)
    {
        Debug.LogError(e.ToString());
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
    }

    public void LogInfo(string message)
    {
        Debug.Log(message);
    }
    #endregion
}

