using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GameLoop : MonoBehaviour
{
	enum State { READY, PROCESSING_PLAYER_INPUT,CALCULATING_REACTION,PROCESSING_REACTION};
	State myState = State.READY;
	int id;
	static int idCount = 0;
	public static KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_GAME_OVER = delegate { },
		EVENT_GAME_WIN = delegate { };

	bool isPlaying = true;
	List<UnitBase> units_animation = new List<UnitBase>();


	public void EVENTHDR_INPUT_PLAYER(int n)
	{
		input_player(dirInputInt[n]);
	}
	void Awake()
	{
		enabled = false;
		Main.EVENT_INTPUT_PLAYER += EVENTHDR_INPUT_PLAYER;
		UnitObjTemp_goal.EVENT_DESTROYED += EVENTHDR_GAME_WIN;
		UnitPlayer.EVENT_KILLED += EVENTHDR_GAME_OVER;
	}

	public void init()
	{
		id = idCount++;
		Debug.Log("GAMELOOP : initiated" + id);
			

		foreach (var u in WorldInfo.unitsUpdate01)
		{
			u.isUpdated = false;
			u.registerOnGrid();
		}	
	}
	void OnDestroy()
	{
		UnitObjTemp_goal.EVENT_DESTROYED -= EVENTHDR_GAME_WIN;
		UnitPlayer.EVENT_KILLED -= EVENTHDR_GAME_OVER;
		Main.EVENT_INTPUT_PLAYER -= EVENTHDR_INPUT_PLAYER;
	}
	Dictionary<int, Vector2> dirInputInt = new Dictionary<int, Vector2>()
	{
		{0,new Vector2(0,1)},
		{1,new Vector2(1,0)},
		{2,new Vector2(0,-1)},
		{3,new Vector2(-1,0)},

	};
	//player has initiated a turn with new input
	public void input_player(Vector2 dir)
	{
		if (!isPlaying || myState != State.READY|| WorldInfo.unitPlayer_real == null|| !WorldInfo.unitPlayer_real.isAlive) return;
		WorldInfo.PLAYER_INPUT = dir;

		myState = State.PROCESSING_PLAYER_INPUT;

		if (!WorldInfo.unitPlayer_real.turn(dir) ||
			!WorldInfo.unitPlayer_real.isAlive ||
			!isPlaying
			) {myState = State.READY; return;}
		if (WorldInfo.unitPlayer_real.isMoved) WorldInfo.unitPlayer_real.UpdateAnimation();
		turn_others(WorldInfo.unitsUpdate00);
		foreach (var u in WorldInfo.unitsUpdate00)
		{
			//if (u.isPushed) { units_animation.Add(u); helperInitAnimation(u); continue; }
			u.UpdateAnimation();
		}
		//turn_record(WorldInfo.unitsUpdate00);

		enabled = true;
	}
	void EVENTHDR_GAME_WIN()
	{
		if (WorldInfo.unitPlayer_real != null)
		{
			WorldInfo.unitPlayer_real.UpdateAnimation();
		}
		isPlaying = false;
		EVENT_GAME_WIN();
	}
	void EVENTHDR_GAME_OVER()
	{
		isPlaying = false;
		EVENT_GAME_OVER();
	}

	bool isCloseEnoughToPlayer(UnitBase u, int disMinX = 5, int disMinY = 5)
	{
		return Mathf.Abs((int)(u.pos.x - WorldInfo.unitPlayer_real.pos.x)) <= disMinX && Mathf.Abs((int)(u.pos.y - WorldInfo.unitPlayer_real.pos.y)) <= disMinY;

	}
	void turn_others(List<UnitBase> l)
	{

		for (int i = 0; i < l.Count; i++)
		{
			if (isCloseEnoughToPlayer(l[i])) l[i].turn();
		}
		for (int i = l.Count - 1; i >= 0; i--)
			if (!l[i].isAlive)
			{
				l[i].Destroy();
			}
	}
	void turn_playAni_reset(List<UnitBase> l)
	{
		foreach (var u in l)
		{
			if (u.isMoved) u.UpdateAnimation();
			u.reset();
		}
	}
	bool isPlayerFinishedProcessingInput()
	{
		if (WorldInfo.unitPlayer_real.GetComponent<RendererSprite>().isAnimating()) return false;
		foreach (var u in WorldInfo.unitsUpdate00) 
			if(u.GetComponent<RendererSprite>().isAnimating()) return false;
		return true;
	}
	bool isPlayerFinishedCalculatingInput()
	{
		foreach (var u in WorldInfo.unitsUpdate01)
			if (u.GetComponent<RendererSprite>().isAnimating()) return false;
		return true;
	}
	void Update()
	{
		switch (myState)
		{
			case State.PROCESSING_PLAYER_INPUT:
				if (!isPlayerFinishedProcessingInput()) return;
				//group 00 contains units related to player's input
				myState = State.CALCULATING_REACTION;
				break;
			case State.CALCULATING_REACTION:
				WorldInfo.initEveryTurn();
				turn_others(WorldInfo.unitsUpdate01);

				if (WorldInfo.unitPlayer_real.isMoved) WorldInfo.unitPlayer_real.UpdateAnimation();
				turn_playAni_reset(units_animation);
				turn_playAni_reset(WorldInfo.unitsUpdate00);
				turn_playAni_reset(WorldInfo.unitsUpdate01);
				for (int i = WorldInfo.unitsUpdate00.Count - 1; i >= 0; i--)
					if (!WorldInfo.unitsUpdate00[i].isAlive)
						WorldInfo.unitsUpdate00[i].Destroy();

				myState = State.PROCESSING_REACTION;
				break;
			case State.PROCESSING_REACTION:
				for (int i = WorldInfo.unitsUpdate01.Count - 1; i >= 0; i--)
					if (WorldInfo.unitsUpdate01[i].GetComponent<RendererSprite>().isAnimating()) break;
				
				if (WorldInfo.unitPlayer_real.amIStuck())
				{
					EVENT_GAME_OVER();
				}
				else
				{
					myState = State.READY;
					units_animation = new List<UnitBase>();
				}
				enabled = false;
				break;

		}
	}
}
