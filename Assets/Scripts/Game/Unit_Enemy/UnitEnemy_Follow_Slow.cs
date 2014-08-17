using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Follow_Slow : UnitEnemy
{
	Vector2 dirContinue;
	public override bool processLocation(int x, int y)
	{
		var dir = new Vector2(x, y) - pos;
		var unit = WorldInfo.gridUnits[x, y];
		if (unit != null) unit.turn();
		unit = WorldInfo.gridUnits[x, y];
		if ((unit == null || unit.typeMe == KEnums.UNIT.PLAYER) && moveAttackRotation(dir)) return true;
		return false;
	}
	bool KUpdate_PredictingMovement(int pPosX, int pPosY)
	{
		int[] dirs = new int[2] { (dirFacing+1)%4,(dirFacing+3)%4};
		for (int i = 0; i < 2; i++)
		{
			int d = dirs[i];
			int posX = (int)pos.x + dirPath[d][0],
				posY = (int)pos.y + dirPath[d][1];
			int score = helperGetScore(posX, posY, pPosX, pPosY);
			if (score != 1) continue;
			if (moveAttackRotation(new Vector2(dirPath[d][0], dirPath[d][1]))) return true;
		}
		return false;
	}
	bool hasPredicted = false;
	public override void KUpdate()
	{
		base.KUpdate();
		
		var pPos = WorldInfo.getClosestPlayerUnit(pos,dirFacing).pos;
		int score = findPathToUnit((int)pPos.x, (int)pPos.y);
		if (score ==-1) return;
		
		var dis = pPos - pos;
		if (score == 1 && (int)Mathf.Abs(dis.x) == 1
			&& !hasPredicted
			&& (int)Mathf.Abs(dis.y) == 1
			&& KUpdate_PredictingMovement((int)pPos.x, (int)pPos.y))
		{
			hasPredicted = true;
			return;
		}
		else hasPredicted = false;
		if (!processLocation(closestTileX, closestTileY))
		{
			var otherRoutes = getOptimalRoute((int)pos.x, (int)pos.y, (int)pPos.x, (int)pPos.y);
			foreach (var r in otherRoutes){
				if (processLocation(r[0], r[1])) return; 
			}
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
