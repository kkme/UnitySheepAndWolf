using UnityEngine;
using System.Collections.Generic;

public class UnitUpdated : UnitBase
{
	//change this
	public virtual List<List<UnitBase>> helperGetListOfUpdated()
	{
		return new List<List<UnitBase>>() { WorldInfo.unitsUpdate01 };
	}

	public override UnitBase init()
	{
		base.init();
		var l = helperGetListOfUpdated();
		for (int i = 0; i < l.Count; i++) l[i].Add(this);
		return (UnitBase)this;
	}
	
	public override void OnDestroy()
	{
		base.OnDestroy();
		var l = helperGetListOfUpdated();
		for (int i = 0; i < l.Count; i++) l[i].Remove(this);
	}
	
}
