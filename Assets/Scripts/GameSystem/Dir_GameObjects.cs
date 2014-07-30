using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Dir_GameObjects : MonoBehaviour
{
	public GameObject board,
		graphic_push,graphic_spawn, graphic_bomb;


	public List<GameObject> dicUnitPlayer,
							dicUnitEnemy,
							dicUnitEnvironment;

	//public static KLevelComponents me;
	public static GameObject BOARD,
		GRAPHIC_PUSH,GRAPHIC_SPAWN,GRAPHIC_BOMB;
	public static Dictionary<KEnums.UNIT, List<GameObject>> dicUnits; 
	public static List<GameObject> DicUntiEnemy;

	void Awake()
	{
		Debug.Log(this + "HAD BEEN AWOKEN");
		Dir_GameObjects.dicUnits = new Dictionary<KEnums.UNIT, List<GameObject>>() 
		{{KEnums.UNIT.PLAYER, dicUnitPlayer}, {KEnums.UNIT.ENEMY , dicUnitEnemy },
		{KEnums.UNIT.ENVIRONMENT,dicUnitEnvironment}
		};
		Dir_GameObjects.BOARD = board;
		Dir_GameObjects.GRAPHIC_PUSH = graphic_push;
		Dir_GameObjects.GRAPHIC_SPAWN = graphic_spawn;
		Dir_GameObjects.GRAPHIC_BOMB = graphic_bomb;
	}

	public static GameObject getEnemy(int n)
	{
		return DicUntiEnemy[n];
	}
}
