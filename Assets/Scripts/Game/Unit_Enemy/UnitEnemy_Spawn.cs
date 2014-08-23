using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitEnemy_Spawn : UnitEnemy
{
	internal KDels.EVENTHDR_REQUEST_SIMPLE_INT_INT
		EVENT_SPAWN = delegate { };
	internal KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_SPAWN_SOON = delegate { },
		EVENT_SPAWN_ATTEMPTED = delegate { };

	internal int turnCount;
	internal GameObject PREFAB_SPAWN = null;
	internal UnitBase unit;
	internal TYPE_ATTACK spawn_Attack;
	internal int spawn_dirFacing;
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
		this.isDestroyable_simpleAttack = typeAttack != TYPE_ATTACK.PUSH;
		this.isBomb = isBomb;
		PREFAB_SPAWN = PREFAB;
		spawn_dirFacing = dir;
		spawn_Attack = typeAttack;
		spawn_isDestroyable_simpleAttack = isDestroyable_simpleAttack;
		spawn_isDestroyable_bomb = isDestroyable_bomb;
		spawn_isBomb = isBomb;
	}
	UnitBase spawn()
	{
		var u = (Instantiate(PREFAB_SPAWN) as GameObject).GetComponent<UnitBase>();
		u.pos = this.pos;
		u.dirFacing = spawn_dirFacing;
		u.typeAttack = spawn_Attack;
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
		EVENT_SPAWN((int)pos.x, (int)pos.y);
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
				copy.setSpawnEnemy(PREFAB_SPAWN, spawn_dirFacing, spawn_isBomb, spawn_Attack, spawn_isDestroyable_simpleAttack, spawn_isDestroyable_bomb, isTrap);

				copy.init();
				registerOnGrid();
				
				return;
			}
			unit.typeAttack = spawn_Attack;
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
		spawnNew();
		if (turnCount == 3) EVENT_SPAWN_SOON();
	}
	public override void KUpdate()
	{
		base.KUpdate();
		var pPos = WorldInfo.getClosestPlayerUnit(pos, -1).pos;
		bool isPathClear = findPathToUnit((int)pPos.x, (int)pPos.y) != -1;
		if (!isPathClear) return;
		
		unit.typeAttack = UnitBase.TYPE_ATTACK.NONE;
		if (++turnCount % 4 != 0)
		{
			if (turnCount == 3) EVENT_SPAWN_SOON();
			return;
		}
		EVENT_SPAWN_ATTEMPTED();
		turnCount = 0;
		if (isTrap) UpdateTrap((int)pPos.x,(int)pPos.y);
		else unit.KUpdate();
		if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y) spawnNew();
		else if(isUpdateRotation)
		{
			if (isTrap) UpdateTrap((int)pPos.x,(int)pPos.y);
			else unit.KUpdate();
			if ((int)unit.pos.x != (int)pos.x || (int)unit.pos.y != (int)pos.y) spawnNew();
		}
	}
	void UpdateTrap(int pPosX, int pPosY)
	{
		
		unit.isUpdated = true; // to prevent recursioning back to "unit"
		if(unit.move(closestTileX,closestTileY, true)) return;
		var routes = getOptimalRoute((int)pos.x, (int)pos.y, (int)pPosX, (int)pPosY);
		foreach (var r in routes)
		{
			if(unit.move(r[0], r[1])) return;
		}

	}
	override public void kill(int dirX, int dirY)
	{
		base.kill(dirX,dirY);
		unit.kill(dirX, dirY);
	}
}