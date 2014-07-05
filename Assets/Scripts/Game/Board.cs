using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class Board : UIItem
{

	public GameObject
			PREFAB_TILE_CORNER,
			PREFAB_TILE_GREEN;
	
	Vector2 count;
	Vector2 sizeCell;

	void Awake()
	{
		renderer.enabled = false;
	}
	void helperInstantiate(GameObject PREFAB, int x, int y)
	{
		var posFrom = transform.getPosBottomLeft();
		var pos = posFrom + sizeCell.mult(.5f + x, .5f + y).XYZ(.1f);
		var obj = Instantiate(PREFAB, pos, Quaternion.identity) as GameObject;
		obj.transform.localScale = sizeCell.XYZ(1);
		obj.transform.parent = transform;
	}
	void initTiles()
	{
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++)
				helperInstantiate(PREFAB_TILE_GREEN, i, j);

		for (int x = 1; x < count.x-1; x++)
		{
			helperInstantiate(PREFAB_TILE_CORNER, x, 0);
			helperInstantiate(PREFAB_TILE_CORNER, x, (int)(count.y - 1));

		}
		for (int y = 0; y < count.y; y++)
		{
			helperInstantiate(PREFAB_TILE_CORNER, 0, y);
			helperInstantiate(PREFAB_TILE_CORNER, (int)(count.x - 1), y);
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
		var posNew = transform.getPosBottomLeft() + (sizeCell.mult(.5f,.5f) + unit.POS.mult(sizeCell)).XYZ();
		unit.transform.position = posNew;
	}
	// Update is called once per frame
	
}
