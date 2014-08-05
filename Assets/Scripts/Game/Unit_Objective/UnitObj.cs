using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObj : UnitStatic
{
	override public void Awake()
	{
		base.Awake();
		typeMe = KEnums.UNIT.ENVIRONMENT;
		isDestroyable_SimpleAttack = false;
		isDestroyable_bomb = false;
	}
}
