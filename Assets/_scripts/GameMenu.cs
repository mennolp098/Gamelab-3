using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	private ConnectionHandler _connectionHandler;
	private UserInfo _myUserInfo;
	private bool _pickedUserName = false;
	//private GameObject[] allRooms = new GameObject[10];

	public GameObject usernamePanel;
	public GameObject serverlistPanel;
	public GameObject mainmenuPanel;
	public GameObject newServerPanel;

	public bool inGameRoom;
	public List<string> allUsernames = new List<string>();
	public List<GameObject> allServers = new List<GameObject>();
	public GameObject serverButton;
	public Text username;
	public Text servername;
	//public GUIStyle buttonStyle;
	//public GUIStyle textStyle;

	void Awake()
	{
		GameObject connector = GameObject.FindGameObjectWithTag(Tags.Connector);
		_myUserInfo = connector.GetComponent<UserInfo>();
		_connectionHandler = connector.GetComponent<ConnectionHandler>();
	}
	/*
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
	*/
	public void Update()
	{
		if(inGameRoom)
		{
			for (int i = 0; i < allUsernames.Count; i++) 
			{
				//TODO: add usernames once
			}
			if(Network.isServer)
			{
				if(Network.connections.Length > 0)
				{
					//TODO: add startgame button once
				}
			}
		}
	}
	public void RefreshServerList()
	{
		_connectionHandler.RefreshHostList();
		foreach(GameObject server in allServers)
		{
			Destroy(server);
		}
		allServers.Clear();
		if(_connectionHandler.hostList != null)
		{
			for(int i = 0; i < _connectionHandler.hostList.Length; i++)
			{
				GameObject newServerBut = Instantiate(serverButton, new Vector3(0,0,0),Quaternion.identity) as GameObject;
				newServerBut.transform.SetParent(serverlistPanel.transform);
				newServerBut.GetComponent<ServerButton>().SetServer(_connectionHandler.hostList[i]);
				newServerBut.GetComponent<ServerButton>().SetPosition(new Vector3(140,-100 + i * -40,0));
				allServers.Add(newServerBut);
			}
		}
	}
	public void JoinGame()
	{

		HostData game = null;
		foreach(GameObject serverButton in allServers)
		{
			ServerButton serverScript = serverButton.GetComponent<ServerButton>();
			if(serverScript.toggled)
			{
				game = serverScript.GetServerData();
			}
		}
		if(game != null)
		{
			_connectionHandler.JoinServer(game);
			serverlistPanel.SetActive(false);
		}
	}
	public void StartGame()
	{
		_connectionHandler.StartGameClicked();
	}
	public void StartNewServer()
	{
		_connectionHandler.gameName = servername.text;
		_connectionHandler.StartServer();
		newServerPanel.SetActive(false);
	}
	public void UsernameButtonClicked()
	{
		_myUserInfo.username = username.text;
		usernamePanel.SetActive(false);
		mainmenuPanel.SetActive(true);
	}
	public void ServerButtonClicked()
	{
		mainmenuPanel.SetActive(false);
		newServerPanel.SetActive(true);
	}
	public void ServerlistButtonClicked()
	{
		mainmenuPanel.SetActive(false);
		serverlistPanel.SetActive(true);
	}
	public void BackButtonClicked()
	{
		serverlistPanel.SetActive(false);
		newServerPanel.SetActive(false);
		mainmenuPanel.SetActive(true);
	}
	public void CreditsButtonClicked()
	{
		//TODO: credits panel
	}
}
