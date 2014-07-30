using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Spawn : UnitEnemy
{
	GameObject PREFAB_SPAWN = null;
	UnitBase unit;
	ATTACK_TYPE spawnAttack;
	int spawnDir;
	bool spawnIsDestroyable, spawnIsBomb;

	public void setSpawnEnemy(GameObject PREFAB, int rotation = 0 , ATTACK_TYPE typeAttack = ATTACK_TYPE.KILL, bool isDestroyable = true,bool isBomb = false, bool isTrap = false)
	{
		spawnDir = rotation;
		PREFAB_SPAWN = PREFAB;
		spawnAttack = typeAttack;
		spawnIsDestroyable = isDestroyable;
		spawnIsBomb = isBomb;
	}
	UnitBase spawn()
	{
		var u = (Instantiate(PREFAB_SPAWN) as GameObject).GetComponent<UnitBase>();
		u.pos = this.pos;
		u.dirFacing = spawnDir;
		u.typeAttack = spawnAttack;
		u.isBomb = spawnIsBomb;
		u.isDestroyable = spawnIsDestroyable;
		u.gameObject.transform.position = new Vector3( pos.x,pos.y,0);
		return u;
		// don't enable it right now since enabling it will result in "active in game" state
	}
	public override void Start()
	{
		base.Start();
		unit = spawn(); 
	}
	public override void KUpdate()
	{
		base.KUpdate();
		unit.KUpdate();

		Debug.Log(unit);
		Debug.Log(unit.pos);
		Debug.Log(pos);

		if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y)
		{
			Debug.Log("spawning");
			unit.init();
			unit = spawn();
			registerOnGrid();
		}
		//count the turns
	}
}