using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class Board : UIItem
{

	public GameObject TILE_DEFAULT;
	public UnitBase TILE_EDGE;
	
	Vector2 count;
	Vector2 sizeCell;
	GameObject[,] tiles,edges;
	void Awake()
	{
		renderer.enabled = false;
	}
	public void init(int width, int height)
	{

		this.count = new Vector2(width, height);
		sizeCell = transform.worldScale().divide(new Vector3(count.x, count.y, 1)).XY();
		initTiles();
		initCorners();
	}
	GameObject helperInstantiate(GameObject PREFAB, int x, int y)
	{
		var posFrom = transform.position;
		var pos = posFrom + sizeCell.mult(x,  y).XYZ(.1f);
		var obj = Instantiate(PREFAB, pos, Quaternion.identity) as GameObject;
		obj.transform.localScale = sizeCell.XYZ(1);
		obj.transform.parent = transform;
		return obj;
	}
	GameObject helperInstantiateUnitbase(UnitBase u, int x, int y)
	{
		if (WorldInfo.gridUnits[x, y] != null) return null;
		var obj = helperInstantiate(u.gameObject,x,y).GetComponent<UnitBase>();
		obj.pos = new Vector2(x, y);
		positionUnit(obj);
		obj.init();
		return obj.gameObject;
	}
	void initTiles()
	{
		tiles = new GameObject[(int)count.x, (int)count.y];
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++) {
			
			if (j % 2 ==i%2) continue;
			tiles[i,j] = helperInstantiate(TILE_DEFAULT, i, j);
		}
	}

	void initCorners()
	{
		edges = new GameObject[13, 13];
		for (int x = 1; x < count.x - 1; x++)
		{
			edges[x,0]=helperInstantiateUnitbase(TILE_EDGE, x, 0);
			edges[x,(int)(count.y - 1)]= helperInstantiateUnitbase(TILE_EDGE, x, (int)(count.y - 1));

		}
		for (int y = 0; y < count.y; y++)
		{
			edges[0,y]=helperInstantiateUnitbase(TILE_EDGE, 0, y);
			edges[(int)(count.x - 1), y] = helperInstantiateUnitbase(TILE_EDGE, (int)(count.x - 1), y);
		}
	}

	public void positionUnit(UnitBase unit)
	{
		var posNew = transform.position + (unit.pos.mult(sizeCell)).XYZ();
		unit.transform.position = posNew;
	}

	void recur(ref bool[,] isChecked, int x, int y){
		if (x < 0 || y < 0 || x > 12 || y > 12 || isChecked[x,y]) return;
		isChecked[x, y] = true;
		if (tiles[x, y] != null) tiles[x, y].SetActive(true);
		if (edges[x, y] != null) edges[x, y].SetActive(true);
		var unit = WorldInfo.gridUnits[x,y];
		if (unit != null && unit.typeMe == KEnums.UNIT.ENVIRONMENT && unit.id == 1)
			return;//wall here don't proceed further
		recur(ref isChecked, x+1, y);
		recur(ref isChecked, x-1, y);
		recur(ref isChecked, x, y + 1);
		recur(ref isChecked, x, y - 1);
		//corners
		recur(ref isChecked, x+1, y + 1);
		recur(ref isChecked, x-1, y + 1);
		recur(ref isChecked, x+1, y - 1);
		recur(ref isChecked, x-1, y - 1);
	}
	// Update is called once per frame
	public void lightOff()
	{
		foreach (var t in tiles) if (t != null) t.SetActive(false);
		foreach (var e in edges) if (e != null) e.SetActive(false);
		bool[,] grid = new bool[13,13];
		recur(ref grid, (int)WorldInfo.unitPlayer_real.pos.x, (int)WorldInfo.unitPlayer_real.pos.y);
			
	
	}


}
