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

	void Awake()
	{
		renderer.enabled = false;
	}
	GameObject helperInstantiate(GameObject PREFAB, int x, int y)
	{
		var posFrom = transform.position;
		var pos = posFrom + sizeCell.mult(.5f + x, .5f + y).XYZ(.1f);
		var obj = Instantiate(PREFAB, pos, Quaternion.identity) as GameObject;
		obj.transform.localScale = sizeCell.XYZ(1);
		obj.transform.parent = transform;
		return obj;
	}
	void helperInstantiateUnitbase(UnitBase u, int x, int y)
	{
		if (WorldInfo.gridUnits[x, y] != null) return;
		var obj = helperInstantiate(u.gameObject,x,y).GetComponent<UnitBase>();
		obj.pos = new Vector2(x, y);
		positionUnit(obj);
	}
	void initTiles()
	{
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++) {
			int n = (int)(i * count.y + j);
			if (n % 2 == 0) continue;
			helperInstantiate(TILE_DEFAULT, i, j);
		}
	}

	public void initCorners()
	{
		for (int x = 1; x < count.x - 1; x++)
		{
			helperInstantiateUnitbase(TILE_EDGE, x, 0);
			helperInstantiateUnitbase(TILE_EDGE, x, (int)(count.y - 1));

		}
		for (int y = 0; y < count.y; y++)
		{
			helperInstantiateUnitbase(TILE_EDGE, 0, y);
			helperInstantiateUnitbase(TILE_EDGE, (int)(count.x - 1), y);
		}
	}

	public void reset(int width, int height)
	{

		this.count = new Vector2(width, height);
		sizeCell = transform.worldScale().divide(new Vector3(count.x, count.y, 1)).XY();
		initTiles();
	}
	public void positionUnit(UnitBase unit)
	{
		var posNew = transform.position + (sizeCell.mult(.5f,.5f) + unit.pos.mult(sizeCell)).XYZ();
		unit.transform.position = posNew;
	}
	// Update is called once per frame


}
