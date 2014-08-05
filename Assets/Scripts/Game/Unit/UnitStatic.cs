using UnityEngine;
using System.Collections;

public class UnitStatic : UnitBase
{
	public override void init()
	{
		base.init();
		registerOnGrid();
	}
	
	public override void Awake()
	{
		
		base.Awake();
		WorldInfo.unitsStatic.Add(this);
	}
	public override void OnDestroy()
	{
		base.OnDestroy();
		WorldInfo.unitsStatic.Remove(this);
	}
}
