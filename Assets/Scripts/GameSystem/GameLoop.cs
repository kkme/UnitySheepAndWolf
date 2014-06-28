﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GameLoop : MonoBehaviour
{
	public static KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_WIN = delegate { };

	Board myBoard;
	bool isPlaying = true;
	public void init(Board b)
	{
		Debug.Log("GAMELOOP : Initiated, Board is " + b);
		myBoard = b;

		EVENT_GAME_WIN += EVENTHDR_GAME_WIN;
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;
		UnitPlayer.EVENT_REACHEED_GOAL += EVENT_GAME_WIN;
		UnitEnemy.EVENT_HIT_PLAYER += EVENT_GAME_OVER;

		foreach (var u in WorldInfo.units)
		{
			u.IsUpdated = false;
			myBoard.positionUnit(u);
			u.registerOnGrid();
		}	
	}

	//player has initiated a turn with new input
	public void turn(Vector2 dir)
	{
		if (!isPlaying) return;
		if (turn_player(dir)) turn_others();
		
		turn_record();
	}
	void EVENTHDR_GAME_WIN()
	{
		isPlaying = false;
	}
	void EVENTHDR_GAME_OVER()
	{
		Debug.Log("GameLoop : gameOver");
		isPlaying = false;
	}

	bool turn_player(Vector2 dir)
	{
		return WorldInfo.unitPlayer.move(dir);
	}

	void turn_others()
	{
		Debug.Log("turn others " + WorldInfo.units.Count);
		
		foreach (var unit in WorldInfo.units)
		{
			Debug.Log(unit);
			if(!unit.IsUpdated) unit.KUpdate();
		}
	
		
	}
	void turn_record()
	{
		foreach (var u in WorldInfo.units)
		{
			u.IsUpdated = false;
			myBoard.positionUnit(u);
		}
	}
}
