using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitEnemy_Trap : UnitEnemy
{
	List<Vector2> dirs = new List<Vector2>(); //checks these directions
	public override void init()
	{
		base.init();
		dirs.Add(new Vector2(1, 0));
		dirs.Add(new Vector2(-1, 0));
		dirs.Add(new Vector2(0, 1));
		dirs.Add(new Vector2(0, -1));
	}
	void KUpdateDir(Vector2 dir)
	{
		var at = pos + dir;
		if(isPlayerAt(dir));
		

	}
	public override void KUpdate()
	{
		base.KUpdate();
		if(!isPlayerClose(1, 1)) return;//proceed only if player is close nearby since it's a trap
		foreach (var d in dirs)
		{

		}
	}
}