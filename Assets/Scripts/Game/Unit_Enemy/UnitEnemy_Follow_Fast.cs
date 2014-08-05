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
		var dis = WorldInfo.getClosestPlayerUnit(pos).pos - pos;
		var dir = dis.dir();
		if (dir.x == 0) dir.x = -1 + 2* Random.Range(0, 1);
		if (dir.y == 0) dir.y = -1 + 2* Random.Range(0, 1);
		if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
		{
			if (moveAttack( new Vector2(dir.x,0))||
				moveAttack(new Vector2(0, dir.y) )||
				moveAttack(new Vector2(0, -dir.y))||
				moveAttack(new Vector2(-dir.x,0)))
			return;
		}

		if (moveAttack(new Vector2(0, dir.y)) ||
			moveAttack(new Vector2(dir.x, 0)) ||
			moveAttack(new Vector2(-dir.x, 0)) ||
			moveAttack(new Vector2(0, -dir.y))) return;
	}
}
