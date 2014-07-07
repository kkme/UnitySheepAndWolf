using UnityEngine;
using System.Collections;

public class UnitUpdated : UnitBase
{
	public override void init()
	{
		base.init();
		Debug.Log("rigistering on grid");
		registerOnGrid();
	}
	public override void Awake()
	{
		base.Awake();
		WorldInfo.units.Add(this);
	}
	public override void kill()
	{
		base.kill();
		WorldInfo.units.Remove(this);
	}
}
