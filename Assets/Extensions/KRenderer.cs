using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class KRenderer : MonoBehaviour
{
	public Vector2 count;
	public List<UnitBase> units;

	Vector2 cellSize;
	void Awake()
	{
		WorldInfo.worldSize = count;
		WorldInfo.gridCollision = new bool[(int)count.x, (int)count.y];
		//for (int i = 0; i < count.x; i++) for (int j = 0; j < count.y; j++) 
		//	WorldInfo.gridCollision[i,j] = false;
	}
	void Start () {
		renderer.enabled = false;
		cellSize = transform.worldScale().divide(new Vector3(count.x, count.y, 1)).XY();
	
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
