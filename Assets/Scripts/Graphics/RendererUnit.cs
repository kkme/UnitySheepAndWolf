using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***
 * This class assume that the gameobject this script is attached to has
 * UnitBase, or anyscript that extends UnitBase is attached to 
 * **/
public class RendererUnit : MonoBehaviour
{
	public bool woke = false;
	UnitBase unit;
	GameObject objBomb = null, objPush = null;
	GameObject helperInstntiate(GameObject o)
	{

		var obj = (GameObject) Instantiate(o, transform.position, Quaternion.identity);
		obj.transform.parent = transform;
		return obj;
	}
	void Start()
	{
		if (woke) return;
		woke = true;
		//test first
		var unit = (UnitBase)GetComponent<UnitBase>();
		if (unit.isBomb) objBomb = helperInstntiate(Dir_GameObjects.GRAPHIC_BOMB);
		if (unit.typeAttack == UnitBase.ATTACK_TYPE.PUSH ) objPush =helperInstntiate(Dir_GameObjects.GRAPHIC_PUSH);
		
		//;
		//Dir_GameObjects.GRAPHIC_PUSH;
		//Dir_GameObjects.GRAPHIC_SPAWN;
	}
	public void refresh()
	{
		if (objBomb != null) Destroy(objBomb);
		if (objPush != null) Destroy(objPush);

		if (unit.isBomb) objBomb = helperInstntiate(Dir_GameObjects.GRAPHIC_BOMB);
		if (unit.typeAttack == UnitBase.ATTACK_TYPE.PUSH) objPush = helperInstntiate(Dir_GameObjects.GRAPHIC_PUSH);
	}
	
}
