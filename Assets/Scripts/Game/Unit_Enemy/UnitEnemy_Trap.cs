using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitEnemy_Trap : UnitEnemy
{
	public List<Vector2> dirs; //checks these directions
	void trapAttack(Vector2 dir)
	{
		
	}
	void KUpdateDir(Vector2 at)
	{
		foreach (var d in dirs)
		{
			
		}
	}
	public override void KUpdate()
	{
		base.KUpdate();
		//if(!isPlayerClose(1, 1)) return;//proceed only if player is close nearby since it's a trap
		foreach (var d in dirs)
		{
			var at = pos + d;
			if (isPlayerAt(pos+d)) { 
				moveAttack(d); 
				return; }
		}
	}
}