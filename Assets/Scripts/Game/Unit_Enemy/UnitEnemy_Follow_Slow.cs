using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Follow_Slow : UnitEnemy
{
	Vector2 dirContinue;
	public override void KUpdate()
	{
		base.KUpdate();
		
		var p = WorldInfo.getClosestPlayerUnit(pos);
		if(!findPath((int)p.pos.x, (int)p.pos.y)) return;
		var dis = new Vector2(closestTileX, closestTileY) - pos;
		Vector2 dirFirst = Vector2.zero, dirSecond = Vector2.zero;
		int dirAlternative = 0;
		if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
		{
			//try X
			dirFirst.y = 0;
			dirFirst.x = helperGetUnit((int)dis.x);
			dirAlternative = helperGetUnit((int)dis.y);
			dirSecond.y = dirAlternative;
		}
		else
		{
			//try Y
			dirFirst.x = 0;
			dirFirst.y = helperGetUnit((int)dis.y);
			dirAlternative = helperGetUnit((int)dis.x);
			dirSecond.x = dirAlternative;
		}
		var unit = WorldInfo.gridUnits[closestTileX, closestTileY];
		if (unit != null && unit.typeMe != KEnums.UNIT.PLAYER) {
			unit.turn();
			unit = WorldInfo.gridUnits[closestTileX, closestTileY];
			if (unit != null) {turnOtherDirection(dirAlternative, dirFirst, dirSecond); return;}
		}
	

		if (moveAttackRotation(dirFirst)) return;
		else
		{
			turnOtherDirection(dirAlternative, dirFirst, dirSecond);
		}
	}
	void turnOtherDirection(int dirAlternative, Vector2 dirFirst, Vector2 dirSecond)
	{
		if (dirAlternative != 0)
		{
			if (moveAttackRotation(dirSecond)) { dirContinue = dirSecond; return; }
			var dirSecondNegative = new Vector2(-dirSecond.x, -dirSecond.y);
			if (moveAttackRotation(dirSecondNegative)) { dirContinue = dirSecondNegative; return; }
		}
		dirContinue = new Vector2(-dirFirst.x, -dirFirst.y);
		moveAttackRotation(dirContinue);//try the other direcction
	}
	/**
	 * 
	 * public override void KUpdate()
	{
		base.KUpdate();
		if (isFailed)
		{
			isFailed = false;
			if(moveAttackRotation(dirContinue)) return;
			
		}
		var p = WorldInfo.getClosestPlayerUnit(pos);
		findPath((int)p.pos.x, (int)p.pos.y);
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
	 * **/
}
