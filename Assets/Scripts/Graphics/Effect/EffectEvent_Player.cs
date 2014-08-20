using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectEvent_Player : MonoBehaviour
{
	UnitBase unit;
	public virtual void Start()
	{
		unit = GetComponent<UnitBase>();
		unit.EVENT_DEAD += delegate(int dirX, int dirY)
		{
			EffectManager.ExplosionBlue((int)unit.pos.x, (int)unit.pos.y, 0, dirX, dirY);
			EffectManager.ExplosionBlue((int)unit.pos.x, (int)unit.pos.y, 0, dirX, dirY);
			EffectManager.ExplosionBlue((int)unit.pos.x, (int)unit.pos.y, 0, dirX, dirY);
			EffectManager.CameraDistortion(dirX*0.3f, dirY*.3f);
		};
	}
}
