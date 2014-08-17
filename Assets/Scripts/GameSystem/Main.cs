using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class Main : MonoBehaviour
{
	static string PATH_DATA = "Assets/Resources/Data/";
	static public KDels.EVENTHDR_REQUEST_SIMPLE_INT EVENT_INTPUT_PLAYER = delegate { };
	public TextMesh debugScreen;
	public
		UIOrganizer 
		myUI_menu, myUI_game,myUI_GameLevelSelector,
		myUI_gameFinished,myUI_editor;
	public TransitionEffect transition;


	GameObject loopEditor = null;
	Board myBoard;
	GameSetting setting = new GameSetting();

	enum STATE { Init, GameMode, EditorMode};
	STATE stateMe = STATE.Init;
	public void Awake()
	{
		//Manage all the player related input here
		OS_DeskTOp.EVENT_INPUT_PLAYER += delegate (int n){ EVENT_INTPUT_PLAYER(n); };
		UI_Game.EVENT_INPUT_PLAYER += delegate(int n){EVENT_INTPUT_PLAYER(n);};

		UI_Menu.EVENT_REQUEST_GAME_START += delegate { stateMe = STATE.GameMode;   transition.initTransition(); };
		UI_Menu.EVENT_REQUEST_MAP_EDITOR += delegate { stateMe = STATE.EditorMode; transition.initTransition(); };
		UI_Game.EVENT_REQUEST_MAP_CHANGE += delegate(int levelIncrement) { WorldInfo.level =Mathf.Max(0, WorldInfo.level+levelIncrement); helperInitLevelSelect(); };
		UI_Game.EVENT_REQUEST_MAP_CHANGE += delegate(int dir) { };
		UI_GameLevelSelector.EVENT_LEVEL_SELECTED += delegate(int level) { WorldInfo.level = level; stateMe = STATE.GameMode; transition.initTransition(); };
		//UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		//UI_Menu.EVENT_REQUEST_MAP_EDITOR += EVENTHDR_INIT_EDITOR;

		//UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENTHDR_INIT_GAME;
		//UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENTHDR_NEXT_LEVEL;

		GameLoop.EVENT_GAME_OVER += EVENTHDR_DEAD_TRYAGAIN;
		GameLoop.EVENT_GAME_WIN += EVENTHDR_NEXT_LEVEL;

		TransitionEffect.EVENT_FINISHED_TRANSITION += transitionCompleted;
	}

	void transitionCompleted() //after finishing playing logo
	{
		switch (stateMe)
		{
			case STATE.Init:
				myUI_menu.show();
				//lets skip editor mode for now
				//stateMe = STATE.GameMode;
				//transitionCompleted();
				break;
			case STATE.GameMode:
				EVENTHDR_INIT_GAME();
				break;
			case STATE.EditorMode:
				EVENTHDR_INIT_EDITOR();
				break;
		}
	}
	void Start()
	{
		hideAll();
		//show the main picture, which is done by default
		//wait for 1.5 seconds
		//fade away main picture in .5 seconds. Turn screen to white
		//myUiMenu show!
		//then uncover white

		//myUI_menu.show();
	}
	void helperInitLevelSelect()
	{
		hideAll();
		setting.destroy();
		myUI_GameLevelSelector.show();

	}
	
	void initGame(int lv = 0)
	{

		WorldInfo.level = lv;
		string fileName = (lv < 10) ? ("level0" + lv) : ("level" + lv);
	

		var content = (Resources.Load("Data/" + fileName) as TextAsset).text;


		var deserialized = JSONArray.Parse(content).AsArray;
		setting.initGame(lv, deserialized);
		//var level = JsonConvert.DeserializeObject<List<DataUnit>> (reader.ReadToEnd());
		//Debug.Log(level);
		//setting.initGame(level);
	}
	void EVENTHDR_INIT_GAME()
	{
		hideAll();
		myUI_game.show();
		initGame(WorldInfo.level);
	}
	void EVENTHDR_INIT_EDITOR()
	{
		if (loopEditor != null) GameObject.Destroy(loopEditor.gameObject);
		hideAll();
		myUI_editor.show();
		string fileName = (WorldInfo.level < 10) ? ("level0" + WorldInfo.level) : ("level" + WorldInfo.level);
		Debug.Log("FILE : " + fileName	);
		var file = Resources.Load("Data/" + fileName);
		string c = "[]";
		if (file != null) c = (file as TextAsset).text;


		setting.initGame(-1,JSON.Parse(c).AsArray, false);
		loopEditor = new GameObject("	EditorLoop", new System.Type[] { typeof(EditorLoop) });
		loopEditor.GetComponent<EditorLoop>()
			.init(myUI_editor.gameObject.GetComponent<UI_Editor>());
	}
	void EVENTHDR_DEAD_TRYAGAIN()
	{
		hideAll();
		//pause for a second
		stateMe = STATE.GameMode;
		transition.initTransition();
	}
	void EVENTHDR_NEXT_LEVEL()
	{
		Debug.Log("NEXT LEVEL");
		hideAll();
		//pause for a second
		WorldInfo.level++; 
		stateMe = STATE.GameMode;
		transition.initTransition();
	}
	void EVENTHDR_GAME_WIN()
	{
		Debug.Log("MAIN : EVENTHDR_GAME_WIN");
		myUI_gameFinished.show();
	}
	void EVENTHDR_GAME_OVER()
	{
		Debug.Log("MAIN : EVENTHDR_GAME_OVER");
		myUI_gameFinished.show();
	}
	void hideAll()
	{
		myUI_menu.hide();
		myUI_game.hide();
		myUI_gameFinished.hide();
		myUI_editor.hide();
		myUI_GameLevelSelector.hide();
	}
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.M))
		{
			string jsonEncoded = "[";
			var data = WorldInfo.toData();
			for (int i = 0; i < data.Count; i++)
			{
				jsonEncoded += data[i].ToString();
				if (i != data.Count - 1) jsonEncoded += ",";
			}
			jsonEncoded += "]"; // close encoding
			string fileName = PATH_DATA + "level" + ((WorldInfo.level < 10) ? "0" : "") + WorldInfo.level + ".txt";
			using (var t = new System.IO.StreamWriter(fileName))
			{
				Debug.Log("FILE : " + fileName + "\nencoding data : \n" + jsonEncoded);
				t.Write(jsonEncoded);
			}
		}
	}
}
