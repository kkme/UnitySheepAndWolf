using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Spawn : UnitEnemy
{
	internal GameObject PREFAB_SPAWN = null;
	internal UnitBase unit;
	internal TYPE_ATTACK spawnAttack;
	internal int spawn_dirFacing, turnCount = 0;
	internal bool 
		isTrap,
		isUpdateRotation,
		isSpawnAsSpawnner,
		spawn_isBomb,
		spawn_isDestroyable_simpleAttack,
		spawn_isDestroyable_bomb;

	
	public void setSpawnEnemy(GameObject PREFAB, 
		int dir, bool isBomb, UnitBase.TYPE_ATTACK typeAttack, 
		bool isDestroyable_simpleAttack, bool isDestroyable_bomb,bool isTrap)
	{
		int id = PREFAB.GetComponent<UnitBase>().id;
		isUpdateRotation = id >0 && id<= 4;
		if (id == 0)
		{
			this.isTrap = true;
			isSpawnAsSpawnner = true;
		}
		else this.isTrap = isTrap;
		PREFAB_SPAWN = PREFAB;
		spawn_dirFacing = dir;
		spawnAttack = typeAttack;
		spawn_isDestroyable_simpleAttack = isDestroyable_simpleAttack;
		spawn_isDestroyable_bomb = isDestroyable_bomb;
		spawn_isBomb = isBomb;
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
		u.gameObject.transform.position = transform.position;
		u.transform.parent = this.transform;
		return u;
		// don't enable it right now since enabling it will result in "active in game" state
	}
	void spawnNew()
	{
		if (unit != null)
		{
			if (isSpawnAsSpawnner)
			{
				var dummy = unit.pos;
				unit.pos = this.pos; //reLink
				var copy = (Instantiate(Dir_GameObjects.dicUnits[this.typeMe][this.id],
							this.transform.position, this.transform.rotation) as GameObject)
							.GetComponent<UnitEnemy_Spawn>();
				copy.pos = dummy;
				copy.isUpdated = true;
				copy.isMoved = true;
				copy.transform.parent = transform.parent;
				copy.setSpawnEnemy(PREFAB_SPAWN, spawn_dirFacing, spawn_isBomb, spawnAttack, spawn_isDestroyable_simpleAttack, spawn_isDestroyable_bomb, isTrap);

				copy.init();
				registerOnGrid();
				
				return;
			}

			unit.transform.parent = transform.parent;
			unit.init();
			spawn_dirFacing = unit.dirFacing;
		}
		unit = spawn();
		registerOnGrid();
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
		if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y) spawnNew();
		else if(isUpdateRotation)
		{
			if (isTrap) UpdateTrap();
			else unit.KUpdate();
			if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y) spawnNew();
		}
	}
	void UpdateTrap()
	{
		bool isPathClear = findPath((int)WorldInfo.getClosestPlayerUnit(pos).pos.x, (int)WorldInfo.getClosestPlayerUnit(pos).pos.y);
		if (!isPathClear) return;
		
		unit.isUpdated = true; // to prevent recursioning back to "unit"
		if(unit.move(closestTileX,closestTileY, true)) return;
		//Lets try to move anywhere else since I failed that direction. 
		if (unit.move(new Vector2(0, 1))) return;
		else if (unit.move(new Vector2(1, 0))) return;
		else if (unit.move(new Vector2(0, -1))) return;
		else if (unit.move(new Vector2(0, -1))) return;

	}
	override public void kill(int dirX, int dirY)
	{
		base.kill(dirX,dirY);
		unit.kill(dirX, dirY);
	}
}