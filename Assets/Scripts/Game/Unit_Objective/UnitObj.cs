using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObj : UnitStatic
{
	override public void Awake()
	{
		base.Awake();
		isDestroyable = false;
	}
}
