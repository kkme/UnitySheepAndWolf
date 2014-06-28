using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitBase
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE EVENT_MOVED = delegate { };
	static public event KDels.EVENTHDR_REQUEST_SIMPLE EVENT_REACHEED_GOAL = delegate { };
	void Awake()
	{
		WorldInfo.units.Add(this);
		WorldInfo.unitPlayer = this;
		myType = KEnums.UNIT.PLAYER;
		//UnitEnemy.EVENT_HIT_PLAYER += EVENT_GOT_HIT;
	}
	void EVENT_GOT_HIT()
	{
		Debug.Log("OH MY GOD I GOT HIT");
	}
	bool isReachedGoal()
	{
		return (pos.x == WorldInfo.PLAYER_GOAL.x && pos.y == WorldInfo.PLAYER_GOAL.y) ;
	}
	public override bool move(int x, int y, bool tryAgain = true)
	{
		IsUpdated = true;
		if (!base.move(x, y, false)) return false;
		
		if (isReachedGoal()) EVENT_REACHEED_GOAL();
		else EVENT_MOVED();
		return true;
	}
	
}
