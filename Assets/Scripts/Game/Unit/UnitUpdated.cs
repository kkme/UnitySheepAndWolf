using UnityEngine;
using System.Collections;

public class UnitUpdated : UnitBase
{
	internal protected bool isActive = true;
	public override void init()
	{
		base.init();
		registerOnGrid();
		WorldInfo.units.Add(this);
	}
}
