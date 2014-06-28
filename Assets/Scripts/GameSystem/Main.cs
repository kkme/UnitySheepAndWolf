using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class Main : MonoBehaviour
{
	public
		UIOrganizer myUI_menu, myUI_game, myUI_gameFinished;
	KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_RESTART = delegate { },
		EVENT_GAME_NEXT = delegate { }; 	

	int level = 0;
	Board myBoard;
	GameSetting setting = new GameSetting();

	public void Awake()	
	{
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;

		UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENT_GAME_RESTART;
		UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENT_GAME_NEXT;
		
		GameLoop.EVENT_GAME_OVER += EVENT_GAME_OVER;

		//var level = new KLevel(3, 3);
		//var a = new KLevel_Unit(KEnums.UNIT.PLAYER,0, 0, 0);
		//var b = new KLevel_Unit(KEnums.UNIT.ENEMY, 0, 0, 1);
		//var c = new KLevel_Unit(KEnums.UNIT.ENEMY, 0, 0, 2);
		//level.units.Add(a);
		//level.units.Add(b);
		//level.units.Add(c);
		//var serialized = JsonConvert.SerializeObject(level);
		//Debug.Log(serialized);
	}
	void Start()
	{
		hideAll();
		myUI_menu.show();
	}
	public void EVENTHDR_INIT_GAME()
	{
		hideAll();
		myUI_game.show();
		var level = new KLevel(3, 3);
		var a = new KLevel_Unit(KEnums.UNIT.PLAYER,0, 0, 0);
		var b = new KLevel_Unit(KEnums.UNIT.ENEMY, 0, 0, 1);
		var c = new KLevel_Unit(KEnums.UNIT.ENEMY, 0, 0, 2);
		level.units.Add(a);
		level.units.Add(b);
		level.units.Add(c);
		var serialized = JsonConvert.SerializeObject(level);
		using (System.IO.StreamWriter writer = new System.IO.StreamWriter("TEST_FILE.txt", true))
		{

			writer.Write("HELLOW");
			writer.Write(serialized);
		}
		Debug.Log("WRITING " + serialized);

		//level.units.Add(c);
		setting.initGame(level);
	}
	void EVENTHDR_GAME_OVER()
	{
		myUI_gameFinished.show();
	}
	void hideAll()
	{
		myUI_menu.hide();
		myUI_game.hide();
		myUI_gameFinished.hide();
	}
}
