using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectManager : MonoBehaviour
{
	public GameObject EFFECT_EXPLOSION;
	void Awake()
	{
		UnitBase.EVENT_EXPLOSION += explosion;
	}
	void explosion(int x, int y)
	{
		Instantiate(EFFECT_EXPLOSION, new Vector3(x, y, 0), Quaternion.identity);
	}
}
