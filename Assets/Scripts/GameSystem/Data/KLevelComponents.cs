using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class KLevelComponents : MonoBehaviour
{
	public GameObject board;
	public GameObject UNIT_PLAYER;
	public List<GameObject> dicUntiEnemy;

	public static KLevelComponents me;
	public static List<GameObject> DicUntiEnemy;

	void Awake()
	{
		KLevelComponents.me = this;

		DicUntiEnemy = dicUntiEnemy;
	}

	public static GameObject getEnemy(int n)
	{
		return DicUntiEnemy[n];
	}
}
