using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitBase
{
	static public event	KDels.EVENTHDR_REQUEST_SIMPLE 
							EVENT_MOVED = delegate { },
							EVENT_ATTACKED = delegate { },
							EVENT_REACHEED_GOAL = delegate { };
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
	bool isEnemyAt(int x, int y)
	{
		if (!helperIsIndexValid(x, y)) return false;
		var u = helperGetGrid()[x, y] as UnitBase;
		Debug.Log(u.TYPE);
		return u.TYPE == KEnums.UNIT.ENEMY;
	}
	public bool turn(Vector2 dir)
	{
		isUpdated = true;
		if (moveAttack(dir))
		{
			if (isReachedGoal()) EVENT_REACHEED_GOAL();
			else EVENT_MOVED();
			return true;
		}
		return false;

	}
	//public override bool move(int x, int y, bool tryAgain = true)
	//{
	//	IsUpdated = true;
	//	bool r = false;
	//	if (!base.move(x, y, false))
	//	{
	//		r = moveAttack(x, y);
	//	}
	//	if (isReachedGoal()) EVENT_REACHEED_GOAL();
	//	else EVENT_MOVED();
	//	return r;
	//}
	
}
