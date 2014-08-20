using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectBase : MonoBehaviour
{
	float timeElapsed=0, timeElapsedMax = 1.0f;
	AniMover mover;
	Rigidbody2D body;
	void Awake()
	{
		body = gameObject.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		gameObject.SetActive(false);
		EffectManager.effectsBase.Add(this);
	}
	public void setPosition(float x, float y)
	{
		var p =this.transform.parent;
		transform.parent = null;
		transform.position = new Vector3(x, y, 0);
		transform.parent = p;
	}
	public void move(float x, float y,float duration = 3.0f)
	{
		transform.localScale = new Vector3(.5f, .5f,1);
		gameObject.SetActive(true);
		enabled = true;

		timeElapsed = 0;
		timeElapsedMax = duration;
		body.velocity = new Vector2(x, y);
		
		
		//posTo = new Vector3(x, y, z);
		//timeElapsedMax = duration;
		//enabled = true;
	}
	void Update()
	{
		float ratio = 1- .85f* Time.deltaTime;
		transform.localScale *= ratio;
		timeElapsed += Time.deltaTime;
		if (timeElapsed > timeElapsedMax) {
			gameObject.SetActive(false);
			enabled = false;
		}

	}
}
