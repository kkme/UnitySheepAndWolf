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
	void Awake()
	{
		enabled = false;
	}
	public void init()
	{
		id = idCount++;
		Debug.Log("GAMELOOP : initiated" + id);
			
		EVENT_GAME_WIN += EVENTHDR_GAME_WIN;
		EVENT_GAME_OVER += EVENTHDR_GAME_OVER;
		UnitObjTemp_goal.EVENT_DESTROYED += EVENT_GAME_WIN;
		UnitPlayer.EVENT_KILLED += EVENT_GAME_OVER;

		foreach (var u in WorldInfo.unitsUpdate01)
		{
			u.isUpdated = false;
			u.registerOnGrid();
		}	
	}
	void OnDestroy()
	{
		Debug.Log("GameLoop + Destroyed " + id);
		UnitPlayer.EVENT_KILLED -= EVENT_GAME_OVER;
	}

	//player has initiated a turn with new input
	public void player_intput(Vector2 dir)
	{
		if (!isPlaying || myState != State.READY|| WorldInfo.unitPlayer_real == null|| !WorldInfo.unitPlayer_real.isAlive) return;
		WorldInfo.PLAYER_INPUT = dir;
		if (!WorldInfo.unitPlayer_real.turn(dir)	||
			!WorldInfo.unitPlayer_real.isAlive		||
			!isPlaying
			) return;
		helperInitAnimation(WorldInfo.unitPlayer_real);
		turn_others(WorldInfo.unitsUpdate00);
		foreach (var u in WorldInfo.unitsUpdate00)
		{
			//if (u.isPushed) { units_animation.Add(u); helperInitAnimation(u); continue; }
			helperInitAnimation(u);
		}
		//turn_record(WorldInfo.unitsUpdate00);

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


	void turn_others(List<UnitBase> l)
	{

		for (int i = 0; i < l.Count; i++) l[i].turn();
		for (int i = l.Count - 1; i >= 0; i--)
			if (!l[i].isAlive)
			{
				l[i].Destroy();
			}
	}
	void helperInitAnimation(UnitBase u)
	{
		var ani = u.GetComponent<RendererSprite>();
		ani.initAnimation(u.pos.x,u.pos.y,u.dirFacing);
	}
	void turn_playAni_reset(List<UnitBase> l)
	{
		foreach (var u in l)
		{
			if (u.isMoved) helperInitAnimation(u);
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
				turn_playAni_reset(units_animation);
				turn_playAni_reset(WorldInfo.unitsUpdate00);
				turn_playAni_reset(WorldInfo.unitsUpdate01);
				for (int i = WorldInfo.unitsUpdate00.Count - 1; i >= 0; i--)
					if (!WorldInfo.unitsUpdate00[i].isAlive)
					{
						WorldInfo.unitsUpdate00[i].Destroy();
					}

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
