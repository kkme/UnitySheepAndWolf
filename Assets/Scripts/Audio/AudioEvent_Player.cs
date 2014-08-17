using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioEvent_Player :MonoBehaviour
{
	void Awake()
	{
		var u = GetComponent<UnitBase>();
		u.EVENT_ATTACK += delegate { AudioManager.Play_Attack(); };
		u.EVENT_MOVED += delegate { AudioManager.Play_PlayerMoved(); };
		u.EVENT_DEAD += delegate { AudioManager.Play_DeadPlayer(); };
	}
}