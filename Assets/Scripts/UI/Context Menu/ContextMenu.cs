using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ContextMenu : MonoBehaviour {

	protected Text _titleText;
	protected Text _descriptionText;

	// Use this for initialization
	public virtual void Awake () {
		GameObject title = transform.Find("Title").gameObject;
		GameObject description = transform.Find("Description").gameObject;
		_titleText = title.GetComponent<Text>();
		_descriptionText = description.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTitle(string title) {
		_titleText.text = title;
	}

	public void SetDescription(string description) {
		_descriptionText.text = description;
	}
}
