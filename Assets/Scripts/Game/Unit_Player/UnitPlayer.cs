using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE
							EVENT_MOVED = delegate { },
							EVENT_KILLED = delegate { };
	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.PLAYER;
		}
	}
	public override void Awake()
	{
		base.Awake();
		WorldInfo.unitsPlayers.Add(this);
		WorldInfo.unitPlayer_real = this;
	}
	void EVENT_GOT_HIT()
	{
		Debug.Log("OH MY GOD I GOT HIT");
	}
	bool isEnemyAt(int x, int y)
	{
		if (!isIndexValid(x, y)) return false;
		var u = helperGetGrid()[x, y] as UnitBase;
		Debug.Log(u.typeMe);
		return u.typeMe == KEnums.UNIT.ENEMY;
	}
	public bool turn(Vector2 dir)
	{
		isUpdated = true;
		int x =(int)( pos.x + dir.x), y = (int)(pos.y + dir.y);
		if (!isIndexValid(x, y)) return false;
		if (moveAttack(dir, false,isFirstHit:true))
		{
			EVENT_MOVED();
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
		return type == KEnums.UNIT.ENEMY || type == KEnums.UNIT.ENVIRONMENT;
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
