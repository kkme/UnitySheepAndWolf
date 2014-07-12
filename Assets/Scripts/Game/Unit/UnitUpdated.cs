using UnityEngine;
using System.Collections;

public class UnitUpdated : UnitBase
{
	public override void init()
	{
		base.init();
		registerOnGrid();
	}
	public override void Awake()
	{
		base.Awake();
		WorldInfo.units.Add(this);
	}
}
