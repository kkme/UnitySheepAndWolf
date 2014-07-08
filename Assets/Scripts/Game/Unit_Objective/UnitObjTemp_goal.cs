using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObjTemp_goal : UnitObj
{
	public override void init()
	{
		base.init();
		WorldInfo.PLAYER_GOAL = pos;
	}
	public override void Awake()
	{
		base.Awake();
		isAttackable = true;
	}
}
