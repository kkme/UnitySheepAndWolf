using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObj : UnitNonPlayer
{
	public override object[,] helperGetGrid()
	{
		return WorldInfo.gridObjectives;
	}
}
