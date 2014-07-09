﻿using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitUpdated
{
	static public event	KDels.EVENTHDR_REQUEST_SIMPLE 
							EVENT_MOVED = delegate { },
							EVENT_ATTACKED = delegate { },
							EVENT_REACHEED_GOAL = delegate { };
	public override void Awake()
	{
		base.Awake();
		myType = KEnums.UNIT.PLAYER;
		WorldInfo.unitPlayer = this;
		isAttackable = true;
		isPushable = true;
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
		if (moveAttack(dir))
		{
			if (isReachedGoal()) EVENT_REACHEED_GOAL();
			else EVENT_MOVED();
			return true;
		}
		return false;
	}
	public override bool attacked()
	{
		EVENT_ATTACKED();
		return base.attacked();
	}
	public override void kill()
	{
		base.kill();
		//EVENT_ATTACKED();
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
