using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE
							EVENT_KILLED = delegate { };
	static public event KDels.EVENTHDR_REQUEST_SIMPLE_POS 
		EVENT_CREATED = delegate{},
		EVENT_MOVED = delegate { };
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
		if (moveAttack(dir, false, isFirstHit: true)) return true;
		else
		{
			var u = WorldInfo.gridUnits[x, y];
			if (u != null && u.isSwappable && swap(u)) return true;
			
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
		base.kill(dirX, dirY);
	}
	bool helperIsStuck(int x, int y)
	{
		return (!isIndexValid(x, y) || 
			(WorldInfo.gridUnits[x, y] != null && !WorldInfo.gridUnits[x, y].isDestroyable_simpleAttack));
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
