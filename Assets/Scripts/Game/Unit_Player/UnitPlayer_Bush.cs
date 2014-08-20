using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitPlayer_Bush : UnitUpdated
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
			return KEnums.UNIT.ENVIRONMENT;
		}
	}
	public override List<List<UnitBase>> helperGetListOfUpdated()
	{
		return new List<List<UnitBase>>() { WorldInfo.unitsUpdate00 };
	}
	public override void Awake()
	{
		base.Awake();
		isDestroyable_simpleAttack = false;
		isSwappable = true;
	}
}
