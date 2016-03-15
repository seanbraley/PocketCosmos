using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceBar : MonoBehaviour {

	private Text _text;
	public int Value {
		get {
			return int.Parse(_text.text);
		}
		set {
			_text.text = value.ToString();
		}
	}

	private bool _isShowing;
	public bool IsShowing {
		get {
			return _isShowing;
		}
		set {
			_isShowing = value;
			if (_isShowing) {
				Show(0.25f);
			}
			else {
				Hide(0.25f);
			}
		}
	}

	private IEnumerator _currentCoroutine;
	public IEnumerator CurrentCoroutine {
		get {
			return _currentCoroutine;
		}
		set {
			if (_currentCoroutine != null) {
				StopCoroutine(_currentCoroutine);
			}
			_currentCoroutine = value;
			if (_currentCoroutine != null) {
				StartCoroutine(_currentCoroutine);
			}
		}
	}

	private Vector3 _showingPosition;
	private Vector3 _hidingPosition;

	public RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		//DISPLAY MANAGER HANDLES INITIALIZATION
	}

	public void Initialize() {
		rectTransform = GetComponent<RectTransform>();
		_text = transform.Find("Text").GetComponent<Text>();
		_showingPosition = rectTransform.anchoredPosition;
		_hidingPosition = _showingPosition + Vector3.up * 100;
	}
	
	// Update is called once per frame
	void Update () {
		/* DEBUG / Fun
		if (Input.GetKeyDown("k")) {

			IsShowing = !IsShowing;
		}
		*/
	}

	public void Show(float time) {
		CurrentCoroutine = ShowCoroutine(true,time);
		_isShowing = true;
	}

	IEnumerator ShowCoroutine(bool show, float time) {
		// pick goal position based on direction of movement
		Vector2 goalPosition;
		if (show) {
			goalPosition = _showingPosition;
		}
		else {
			goalPosition = _hidingPosition;
		}

		// determine speed
		float deltaY = Mathf.Abs(_showingPosition.y - _hidingPosition.y);
		float speed;
		if (time > 0) {
			speed = 1 / time; // reciprocal (shoutout to grade 9 math)
		}
		else {
			speed = Mathf.Infinity;
			rectTransform.anchoredPosition = goalPosition; // instantaneous
		}

		// Slide to goal position
		while (rectTransform.anchoredPosition != goalPosition) {
			Debug.Log(rectTransform.anchoredPosition);
			rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition,goalPosition,deltaY*speed*Time.deltaTime);
			yield return null;
		}

		//Tidy up
		_currentCoroutine = null;
		yield return true;
	}

	public void Hide(float time) {
		CurrentCoroutine = ShowCoroutine(false,time);
		_isShowing = false;
	}
}
