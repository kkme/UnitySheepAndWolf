using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataUnit
{

	public static explicit operator DataUnit(UnitBase unit)
	{
		var data = new DataUnit(unit.typeMe, unit.id, (int)unit.pos.x, (int)unit.pos.y, unit.dirFacing, unit.typeAttack, unit.isBomb, unit.isSwappable, unit.isDestroyable_simpleAttack, unit.isDestroyable_bomb);
		var spawn = unit.GetComponent<UnitEnemy_Spawn>();
		if (spawn != null)
		{
			var _u = spawn.PREFAB_SPAWN.GetComponent<UnitBase>();
			Debug.Log("OTHER SPAWN " + spawn.PREFAB_SPAWN + " , " + _u);
			Debug.Log(_u.typeMe + " " + _u.id);
			data.other = (DataUnit)spawn.PREFAB_SPAWN.GetComponent<UnitBase>();
			
		}
		else data.other = null;

		//if it is spawn then do something else
		return data;
	}

	public DataUnit(KEnums.UNIT typeUnit, int id, int x, int y, int dirFacing,UnitBase.TYPE_ATTACK typeAttack, bool isBomb,bool isSwappable,
		bool isDestroyable_simpleAttack, bool isDestroyable_bomb, DataUnit other = null)
	{
		this.typeUnit = typeUnit;
		this.id = id;
		this.x = x;
		this.y = y;
		this.dirFacing = dirFacing;
		this.typeAttack = typeAttack;
		this.isBomb = isBomb;
		this.isSwappable = isSwappable;
		this.isDestroyable_simpleAttack = isDestroyable_simpleAttack;
		this.isDestroyable_bomb = isDestroyable_bomb;
		this.other = other;
	}
	public KEnums.UNIT typeUnit;
	public int x,y,
		id,dirFacing;
	public UnitBase.TYPE_ATTACK typeAttack;
	public bool
		isBomb,
		isSwappable,
		isDestroyable_simpleAttack,
		isDestroyable_bomb;

	public DataUnit other;
}
