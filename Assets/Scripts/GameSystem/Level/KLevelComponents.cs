using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class KLevelComponents : MonoBehaviour
{
	public GameObject board;

	public List<GameObject> dicUnitPlayer, //there is going to be only one unit
							dicUnitEnemy,
							dicUnitEnvironment;

	//public static KLevelComponents me;
	public static GameObject BOARD;
	public static Dictionary<KEnums.UNIT, List<GameObject>> dicUnits; 
	public static List<GameObject> DicUntiEnemy;

	void Awake()
	{
		KLevelComponents.dicUnits = new Dictionary<KEnums.UNIT, List<GameObject>>() 
		{{KEnums.UNIT.PLAYER, dicUnitPlayer}, {KEnums.UNIT.ENEMY , dicUnitEnemy },
		{KEnums.UNIT.ENVIRONMENT,dicUnitEnvironment}
		};
		KLevelComponents.BOARD = board;
	}

	public static GameObject getEnemy(int n)
	{
		return DicUntiEnemy[n];
	}
}
