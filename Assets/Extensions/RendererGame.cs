using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class RendererGame : MonoBehaviour
{
	public GameObject PREFAB_TILE_GREEN;
	public Vector2 count;
	public List<UnitBase> units;

	Vector2 cellSize;
	void Awake()
	{
		WorldInfo.worldSize = count;
		WorldInfo.gridUnits = new EnumUnitTypes.UNIT? [(int)count.x, (int)count.y];
		//	for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++) 
		//		WorldInfo.gridCollision[i,j] = false;
	}
	void Start () {
		renderer.enabled = false;
		cellSize = transform.worldScale().divide(new Vector3(count.x, count.y, 1)).XY();
		initTiles(cellSize);
	}
	void initTiles(Vector2 cellSize)
	{
		var posFrom = transform.getPosBottomLeft();
		for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++)
			{
				var pos = posFrom + cellSize.mult(.5f+i, .5f+j).XYZ(.1f);
				var obj = Instantiate(PREFAB_TILE_GREEN, pos, Quaternion.identity) as GameObject;
				obj.transform.parent = transform;


			}
	}
	void repositionUnit(UnitBase unit)
	{
		var posNew = transform.getPosBottomLeft() + (cellSize.mult(.5f,.5f) + unit.POS.mult(cellSize)).XYZ();
		unit.transform.position = posNew;
		
	}
	// Update is called once per frame
	void Update () {
		foreach (var u in units) repositionUnit(u);
	
	}
}
