using UnityEngine;
using System.Collections.Generic;

using ExtensionsUnityVectors;

public class UnitEnemy : UnitUpdated
{
	static public event KDels.EVENTHDR_REQUEST_SIMPLE EVENT_HIT_PLAYER = delegate { };

	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.ENEMY;
		}
	}
	public override bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return type == KEnums.UNIT.PLAYER;
	}
	public override void Awake()
	{
		base.Awake();
		isDestroyable_simpleAttack = true;
	}
	bool isPlayerNextTo(Vector2 pos)
	{
		Vector2[] dics = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
		for (int i = 0; i < 4; i++)
		{

			var at = pos + dics[i];
			if (!isIndexValid((int)at.x, (int)at.y)) continue;
			if (isPlayerAt((int)at.x, (int)at.y)) return true;

		}
		return false;
	}
	protected bool isPlayerAt(int x, int y)
	{
		if (!isIndexValid(x, y)) return false;
		var g = helperGetGrid()[x, y] as UnitBase;
		return (g != null && g.typeMe == KEnums.UNIT.PLAYER);
	}

	protected bool isPlayerAt(Vector2 v)
	{
		return isPlayerAt((int)v.x, (int)v.y);
	}
	public bool isPlayerClose(int rangeW, int rangeH)
	{
		var diff = pos - WorldInfo.getClosestPlayerUnit(pos,-1).pos;
		return Mathf.Abs(diff.x) <= rangeW && Mathf.Abs(diff.y) <= rangeH;
	}
	public override void KUpdate()
	{
		isUpdated = true;
	}
	
}
