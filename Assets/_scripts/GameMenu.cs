﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMenu : MonoBehaviour {
	private ConnectionHandler _connectionHandler;
	private UserInfo _myUserInfo;
	private bool _pickedUserName = false;
	//private GameObject[] allRooms = new GameObject[10];
	
	public bool inGameRoom;
	public List<string> allUsernames = new List<string>();
	public GUIStyle buttonStyle;
	public GUIStyle textStyle;

	void Awake()
	{
		_myUserInfo = GetComponent<UserInfo>();
		_connectionHandler = GetComponent<ConnectionHandler>();
	}
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if(!_pickedUserName)
			{
				_myUserInfo.username = GUI.TextField(new Rect(Screen.width/2-75,Screen.height/2,150,25), _myUserInfo.username, textStyle);
				if (GUI.Button(new Rect(Screen.width/2-75, Screen.height/2-110, 150, 50), "Pick Username", buttonStyle))
					_pickedUserName = true;
			} 
			else
			{
				_connectionHandler.gameName = GUI.TextField(new Rect(Screen.width/2-75,Screen.height/2,150,25), _connectionHandler.gameName, textStyle);
				if (GUI.Button(new Rect(Screen.width/2-125, Screen.height/2-150, 250, 100), "Start New Server", buttonStyle))
					_connectionHandler.StartServer();
				if (GUI.Button(new Rect(Screen.width/2-125, Screen.height/2+100, 250, 100), "Refresh Servers", buttonStyle))
					_connectionHandler.RefreshHostList();
				
				if (_connectionHandler.hostList != null)
				{
					for (int i = 0; i < _connectionHandler.hostList.Length; i++)
					{
						if (GUI.Button(new Rect(Screen.width/2 - 100,150 + (75 * i), 200, 50), _connectionHandler.hostList[i].gameName + " : " + _connectionHandler.hostList[i].connectedPlayers.ToString() + "/4", buttonStyle))
							_connectionHandler.JoinServer(_connectionHandler.hostList[i]);
					}
				} 
			}
		}
		if(inGameRoom)
		{
			for (int i = 0; i < allUsernames.Count; i++) 
			{
				GUI.TextArea(new Rect(Screen.width/2-50, Screen.height/2+50+50*i, 100, 50), allUsernames[i], textStyle);
			}
			if(Network.isServer)
			{
				if(Network.connections.Length > 0)
				{
					if (GUI.Button(new Rect(Screen.width/2-125, Screen.height/2-100, 250, 100), "Start Game", buttonStyle))
					{
						_connectionHandler.StartGameClicked();
					}
				}
			}
		}
	}
}
