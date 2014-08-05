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
	void helperInstantiateUnitbase(UnitBase u, int x, int y)
	{
		if (WorldInfo.gridUnits[x, y] != null) return;
		var obj = helperInstantiate(u.gameObject,x,y).GetComponent<UnitBase>();
		obj.pos = new Vector2(x, y);
		positionUnit(obj);
		obj.init();
	}
	void initTiles()
	{
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++) {
			
			if (j % 2 ==i%2) continue;
			helperInstantiate(TILE_DEFAULT, i, j);
		}
	}

	void initCorners()
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

	
	public void positionUnit(UnitBase unit)
	{
		var posNew = transform.position + (unit.pos.mult(sizeCell)).XYZ();
		unit.transform.position = posNew;
	}
	// Update is called once per frame


}
