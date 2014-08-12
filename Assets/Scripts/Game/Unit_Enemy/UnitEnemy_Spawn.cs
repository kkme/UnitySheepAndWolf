using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Spawn : UnitEnemy
{
	internal GameObject PREFAB_SPAWN = null;
	UnitBase unit;
	ATTACK_TYPE spawnAttack;
	int spawn_dirFacing, turnCount = 0;
	bool isTrap,
		spawn_isBomb,
		spawn_isDestroyable_simpleAttack,
		spawn_isDestroyable_bomb;

	
	public void setSpawnEnemy(GameObject PREFAB, 
		int dir, bool isBomb, UnitBase.ATTACK_TYPE typeAttack, 
		bool isDestroyable_simpleAttack, bool isDestroyable_bomb,bool isTrap)
	{
		PREFAB_SPAWN = PREFAB;
		spawn_dirFacing = dir;
		spawnAttack = typeAttack;
		spawn_isDestroyable_simpleAttack = isDestroyable_simpleAttack;
		spawn_isDestroyable_bomb = isDestroyable_bomb;
		spawn_isBomb = isBomb;
		this.isTrap = isTrap;
	}
	UnitBase spawn()
	{
		var u = (Instantiate(PREFAB_SPAWN) as GameObject).GetComponent<UnitBase>();
		u.pos = this.pos;
		u.dirFacing = spawn_dirFacing;
		u.typeAttack = spawnAttack;
		u.isBomb = spawn_isBomb;
		u.isDestroyable_simpleAttack = spawn_isDestroyable_simpleAttack;
		u.isDestroyable_bomb = spawn_isDestroyable_bomb;
		u.gameObject.transform.position = new Vector3( pos.x,pos.y,0);
		u.transform.parent = this.transform;
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
		if (++turnCount % 4 != 0) return;
		turnCount = 0;
		if (isTrap) UpdateTrap();
		else unit.KUpdate();

		if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y)
		{
			unit.transform.parent = transform.parent;
			unit.init();
			unit = spawn();
			registerOnGrid();
		}
	}
	void UpdateTrap()
	{
		var dis = WorldInfo.getClosestPlayerUnit(pos).pos - pos;
		if (
			((int)dis.x == 1 && (int)dis.y == 0	)||
			((int)dis.x == 0 && (int)dis.y == 1	) 
			)return;
		//spawn at the cloest 
		var dirMove = (Mathf.Abs(dis.x) > Mathf.Abs(dis.y)) ?
			new Vector2(helperGetUnit((int)dis.x), 0) : new Vector2(0,helperGetUnit((int)dis.y));
		unit.isUpdated = true;
		if(unit.move(dirMove,true)) return;

		//has failed now lets try to move in all four direction
		if (unit.move(new Vector2(0, 1))) return;
		else if (unit.move(new Vector2(1, 0))) return;
		else if (unit.move(new Vector2(0, -1))) return;
		else if (unit.move(new Vector2(0, -1))) return;
		//try to spawn at dirSpawn
		//if I can't, then try default spawn locations.

	}
	override public void kill(int dirX, int dirY)
	{
		base.kill(dirX,dirY);
		unit.kill(dirX, dirY);
	}
}