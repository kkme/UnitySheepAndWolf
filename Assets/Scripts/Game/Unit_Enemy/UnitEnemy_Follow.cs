using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Follow : UnitEnemy
{
	public override void KUpdate()
	{
		base.KUpdate();
		var dir = helperGetDir(WorldInfo.unitPlayer);
		UnityEngine.Debug.Log(dir);
		move(dir);
	}
}
