using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerButton : MonoBehaviour {
	public bool toggled;
	public Text players;
	public Text serverName;
	public Text gameType;
	private GameMenu _gameMenu;
	private HostData _myHostData;
	private RectTransform _myRectTransform;
	void Start()
	{
		_myRectTransform = this.GetComponent<RectTransform>();
		_gameMenu = GetComponentInParent<GameMenu>();
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
	public void SetServer(HostData data)
	{
		_myHostData = data;
		players.text = "|  " + data.connectedPlayers + "/" + data.playerLimit + "  |";
		serverName.text = "|  " + data.gameName + "  |";
		gameType.text = "|  " + data.gameType + "  |";
	}
	public void ToggleMe()
	{
		foreach(GameObject serverBut in _gameMenu.allServers)
		{
			if(serverBut != this.gameObject)
			{
				serverBut.GetComponent<Toggle>().isOn = false;
			}
		}
		toggled = this.GetComponent<Toggle>().isOn;
	}
	public HostData GetServerData()
	{
		return _myHostData;
	}
}
