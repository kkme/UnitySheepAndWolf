using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitEnemy_Swing : UnitEnemy
{
	public delegate int GET_PLAYER_DIS();
	GET_PLAYER_DIS helpGetPlayerDis;

	public bool isX = true;
	public Vector2[] dirs = new Vector2[2];
	//if positive 0
	//if negative 1
	int helperGetPlayerDis_X()
	{
		var dis = WorldInfo.unitPlayer.pos - pos;
		var n = (int)dis.x;
		return (n == 0) ? 0 : (1-n/Mathf.Abs(n)) / 2;
	}
	int helperGetPlayerDis_Y()
	{
		var dis = WorldInfo.unitPlayer.pos - pos;
		var n = (int)dis.y;
		return (n == 0) ? 0 : (1-n/Mathf.Abs(n)) / 2;
	}
	public override void Awake()
	{
		base.Awake();
		if (isX) helpGetPlayerDis = helperGetPlayerDis_X;
		else helpGetPlayerDis = helperGetPlayerDis_Y;
	}
	public override void KUpdate()
	{
		base.KUpdate();
		int d = helpGetPlayerDis();
		if (!moveAttack(dirs[d]))
			moveAttack(dirs[(d + 1) % 2]);
	}
}