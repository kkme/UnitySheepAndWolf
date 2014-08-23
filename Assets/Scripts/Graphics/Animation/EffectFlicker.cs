using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectFlicker : MonoBehaviour
{
	float timeElapsed = 0, timeElapsedMax = .3f;
	public SpriteRenderer renderer;
	public void init(float duration = .3f)
	{
		gameObject.SetActive(true);
		timeElapsed = 0;
		timeElapsedMax = duration;
		renderer.enabled = true;
		enabled = true;
	}
	public void off()
	{
		renderer.enabled = false;
		enabled = false;
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		
		if (timeElapsed >= timeElapsedMax)
		{
			timeElapsed = 0;
			renderer.enabled = !renderer.enabled ;
		}
	}
}
