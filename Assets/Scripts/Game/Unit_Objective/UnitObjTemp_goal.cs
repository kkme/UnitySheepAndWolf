using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObjTemp_goal : UnitObjTemp
{
	public override void init()
	{
		WorldInfo.PLAYER_GOAL = pos;
	}
}
