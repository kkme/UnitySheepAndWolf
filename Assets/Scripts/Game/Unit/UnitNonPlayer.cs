using UnityEngine;
using System.Collections;

public class UnitNonPlayer : UnitBase
{

	public override void Awake()
	{
		WorldInfo.units.Add(this);
	}
	public void kill()
	{
		base.kill();
		WorldInfo.units.Remove(this);
	}

	
	
	void Update () {
	
	}
}
