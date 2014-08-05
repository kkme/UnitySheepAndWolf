using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitUpdated
{
	static public event	KDels.EVENTHDR_REQUEST_SIMPLE 
							EVENT_MOVED = delegate { },
							EVENT_KILLED = delegate { },
							EVENT_REACHEED_GOAL = delegate { };
	public override void Awake()
	{
		base.Awake();
		typeMe = KEnums.UNIT.PLAYER;
		WorldInfo.unitsPlayers.Add(this);
		WorldInfo.unitPlayer_real = this;
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
		if (!isIndexValid(x, y)) return false;
		var u = helperGetGrid()[x, y] as UnitBase;
		Debug.Log(u.TYPE);
		return u.TYPE == KEnums.UNIT.ENEMY;
	}
	public bool turn(Vector2 dir)
	{
		isUpdated = true;
		int x =(int)( pos.x + dir.x), y = (int)(pos.y + dir.y);
		if (!isIndexValid(x, y)) return false;
		if (moveAttack(dir, false,isFirstHit:true))
		{
			if (isReachedGoal()) EVENT_REACHEED_GOAL();
			else EVENT_MOVED();
			health = 1;
			return true;
		}
		else
		{
			var u = WorldInfo.gridUnits[x, y];
			if (u != null && u.isSwappable && swap(u)) return true;
		}
		return false;
	}
	


	public override bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return type == KEnums.UNIT.ENEMY;
	}
	
	public override void kill(int dirX, int dirY)
	{
		EVENT_KILLED();
		base.kill(dirX, dirY);
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
