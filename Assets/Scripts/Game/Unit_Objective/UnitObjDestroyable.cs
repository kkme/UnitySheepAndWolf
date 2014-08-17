using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObjDestroyable : UnitUpdated
{
	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.ENVIRONMENT;
		}
	}
	public override void Awake()
	{
		isDestroyable_simpleAttack = false;
		isDestroyable_bomb = true;
	}
}
