using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message : MonoBehaviour {

	private CanvasGroup _canvasGroup;

	public static float fadeInTime = 0.5f;
	public static float fadeOutTime = 2f;
	private bool fadingUp = true;

	// Use this for initialization
	void Start () {
		GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
		GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
		GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingUp) {
			if (_canvasGroup.alpha < 1) {
				_canvasGroup.alpha += (1f/fadeInTime*Time.deltaTime);
			}
			else {
				fadingUp = false;
			}
		}
		else {
			if (_canvasGroup.alpha > 0) {
				_canvasGroup.alpha -= (1f/fadeOutTime*Time.deltaTime);
			}
			else {
				Destroy(this.gameObject);
			}
		}
	}
}
