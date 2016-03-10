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
        GameObject origin = Player.instance.selected;
		if (origin == null || !origin.GetComponent<Star>().Discovered) {
            _currentCoroutine = null;
            Debug.Log("CANT SEND SHIP FROM HERE");
            yield break;
		}
        if (PlayerData.instance.spacebux < 5)
        {
            _currentCoroutine = null;
            Debug.Log("NOT ENOUGH SPACEBUX TO LAUNCH SHIP.");
            yield break;
        }

        /*
		Debug.Log("Select an origin!");
		while(origin == null) {
			if (Player.instance.selected != null) {
				origin = Player.instance.selected;
			}
			yield return null;
		}
		*/

        GameObject destination = null;
		Player.instance.selected = null;
		Debug.Log("Select a destination!");
		while(destination == null) {
			if (Player.instance.selected != null && Player.instance.selected != origin) {
				destination = Player.instance.selected;
			}
			yield return null;
		}
		LaunchShip(origin,destination);
		yield return true;
        NetworkManager.instance._controller.SendDiscoveredStar(destination.gameObject.GetComponent<Star>().myNumber); // TESTING
        NetworkManager.instance._controller.RetrieveKnownStars(); // TESTING
    }

	public void LaunchShip(GameObject origin, GameObject destination) {
		Debug.Log("Launching Ship!");
        PlayerData.instance.spacebux -= 5;
        GameObject ship = Instantiate(Ship_Prefab,Vector3.zero,Quaternion.identity) as GameObject;
		ship.GetComponent<Ship>().origin = origin;
		ship.GetComponent<Ship>().destination = destination;
        origin.GetComponent<Star>().KeepLoaded();
        destination.GetComponent<Star>().KeepLoaded();
    }
}
