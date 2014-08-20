using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE
							EVENT_KILLED = delegate { };
	static public event KDels.EVENTHDR_REQUEST_SIMPLE_INT_INT
		EVENT_CREATED = delegate { },
		EVENT_MOVED = delegate { },
		EVENT_EXPLOSION = delegate { };
	
	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.PLAYER;
		}
	}
	
	public override bool helperExplode(int x, int y)
	{
		var result = base.helperExplode(x, y);
		if (result) EVENT_EXPLOSION(x, y);
		return result;
	}
	public override System.Collections.Generic.List<System.Collections.Generic.List<UnitBase>> helperGetListOfUpdated()
	{
		return new System.Collections.Generic.List<System.Collections.Generic.List<UnitBase>>() { WorldInfo.unitsPlayers };
	}
	public override void Awake()
	{
		base.Awake();
		WorldInfo.unitPlayer_real = this;
	}
	public override void Start()
	{
		base.Start();
		EVENT_CREATED((int)pos.x, (int)pos.y);
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
		var u = WorldInfo.gridUnits[x, y];
		if ((u == null || u.typeMe == KEnums.UNIT.ENEMY || 
			(u.typeMe == KEnums.UNIT.ENVIRONMENT && u.id != 0 && u.id != 3 ))) { 
			if (moveAttack(dir, false, isFirstHit: true))
				return true;
		}
		else if ( u.isSwappable && swap(u))
			return true;
		else if (u.typeMe == KEnums.UNIT.ENVIRONMENT && u.id == 0){
			pos += dir * .75f;
			isMoved = true;
			u.attacked((int)dir.x, (int)dir.y);
			return true;
		}

		return false;
	}
	public override bool move(int x, int y, bool tryAgain = true)
	{
		var result = base.move(x, y, tryAgain);
		if (result) EVENT_MOVED((int)pos.x, (int)pos.y);
		return result;
	}
	


	public override bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return type == KEnums.UNIT.ENEMY || type == KEnums.UNIT.ENVIRONMENT;
	}
	
	public override void kill(int dirX, int dirY)
	{
		EVENT_KILLED();
		gameObject.SetActive(false);
		base.kill(dirX, dirY);
	}
	bool helperIsStuck(int x, int y)
	{
		return (!isIndexValid(x, y) ||
			(WorldInfo.gridUnits[x, y] != null && !WorldInfo.gridUnits[x, y].isDestroyable_simpleAttack && !WorldInfo.gridUnits[x, y].isSwappable));
	}
	public bool amIStuck()
	{
		int x = (int)pos.x, y=(int)pos.y;
		return 
			helperIsStuck(x-1, y) &&
			helperIsStuck(x+1, y) &&
			helperIsStuck(x, y+1) &&
			helperIsStuck(x, y-1);

	}
	public override void Destroy()
	{
		Debug.Log("DESTORTYED");
		base.OnDestroy();
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
