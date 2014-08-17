using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitEnemy_Follow_Fast : UnitEnemy
{

	public override void Awake()
	{
		base.Awake();
		
	}
	int helperGetUnit(int n)
	{
		return (n == 0) ?1: n / Mathf.Abs(n);
	}
	public override void KUpdate()
	{
		base.KUpdate();

		var pPos = WorldInfo.getClosestPlayerUnit(pos,-1).pos;
		var pDis = pPos - pos;
		dirFacing = (Mathf.Abs(pDis.x) >Mathf.Abs( pDis.y))? 
			helperToDirFromRaw(new Vector2(pDis.x, 0)):
			helperToDirFromRaw(new Vector2(0, pDis.y));

		if (findPathToUnit((int)pPos.x, (int)pPos.y) == -1) return;
		var dir = new Vector2(closestTileX, closestTileY) - pos;
		if (moveAttack(new Vector2(closestTileX, closestTileY) - pos)) return;
		//
		var routes = getOptimalRoute((int)pos.x, (int)pos.y,(int)pPos.x,(int)pPos.y, dirFacing);
		foreach (var r in routes)
		{
			if(moveAttack(r[0], r[1]))return;
		}
	}
	public override void UpdateAnimation()
	{
		//ani.initAnimation(pos.x,pos.y, dirFacing);
		rSprite.move(pos.x, pos.y);
	}
 
}
