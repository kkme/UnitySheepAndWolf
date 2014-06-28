using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KLevel_Unit
{
	public KEnums.UNIT type00;	//general type of the object
	public int type01;			//which version of the "generalized version" is it
	public int[] position;		

	public KLevel_Unit(KEnums.UNIT type00 = KEnums.UNIT.BASIC, int type01=0, int x=0, int y=0)
	{
		this.type00 = type00;
		this.type01 = type01;
		this.position = new int[] { x, y };
	}
}
