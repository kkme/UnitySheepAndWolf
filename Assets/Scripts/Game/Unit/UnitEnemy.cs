using UnityEngine;
using System.Collections;

using ExtensionsUnityVectors;

public class UnitEnemy : UnitNonPlayer {
	public delegate void EVENTHDR_HIT_PLAYER();
	static public event EVENTHDR_HIT_PLAYER EVENT_HIT_PLAYER = delegate { };

	bool isPlayerNextTo(Vector2 pos)
	{
		Vector2[] dics =new Vector2[]{new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1), new Vector2(0,-1)};
		for (int i = 0; i < 4; i++)
		{
			
			var at = pos + dics[i];
			if (!helperIsIndexValid((int)at.x, (int)at.y)) continue;
			if ( isPlayerAt((int)at.x, (int)at.y ) ) return true;

		}
		return false;
	}
	bool isPlayerAt(int x, int y)
	{
		var g = helperGetGrid()[x,y] as UnitBase;
		return (g != null && g.TYPE == KEnums.UNIT.PLAYER);
	}
	public bool isPlayerClose(int rangeW,int rangeH)
	{
		var diff = pos - WorldInfo.unitPlayer.POS;
		return Mathf.Abs(diff.x) <= rangeW && Mathf.Abs(diff.y) <= rangeH;
	}
	Vector2 helperGetDir(UnitBase unit)
	{
		var dir = (unit.POS - pos).dir();
		int index = Random.Range(1, 2);
		for (int i = 0; i < 2; i++)
		{
			if (dir[index] != 0) { index = (index + 1) % 2; dir[index] = 0; }
			else index = (index + 1) % 2;
		}
		return dir;
	}

	public override void KUpdate(){
		IsUpdated = true;
		if (isPlayerClose(5, 5)){
			var dir = helperGetDir(WorldInfo.unitPlayer);
			move(dir);
		}
	}
	public override bool move(int x, int y, bool tryAgain = true)
	{
		if (!base.move(x, y, true))
		{
			if (isPlayerNextTo(pos)) EVENT_HIT_PLAYER();
		}
		return true;
	}
}
