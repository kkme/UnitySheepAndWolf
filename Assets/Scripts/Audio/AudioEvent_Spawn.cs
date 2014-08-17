using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioEvent_Spawn :MonoBehaviour
{
	void Start()
	{
		var u = GetComponent<UnitEnemy_Spawn>();
		u.EVENT_SPAWN += delegate { AudioManager.Play_Spawn(); };
	}
}
