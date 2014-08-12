using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class Main : MonoBehaviour
{
	static string PATH_DATA = "Assets\\Data\\";
	public
		UIOrganizer myUI_menu, myUI_game, myUI_gameFinished,myUI_editor;
	KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_RESTART = delegate { },
		EVENT_GAME_NEXT = delegate { },
		EVENT_GAME_WIN = delegate { };

	int levelSelected = 0;
	GameObject loopEditor = null;
	Board myBoard;
	GameSetting setting = new GameSetting();

	public void Awake()
	{
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;
		EVENT_GAME_WIN += EVENTHDR_GAME_WIN;

		UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		UI_Menu.EVENT_REQUEST_MAP_EDITOR += EVENTHDR_INIT_EDITOR;
		UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENTHDR_INIT_GAME;
		UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENTHDR_NEXT_LEVEL;

		GameLoop.EVENT_GAME_OVER	+= EVENT_GAME_OVER;
		GameLoop.EVENT_GAME_WIN += EVENT_GAME_WIN;
	}
	void Start()
	{
		hideAll();
		myUI_menu.show();
	}
	void helperInitGame(int lv = 0)
	{
		string fileName = (lv < 10) ? ("level0" + lv) : ("level" + lv);
		fileName += ".txt";
		using (var reader = new System.IO.StreamReader(PATH_DATA + fileName))
		{
			var level = JsonConvert.DeserializeObject<List<DataUnit>> (reader.ReadToEnd());
			Debug.Log(level);
			setting.initGame(level);
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
		setting.initGame(new List<DataUnit>(), false);
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
			var data = WorldInfo.toData();
			JsonConvert.SerializeObject(data);
			using (var t = new System.IO.StreamWriter(PATH_DATA + "level00.txt"))
			{
				t.Write(JsonConvert.SerializeObject(data));
			}
		}
	}
}
