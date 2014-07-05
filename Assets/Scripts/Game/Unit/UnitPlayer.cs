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
	public bool moveAttack(int x, int y)
	{
		Debug.Log(x + " " + y);
		Debug.Log("PLAYER : move attack " + isEnemyAt(x, y) + " WHICH WAS " + helperIsIndexValid(x, y));
		if (isEnemyAt(x, y))
		{
			(helperGetGrid()[x, y] as UnitEnemy).kill();
			helperSetPosition(x, y);
			return true;
		}
		return false;
	}
	public override bool move(int x, int y, bool tryAgain = true)
	{
		IsUpdated = true;
		if (!base.move(x, y, false))
		{
			if (moveAttack(x, y)) return true;
			return false;
		}
		if (isReachedGoal()) EVENT_REACHEED_GOAL();
		else EVENT_MOVED();
		return true;
	}
	
}
