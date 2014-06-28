using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class KLevelComponents : MonoBehaviour
{
	public GameObject board;

	public List<GameObject> dicUnitBasic; // just dummy 
	public List<GameObject> dicUnitPlayer; //there is going to be only one unit
	public List<GameObject> dicUnitEnemy;

	//public static KLevelComponents me;
	public static GameObject BOARD;
	public static Dictionary<KEnums.UNIT, List<GameObject>> dicUnits; 
	public static List<GameObject> DicUntiEnemy;

	void Awake()
	{
		KLevelComponents.dicUnits = new Dictionary<KEnums.UNIT, List<GameObject>>() 
		{{KEnums.UNIT.BASIC, dicUnitBasic},{KEnums.UNIT.PLAYER, dicUnitPlayer}, {KEnums.UNIT.ENEMY , dicUnitEnemy }};
		KLevelComponents.BOARD = board;
	}

	public static GameObject getEnemy(int n)
	{
		return DicUntiEnemy[n];
	}
}
