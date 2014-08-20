using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class Main : MonoBehaviour
{
	static int MaxLevel = 25;
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

	enum STATE { Init, GameModeDelay, GameModeNoDelay, EditorMode };
	STATE stateMe = STATE.Init;
	public void Awake()
	{
		//Manage all the player related input here
		OS_DeskTOp.EVENT_INPUT_PLAYER += delegate (int n){ EVENT_INTPUT_PLAYER(n); };
		UI_Game.EVENT_INPUT_PLAYER += delegate(int n){EVENT_INTPUT_PLAYER(n);};

		UI_Menu.EVENT_REQUEST_GAME_START += delegate { stateMe = STATE.GameModeNoDelay;  transition.initTransition(); };
		UI_Menu.EVENT_REQUEST_MAP_EDITOR += delegate { stateMe = STATE.EditorMode; transition.initTransition(); };
		UI_Game.EVENT_REQUEST_MAP_CHANGE += delegate(int levelIncrement) { helperInitLevelSelect(); UI_GameLevelSelector.EVENT_REQUEST_BTTN_CLICK(levelIncrement); };
		UI_GameLevelSelector.EVENT_LEVEL_SELECTED += delegate(int level) { WorldInfo.level = level; stateMe = STATE.GameModeNoDelay; transition.initTransition(); };
		//UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		//UI_Menu.EVENT_REQUEST_MAP_EDITOR += EVENTHDR_INIT_EDITOR;

		//UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENTHDR_INIT_GAME;
		//UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENTHDR_NEXT_LEVEL;

		GameLoop.EVENT_GAME_OVER += EVENTHDR_DEAD_TRYAGAIN;
		GameLoop.EVENT_GAME_WIN += EVENTHDR_NEXT_LEVEL;

		TransitionEffect.EVENT_FINISHED_TRANSITION += transitionCompleted;
	}

	IEnumerator<WaitForSeconds> handleTransition()
	{


		yield return new WaitForSeconds(.001f);
		switch (stateMe)
		{
			case STATE.Init:
				//myUI_menu.show();
				//lets skip editor mode for now
				stateMe = STATE.GameModeNoDelay;
				transitionCompleted();
				break;

			case STATE.GameModeDelay:
				yield return new WaitForSeconds(1.0f);
				EVENTHDR_INIT_GAME();
				break;
			case STATE.GameModeNoDelay:
				EVENTHDR_INIT_GAME();
				break;

			case STATE.EditorMode:
				EVENTHDR_INIT_EDITOR();
				break;
		}
		TransitionEffect.EVENT_RESUME();

	}

	void transitionCompleted() //after finishing playing logo
	{
		StartCoroutine(handleTransition());
		/**
		 * TransitionEffect.EVENT_RESUME();
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
		 * **/
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
		EffectManager.Clear();
		setting.destroy();
		myUI_GameLevelSelector.show();

	}
	
	void initGame(int lv = 0)
	{
		lv = Mathf.Min(lv, MaxLevel);
		EffectManager.Clear();
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
		StartCoroutine(COROUTINE_EVENTHDR_DEAD_TRYAGAIN());
		//hideAll();
		////pause for a second
		//stateMe = STATE.GameModeNoDelay;
		//transition.initTransition();
	}
	IEnumerator<WaitForSeconds> COROUTINE_EVENTHDR_DEAD_TRYAGAIN()
	{

		hideAll();
		//pause for a second
		yield return new WaitForSeconds(.4f);
		stateMe = STATE.GameModeNoDelay;
		transition.initTransition();


		//
		//StartCoroutine(COROUTINE_EVENTHDR_NEXT_LEVEL());

	}

	void EVENTHDR_NEXT_LEVEL()
	{
		Debug.Log("NEXT LEVEL");
		hideAll();
		//pause for a second
		WorldInfo.level = Mathf.Min(MaxLevel, WorldInfo.level + 1);
		stateMe = STATE.GameModeDelay;
		transition.initTransition(""+WorldInfo.level, (WorldInfo.level == 0) ? "" : ("" + (WorldInfo.level-1)));
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
