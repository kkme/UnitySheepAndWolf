using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Follow_Slow : UnitEnemy
{
	bool isFailed = false;
	Vector2 dirContinue;
	public override void KUpdate()
	{
		base.KUpdate();
		if (isFailed)
		{
			isFailed = false;
			if(moveAttackRotation(dirContinue)) return;
			
		}
		var dis = WorldInfo.getClosestPlayerUnit(pos).pos - pos;
		Vector2 dirFirst = Vector2.zero, dirSecond = Vector2.zero;
		int dirAlternative = 0;

		if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
		{
			//try X
			dirFirst.x = helperGetUnit((int)dis.x);
			dirAlternative = helperGetUnit((int)dis.y);
			dirSecond.y = dirAlternative;
		}
		else
		{
			//try Y
			dirFirst.y = helperGetUnit((int)dis.y);
			dirAlternative = helperGetUnit((int)dis.x);
			dirSecond.x = dirAlternative;
		}

		if (moveAttackRotation(dirFirst)) return;
		else
		{
			isFailed = true;
			if (dirAlternative != 0){
				if(moveAttackRotation(dirSecond)){dirContinue = dirSecond; return;}
				var dirSecondNegative = new Vector2(-dirSecond.x, -dirSecond.y);
				if (moveAttackRotation(dirSecondNegative)) { dirContinue = dirSecondNegative; return; }
			}
			dirContinue = new Vector2(-dirFirst.x, -dirFirst.y);
			moveAttackRotation(dirContinue);//try the other direcction
		}
	}
}
