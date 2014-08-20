using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioEvent_Spawn :AudioEvent_Enemy
{
	public override void Start()
	{
		base.Start();
		var u = GetComponent<UnitEnemy_Spawn>();
		u.EVENT_SPAWN += delegate { AudioManager.Play_Spawn(); };
	}
}
