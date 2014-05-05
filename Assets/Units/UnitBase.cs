using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	public Vector2 pos;
	public Vector2 POS { get { return pos; } }
	
	void Awake()
	{
		WorldInfo.units.Add(this);
	}
	bool helperIsGridAvailable(bool[,] grid, int x, int y)
	{
		if (x < 0 || y < 0 || 
			x >= WorldInfo.worldSize.x || y >= WorldInfo.worldSize.y ||
			grid[x,y] ) return false;		
		return true;
	}
	public virtual bool move(Vector2 dir)
	{
		var posTo = pos + dir;
		if (!helperIsGridAvailable(WorldInfo.gridCollision, (int)posTo.x, (int)posTo.y)) return false;
		WorldInfo.gridCollision[(int)pos.x, (int)pos.y] = false;
		pos = posTo;
		WorldInfo.gridCollision[(int)pos.x, (int)pos.y] = true;
		return true;
	}
}