using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GameLoop : MonoBehaviour
{
	int id;
	static int idCount = 0;
	public static KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_WIN = delegate { };

	Board myBoard;
	bool isPlaying = true;
	public void init(Board b)
	{
		id = idCount++;
		Debug.Log("GAMELOOP : initiated" + id);
		myBoard = b;
			
		EVENT_GAME_WIN += EVENTHDR_GAME_WIN;
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;
		UnitPlayer.EVENT_REACHEED_GOAL += EVENT_GAME_WIN;
		UnitPlayer.EVENT_ATTACKED += EVENT_GAME_OVER;

		foreach (var u in WorldInfo.units)
		{
			u.isUpdated = false;
			myBoard.positionUnit(u);
			u.registerOnGrid();
		}	
	}
	void OnDestroy()
	{
		Debug.Log("GameLoop + Destroyed " + id);
		UnitPlayer.EVENT_REACHEED_GOAL -= EVENT_GAME_WIN;
		UnitPlayer.EVENT_ATTACKED -= EVENT_GAME_OVER;
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
		Debug.Log("GameLoop : gameOver" + id);
		isPlaying = false;
	}

	bool turn_player(Vector2 dir)
	{
		return WorldInfo.unitPlayer.turn(dir);
	}

	void turn_others()
	{

		foreach (var u in WorldInfo.units) u.KUpdate();
		for (int i = WorldInfo.units.Count - 1; i >= 0; i--) 
			if (!WorldInfo.units[i].isAlive) WorldInfo.units.RemoveAt(i);
		
	}
	void turn_record()
	{
		foreach (var u in WorldInfo.units)
		{
			//if (u.isMoved) myBoard.positionUnit(u);
			//Debug.Log(u + " " + u.isMoved);
			if (u.isMoved)
			{
				var ani = u.GetComponent<KSpriteRenderer>();
				ani.move(u.pos.x + .5f, (int)u.pos.y + .5f);
				ani.rotate((++u.dirFacing)%4);
			}
			
			u.isUpdated = false;
			u.isMoved = false;
		}
	}
}
