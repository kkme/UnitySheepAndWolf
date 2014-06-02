using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	public Vector2 pos;
	public Vector2 POS { get { return pos; } }
	
	protected EnumUnitTypes.UNIT myType = EnumUnitTypes.UNIT.BASIC;

	void Awake()
	{
		WorldInfo.units.Add(this);
	}
	protected bool helperIsIndexValid(int x, int y)
	{
		return !(x < 0 || y < 0 ||
			x >= WorldInfo.worldSize.x || y >= WorldInfo.worldSize.y);
	}
	protected bool helperIsGridAvailable(EnumUnitTypes.UNIT?[,] grid, int x, int y)
	{
		if (!helperIsIndexValid(x, y) || grid[x, y] != null) return false;		
		return true;
	}
	public virtual bool move(Vector2 dir)
	{
		var posTo = pos + dir;
		if (!helperIsGridAvailable(WorldInfo.gridUnits, (int)posTo.x, (int)posTo.y)) return false;
		WorldInfo.gridUnits[(int)pos.x, (int)pos.y] = null;
		pos = posTo;
		WorldInfo.gridUnits[(int)pos.x, (int)pos.y] = myType;
		return true;
	}
}