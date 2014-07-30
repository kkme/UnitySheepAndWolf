using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitEnemy_Trap : UnitEnemy
{
	//public List<Vector2> dirs; //checks these directions
	public List<int> dirs;
	Dictionary<int, Vector2> dir = new Dictionary<int, Vector2>(){
		{0, new Vector2(0,1)},{1, new Vector2(1,0)},
		{2, new Vector2(0,-1)},{3, new Vector2(-1,0)}
	};
	public override void Awake()
	{
		base.Awake();
		for (int i = 0; i < dirs.Count; i++)
		{
			dirs[i] = (dirs[i] +  dirFacing ) % 4; 
		}
		isBomb = true;
	}
	public override void KUpdate()
	{
		base.KUpdate();
		//if(!isPlayerClose(1, 1)) return;//proceed only if player is close nearby since it's a trap
		foreach (var d in dirs)
		{
			var at = pos + dir[d];
			if (isPlayerAt(pos+dir[d])) {
				moveAttack(dir[d]); 
				return; 
			}
		}
	}
}