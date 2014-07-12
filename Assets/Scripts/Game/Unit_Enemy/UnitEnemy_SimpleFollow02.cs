using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_SimpleFollow02 : UnitEnemy
{
	public bool isSleep = true;
	public bool fallBackSleep = true;
	public int RANGE_HORIZONTAL, RANGE_VERTICAL;
	//public override void KUpdate()
	//{
	//	IsUpdated = true;
	//	if (isPlayerClose(RANGE_HORIZONTAL, RANGE_VERTICAL))
	//	{
	//		if (isSleep) { isSleep = false; return; }
	//		var dir = helperGetDir(WorldInfo.unitPlayer);
	//		move(dir);
	//	}
	//	else if (fallBackSleep) isSleep = true;
	//}
}
