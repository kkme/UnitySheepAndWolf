using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitBase
{
	public delegate void EVENTHDR_PlayerMoved();
	static public event EVENTHDR_PlayerMoved EVENT_PlayerMoved = delegate { };

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
	public override bool move(int x, int y, bool tryAgain = true)
	{
		IsUpdated = true;
		if (!base.move(x, y, false)) return false;
		EVENT_PlayerMoved();
		return true;
	}
	
}
