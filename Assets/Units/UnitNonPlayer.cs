using UnityEngine;
using System.Collections;

public class UnitNonPlayer : UnitBase
{
	// Use this for initialization
	void Start () {}
	public virtual void EVENT_UPDATE()
	{
		var dir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
		move(dir);
	}
	
	void Update () {
	
	}
}
