using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KEnums;

class Level00 : KLevel
{
	public Level00() : base(3,3)
	{
		addUnit(UNIT.PLAYER, 0, 0, 0);
		addUnit(UNIT.ENEMY, ENEMY.WOLF, 0, 1);
		addUnit(UNIT.ENEMY, ENEMY.BEER, 0, 2);
	}
}
