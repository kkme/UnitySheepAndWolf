using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AudioEvent_Door : MonoBehaviour
{
	void Start()
	{
		var u = GetComponent<UnitBase>();
		u.EVENT_DEAD += delegate { AudioManager.Play_DeadDoor(); };
	}
}
