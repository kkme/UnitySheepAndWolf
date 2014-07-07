using UnityEngine;
using System.Collections;

public class UnitStatic : UnitBase
{
	public override void init()
	{
		base.init();
		Debug.Log("rigistering on grid");
		registerOnGrid();
	}
}
