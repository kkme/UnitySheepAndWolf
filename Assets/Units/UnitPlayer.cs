using UnityEngine;
using System.Collections;

public class UnitPlayer : UnitBase
{
	public delegate void EVENTHDR_PlayerMoved();
	static public event EVENTHDR_PlayerMoved EVENT_PlayerMoved = delegate { };

	void Awake()
	{
		WorldInfo.unitPlayer = this;
	}
	public override bool move(Vector2 dir)
	{
		var posBefore = pos;
		if(!base.move(dir) ) return false;
		EVENT_PlayerMoved();
		return true;
	}
	
}
