using UnityEngine;
using System.Collections;

public class UnitNonPlayer : UnitBase
{
	void Awake()
	{
		WorldInfo.units.Add(this);
		//lets do it for now
	}
	void Start () {}
	
	
	void Update () {
	
	}
}
