using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitPlayer_Bush : UnitUpdated
{
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
		isSwappable = true;
	}
}
