using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsernameBox : MonoBehaviour {
	private RectTransform _myRectTransform;
	void Start()
	{
		_myRectTransform = this.GetComponent<RectTransform>();
	}
	public void SetPosition(Vector3 position)
	{
		if(_myRectTransform != null)
		{
			_myRectTransform.position = position;
		} else {
			_myRectTransform = this.GetComponent<RectTransform>();
			_myRectTransform.anchoredPosition = position;
		}
	}
	public void SetText(string txt)
	{
		Text textToChange = GetComponentInChildren<Text>();
		if(textToChange != null)
			textToChange.text = txt;
	}
}
