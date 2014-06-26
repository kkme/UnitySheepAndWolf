using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GameLoop : MonoBehaviour
{
	public static KDels.EVENTHDR_REQUEST_SIMPLE EVENT_GAME_OVER = delegate { };

	Board myBoard;
	bool isPlaying = true;
	public void init(Board b)
	{
		Debug.Log("GAME LOOP BOARD SET " + b);
		myBoard = b;
		UnitEnemy.EVENT_HIT_PLAYER += EVENTHDR_PlayerGotHit;
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

	void EVENTHDR_PlayerGotHit()
	{
		Debug.Log("Player got hit ");
		gameOver();
	}
	void gameOver()
	{
		Debug.Log("GameLoop : gameOver");
		isPlaying = false;
		EVENT_GAME_OVER();
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
