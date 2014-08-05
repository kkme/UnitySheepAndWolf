using UnityEngine;
using System.Collections.Generic;

public class UnitUpdated : UnitBase
{
	//change this
	public virtual List<List<UnitBase>> helperGetListOfUpdated()
	{
		return new List<List<UnitBase>>() { WorldInfo.unitsUpdate01 };
	}
	public override void init()
	{
		base.init();
		registerOnGrid();
		var l = helperGetListOfUpdated();
		for (int i = 0; i < l.Count; i++) l[i].Add(this);
	}
	public override void Destroy()
	{
		base.Destroy();
		unRegisterOnGrid();
	}
	public override void OnDestroy()
	{
		base.OnDestroy();
		var l = helperGetListOfUpdated();
		for (int i = 0; i < l.Count; i++) l[i].Remove(this);
	}
	
}
