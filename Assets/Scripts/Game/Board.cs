using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class Board : UIItem
{
	public GameObject PREFAB_TILE_GREEN;
	
	Vector2 count;
	Vector2 sizeCell;

	void Awake()
	{
		renderer.enabled = false;
	}
	public void reset(int width, int height)
	{

		this.count = new Vector2(width, height);
		sizeCell = transform.worldScale().divide(new Vector3(count.x, count.y, 1)).XY();
		initTiles(sizeCell);
	}
	void initTiles(Vector2 cellSize)
	{
		var posFrom = transform.getPosBottomLeft();
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++)
			{
				var pos = posFrom + cellSize.mult(.5f+i, .5f+j).XYZ(.1f);
				var obj = Instantiate(PREFAB_TILE_GREEN, pos, Quaternion.identity) as GameObject;
				obj.transform.localScale = cellSize.XYZ(1);
				obj.transform.parent = transform;
			}
	}
	public void positionUnit(UnitBase unit)
	{
		var posNew = transform.getPosBottomLeft() + (sizeCell.mult(.5f,.5f) + unit.POS.mult(sizeCell)).XYZ();
		unit.transform.position = posNew;
	}
	// Update is called once per frame
	
}
