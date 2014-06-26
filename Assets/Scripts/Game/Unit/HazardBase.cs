using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HazardBase : UnitBase
{
	KEnums.Hazard myType;
	public override object[,] helperGetGrid()
	{
		return WorldInfo.gridHazard;
	}
}