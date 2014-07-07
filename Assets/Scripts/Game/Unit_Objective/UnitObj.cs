using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObj : UnitStatic
{
	void Awake()
	{
		isAttackable = false;
		isPushable = false;
	}
}
