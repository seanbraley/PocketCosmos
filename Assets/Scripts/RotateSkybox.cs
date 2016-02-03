using UnityEngine;
using System.Collections;

// Slowly and continually rotates the skybox.
// Put this on a main camera in the scene.
public class RotateSkybox : MonoBehaviour {

    float _currentRot = 0;
    public float _degree = 1;   // degree of rotation
    	
	// Update is called once per frame
	void Update () {

        _currentRot += _degree * Time.deltaTime;
        _currentRot %= 360;
        RenderSettings.skybox.SetFloat("_Rotation", _currentRot);
    }
            
}
