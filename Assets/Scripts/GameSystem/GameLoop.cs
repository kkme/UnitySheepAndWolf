using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GameLoop : MonoBehaviour
{
	enum State { READY, PROCESSING_PLAYER_INPUT };
	State myState = State.READY;
	int id;
	static int idCount = 0;
	public static KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_WIN = delegate { };

	Board myBoard;
	bool isPlaying = true;
	void Awake()
	{
		enabled = false;
	}
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
	public void player_intput(Vector2 dir)
	{
		if (!isPlaying || myState != State.READY) return;

		WorldInfo.unitPlayer.turn(dir);
		helperInitAnimation(WorldInfo.unitPlayer);
		myState = State.PROCESSING_PLAYER_INPUT;
		enabled = true;
	}
	void EVENTHDR_GAME_WIN()
	{
		isPlaying = false;
	}
	void EVENTHDR_GAME_OVER()
	{
		isPlaying = false;
	}


	void turn_others()
	{

		foreach (var u in WorldInfo.units) u.turn();
		for (int i = WorldInfo.units.Count - 1; i >= 0; i--) 
			if (!WorldInfo.units[i].isAlive) WorldInfo.units.RemoveAt(i);
	}
	void helperInitAnimation(UnitBase u)
	{
		var ani = u.GetComponent<KSpriteRenderer>();
		ani.initAnimation(u.pos.x,u.pos.y,u.dirFacing);

	}
	void turn_record()
	{
		foreach (var u in WorldInfo.units)
		{
			//if (u.isMoved) myBoard.positionUnit(u);
			//Debug.Log(u + " " + u.isMoved);
			if (u.isMoved) helperInitAnimation(u);
			
			u.isUpdated = false;
			u.isMoved = false;
		}
	}
	void Update()
	{
		if (!WorldInfo.unitPlayer.GetComponent<KSpriteRenderer>().isAnimating())
			myState = State.READY;
		if (myState == State.READY)
		{
			turn_others();
			turn_record();
			enabled = false;
		}
	}
}
