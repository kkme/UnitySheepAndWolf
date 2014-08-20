using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioEvent_Enemy : MonoBehaviour
{
	public virtual void Start()
	{
		var u = GetComponent<UnitBase>();
		u.EVENT_ATTACK_PUSH += delegate { AudioManager.Play_AttackPush(); };
		u.EVENT_DEAD += delegate { AudioManager.Play_DeadEnemy(); };
	}
}
