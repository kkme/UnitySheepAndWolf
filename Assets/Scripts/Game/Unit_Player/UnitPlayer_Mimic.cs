using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitPlayer_Mimic : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE_INT_INT
		EVENT_EXPLOSION = delegate { };

	public override bool helperExplode(int x, int y)
	{
		var result = base.helperExplode(x, y);
		if (result) EVENT_EXPLOSION(x, y);
		return result;
	}
	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.PLAYER;
		}
	}

	public override List<List<UnitBase>> helperGetListOfUpdated()
	{
		return new List<List<UnitBase>>() { WorldInfo.unitsPlayers, WorldInfo.unitsUpdate00 };
	}
	public override bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return type == KEnums.UNIT.ENEMY;
	}
	public override void Awake()
	{
		base.Awake();
		isSwappable = true;
	}
	public override void KUpdate()
	{
		base.KUpdate();
		int x = (int)(pos.x+ WorldInfo.PLAYER_INPUT.x), y = (int)(pos.y+ WorldInfo.PLAYER_INPUT.y);
		if(!isIndexValid(x,y)) return;

		var at = helperGetGrid()[x,y];

		if (at == null || at.typeMe != KEnums.UNIT.ENEMY){
			if (!move(WorldInfo.PLAYER_INPUT) && !isPushed && at.GetComponent<UnitPlayer_Mimic>() != null) 
				swap(at);
		}
		else moveAttack(WorldInfo.PLAYER_INPUT, false, true);
		//you need to follow the last input of the player, just a simple move that is
		//save the last input somewhere? where? WorldInfo of course 
	}
}
