using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class Main : MonoBehaviour
{
	static string PATH_DATA = "Assets\\Data\\";
	
	public
		UIOrganizer myUI_menu, myUI_game, myUI_gameFinished,myUI_editor;
	public TransitionEffect transition;

	KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_RESTART = delegate { },
		EVENT_GAME_NEXT = delegate { },
		EVENT_GAME_WIN = delegate { };

	int levelSelected = 10;
	GameObject loopEditor = null;
	Board myBoard;
	GameSetting setting = new GameSetting();

	enum STATE { Init, GameMode, EditorMode };
	STATE stateMe = STATE.Init;
	public void Awake()
	{
		//EVENT_GAME_OVER += delegate { transition.initTransition(); stateMe = STATE.GameMode; };
		//EVENT_GAME_WIN +=  delegate { transition.initTransition(); stateMe = STATE.EditorMode; };

		UI_Menu.EVENT_REQUEST_GAME_START += delegate { transition.initTransition(); stateMe = STATE.GameMode; };
		UI_Menu.EVENT_REQUEST_MAP_EDITOR += delegate { transition.initTransition(); stateMe = STATE.EditorMode; };
		//UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		//UI_Menu.EVENT_REQUEST_MAP_EDITOR += EVENTHDR_INIT_EDITOR;

		//UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENTHDR_INIT_GAME;
		//UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENTHDR_NEXT_LEVEL;

		GameLoop.EVENT_GAME_OVER	+= EVENT_GAME_OVER;
		GameLoop.EVENT_GAME_WIN += EVENT_GAME_WIN;

		TransitionEffect.EVENT_FINISHED_TRANSITION += transitionCompleted;
	}

	void transitionCompleted() //after finishing playing logo
	{
		switch (stateMe)
		{
			case STATE.Init:
				myUI_menu.show();
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
	void helperInitGame(int lv = 0)
	{
		string fileName = (lv < 10) ? ("level0" + lv) : ("level" + lv);
		fileName += ".txt";
		using (var reader = new System.IO.StreamReader(PATH_DATA + fileName))
		{
			var deserialized = JSONArray.Parse(reader.ReadToEnd()).AsArray;
			setting.initGame(deserialized);
			//var level = JsonConvert.DeserializeObject<List<DataUnit>> (reader.ReadToEnd());
			//Debug.Log(level);
			//setting.initGame(level);
		}
	}
	void EVENTHDR_INIT_GAME()
	{
		hideAll();
		myUI_game.show();
		helperInitGame(levelSelected);
	}
	void EVENTHDR_INIT_EDITOR()
	{
		if (loopEditor != null) GameObject.Destroy(loopEditor.gameObject);
		hideAll();
		myUI_editor.show();
		setting.initGame(JSON.Parse("[]").AsArray, false);
		loopEditor = new GameObject("	EditorLoop", new System.Type[] { typeof(EditorLoop) });
		loopEditor.GetComponent<EditorLoop>()
			.init(myUI_editor.gameObject.GetComponent<UI_Editor>());
	}
	void EVENTHDR_NEXT_LEVEL()
	{
		hideAll();
		myUI_game.show();
		helperInitGame(++levelSelected);
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

			using (var t = new System.IO.StreamWriter(PATH_DATA + "level"+((levelSelected<10)?"0":"" ) +levelSelected+".txt"))
			{
				Debug.Log("encoding data : " + jsonEncoded);
				t.Write(jsonEncoded);
			}
		}
	}
}
