using UnityEngine;
using System.Collections.Generic;

using ExtensionsUnityVectors;

public class UnitEnemy : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE EVENT_HIT_PLAYER = delegate { };

	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.ENEMY;
		}
	}
	public override bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return type == KEnums.UNIT.PLAYER;
	}
	public override void Awake()
	{
		base.Awake();
		isDestroyable_simpleAttack = true;
	}
	bool isPlayerNextTo(Vector2 pos)
	{
		Vector2[] dics = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
		for (int i = 0; i < 4; i++)
		{

			var at = pos + dics[i];
			if (!isIndexValid((int)at.x, (int)at.y)) continue;
			if (isPlayerAt((int)at.x, (int)at.y)) return true;

		}
		return false;
	}
	protected bool isPlayerAt(int x, int y)
	{
		if (!isIndexValid(x, y)) return false;
		var g = helperGetGrid()[x, y] as UnitBase;
		return (g != null && g.typeMe == KEnums.UNIT.PLAYER);
	}

	protected bool isPlayerAt(Vector2 v)
	{
		return isPlayerAt((int)v.x, (int)v.y);
	}
	public bool isPlayerClose(int rangeW, int rangeH)
	{
		var diff = pos - WorldInfo.getClosestPlayerUnit(pos).pos;
		return Mathf.Abs(diff.x) <= rangeW && Mathf.Abs(diff.y) <= rangeH;
	}
	public override void KUpdate()
	{
		isUpdated = true;
	}
	protected bool findPath(int x, int y)
	{
		int dirIStart = -1;
		int xNew=0, yNew=0;
		bool[,] map = new bool[13, 13];
		int score = 9999;
		if (helperGetScore(x, y) == 1)
		{
			closestTileX = x; closestTileY = y;
			return true;
		}
		for (int i = 0; i < 4; i++)
		{
			xNew = x + dirPath[i][0];
			yNew = y + dirPath[i][1];
			if (!isIndexValid(xNew, yNew)) continue;
			var unit = helperGetGrid()[xNew, yNew];
			if (unit != null && unit.typeMe == KEnums.UNIT.ENVIRONMENT) continue;
			int scoreNew = helperGetScore(xNew, yNew);
			if (scoreNew < score)
			{
				dirIStart = i;
				score = scoreNew;
			}
		}
		if (dirIStart == -1) return false;
		return recursivePath(ref map, x, y);
	}
	int helperGetScore(int x, int y)
	{
		int s = Mathf.Abs(x - (int)pos.x) + Mathf.Abs(y - (int)pos.y);
		if (s != 0 && WorldInfo.gridUnits[x, y] != null) s++;
		return s;
	}
	Dictionary<int, int[]> dirPath = new Dictionary<int, int[]>(){
		{0, new int[2]{0,1}},{1, new int[2]{1,0}},{2, new int[2]{0,-1}},{3, new int[2]{-1,0}}
	};
	protected int closestTileX=0, closestTileY=0;
	bool recursivePath(ref bool[,] map, int x, int y)
	{
		if (map[x, y]) return false;
		map[x, y] = true;
		//Debug.Log("exploring " + x + " " + y);
		List<int> dirsAvailable = new List<int>();
		int scoreMin = 9999, dirNew = -1; //score : smaller, closer
		for (int i = 0; i < 4; i++)
		{
			int dirTemp = i;
			int xNew = x + dirPath[dirTemp][0], yNew = y+dirPath[dirTemp][1];
			if(!isIndexValid(xNew, yNew)) continue;
			var unit = WorldInfo.gridUnits[xNew, yNew];
			if (unit != null &&
				(unit.typeMe == KEnums.UNIT.ENVIRONMENT || (unit.typeMe== KEnums.UNIT.ENEMY && (unit.id ==0 || (unit.id > 4 &&unit.id < 10)))))
				continue;
			int scoreNew = helperGetScore(xNew, yNew);
			if (scoreNew < scoreMin)
			{
				dirNew = dirTemp;
				scoreMin = scoreNew;
				dirsAvailable.Insert(0, dirNew);
			}
			else dirsAvailable.Add(dirTemp);
		}
		if (scoreMin == 0) { closestTileX = x; closestTileY = y; return true; } // yeah the unit is right next to me, I need to register this somewhere
		foreach (var d in dirsAvailable)
		{
			if (recursivePath(ref map, x + dirPath[d][0], y + dirPath[d][1])) { return true; }
		}
		return false;
	}
}
