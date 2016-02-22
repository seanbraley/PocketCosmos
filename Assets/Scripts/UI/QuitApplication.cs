using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour {

	public void Quit()
	{
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        // Save player data
        PlayerData.playdata.Save(); // TO DO: Push changes to server

		//Quit the application
		Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        // Save player data
        PlayerData.playdata.Save(); // TO DO: Push changes to server

        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
	#endif
	}
}
