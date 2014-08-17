using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AudioEvent_Enemy : MonoBehaviour
{
	void Start()
	{
		var u = GetComponent<UnitBase>();
		u.EVENT_ATTACK_PUSH += delegate { AudioManager.Play_AttackPush(); };
		u.EVENT_DEAD += delegate { AudioManager.Play_DeadEnemy(); };
	}
}
