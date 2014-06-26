using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Main : MonoBehaviour
{
	public Board PREFAB_BOARD;
	public
		UIOrganizer myUI_menu, myUI_game, myUI_gameFinished;

	Board myBoard;
	KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_RESTART = delegate { },
		EVENT_GAME_NEXT = delegate { }; 

	public void Awake()	
	{
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;

		UI_Menu.EVENT_REQUEST_GAME_START += EVENTHDR_INIT_GAME;
		UI_GameFinished.EVENT_REQUEST_RESTART_LEVEL += EVENT_GAME_RESTART;
		UI_GameFinished.EVENT_REQUEST_NEXT_LEVEL += EVENT_GAME_NEXT;
		
		GameLoop.EVENT_GAME_OVER += EVENT_GAME_OVER;
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
		GameSetting.initGame(new Level00());
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
