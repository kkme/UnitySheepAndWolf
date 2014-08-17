using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioEvent_Env : MonoBehaviour
{
	void Start()
	{
		var u = GetComponent<UnitBase>();
		u.EVENT_ATTACKED += delegate { AudioManager.Play_Attacked(); };
	}
}