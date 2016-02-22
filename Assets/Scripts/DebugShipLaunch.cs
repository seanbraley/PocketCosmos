using UnityEngine;
using System.Collections;

public class DebugShipLaunch : MonoBehaviour {

	public GameObject Ship_Prefab;

	private IEnumerator _currentCoroutine;
    private object yield;

    public void BeginLaunchSetup() {
		if (_currentCoroutine != null) {
			CancelLaunch();
		}
		_currentCoroutine = LaunchSetupCoroutine();
		StartCoroutine(_currentCoroutine);
	}

	public void CancelLaunch() {
		StopCoroutine(_currentCoroutine);
	}

	IEnumerator LaunchSetupCoroutine() {
        //yield return null;
        GameObject origin = Player.plyr.selected;
		if (origin == null || !origin.GetComponent<Star>().Discovered) {
            _currentCoroutine = null;
            Debug.Log("CANT SEND SHIP FROM HERE");
            yield break;
		}
        if (PlayerData.playdata.spacebux < 5)
        {
            _currentCoroutine = null;
            Debug.Log("NOT ENOUGH SPACEBUX TO LAUNCH SHIP.");
            yield break;
        }

        /*
		Debug.Log("Select an origin!");
		while(origin == null) {
			if (Player.plyr.selected != null) {
				origin = Player.plyr.selected;
			}
			yield return null;
		}
		*/

        GameObject destination = null;
		Player.plyr.selected = null;
		Debug.Log("Select a destination!");
		while(destination == null) {
			if (Player.plyr.selected != null && Player.plyr.selected != origin) {
				destination = Player.plyr.selected;
			}
			yield return null;
		}
		LaunchShip(origin,destination);
		yield return true;
	}

	public void LaunchShip(GameObject origin, GameObject destination) {
		Debug.Log("Launching Ship!");
        PlayerData.playdata.spacebux -= 5;
        GameObject ship = Instantiate(Ship_Prefab,Vector3.zero,Quaternion.identity) as GameObject;
		ship.GetComponent<Ship>().origin = origin;
		ship.GetComponent<Ship>().destination = destination;
	}
}
