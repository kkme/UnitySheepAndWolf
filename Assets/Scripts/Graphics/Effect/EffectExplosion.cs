using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectExplosion :MonoBehaviour
{
	enum STATE { Expanding, Stable, Shrinking };
	STATE stateMe = STATE.Expanding;
	float rate = 3.0f, expandMax = .5f, expanded = 0;
	void Update()
	{
		switch (stateMe)
		{
			case STATE.Expanding:
				expanded += rate * Time.deltaTime;
				transform.localScale = new Vector3(expanded, expanded, expanded);
				if (expanded >= expandMax) stateMe = STATE.Shrinking;
				break;
			case STATE.Stable:
				break;
			case STATE.Shrinking:
				expanded -= rate * Time.deltaTime;
				transform.localScale = new Vector3(expanded, expanded, expanded);
				if (expanded <= .01f) Destroy(this.gameObject);
				break;

		}
	}
}
