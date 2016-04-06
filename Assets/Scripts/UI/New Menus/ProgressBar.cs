using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	private Image _background;
	private Image _foreground;

	private DateTime _startTime;
	private DateTime _currentTime;
	private DateTime _endTime;

	private Text _timeRemainingText;
	private int _secondsRemaining;
	public int SecondsRemaining {
		get {
			return _secondsRemaining;
		}
		set {
			if (value > 0) {
				_secondsRemaining = value;
			}
			else {
				_secondsRemaining = 0;
			}
			_timeRemainingText.text = "" + _secondsRemaining + " SEC";
		}
	}

	// Use this for initialization
	void Awake () {
		_background	= transform.Find("BG").GetComponent<Image>();
		_foreground	= transform.Find("FG").GetComponent<Image>();
		_timeRemainingText = transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_currentTime = DateTime.Now;
		double totalTravelTime = _endTime.Subtract(_startTime).TotalSeconds;
		double timeTravelled = _currentTime.Subtract(_startTime).TotalSeconds;
		SecondsRemaining = (int) Math.Floor(totalTravelTime - timeTravelled);

		double ratio = timeTravelled / totalTravelTime;

		_foreground.fillAmount = (float) ratio;
	}

	public void SetInfo(ShipInfo info) {
		_background	= transform.Find("BG").GetComponent<Image>();
		_foreground	= transform.Find("FG").GetComponent<Image>();
		_timeRemainingText = transform.Find("Text").GetComponent<Text>();

		_currentTime = DateTime.Now;
		_startTime = info.departure_time;
		_endTime = info.arrival_time;
	}
}
