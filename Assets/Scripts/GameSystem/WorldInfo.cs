﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldInfo
{


	static public Vector2	WORLD_SIZE,
							PLAYER_GOAL, // I think this is deprecated, but let's keep it for now. Productivity > Pretty.
							PLAYER_INPUT;

	static public UnitBase[,]		gridUnits;


	static public List<UnitBase>	unitsUpdate01,
									unitsUpdate00,
									unitsStatic,
									unitsPlayers,
									//group of units for animation phase 00, right before player input 
									unitsAnimation00;

	public static UnitBase getClosestPlayerUnit(Vector2 pos)
	{
		int disShortest = 99999;
		UnitBase unit = null;
		Vector2 dis;
		foreach (var u in unitsPlayers)
		{
			dis = u.pos - pos;
			var mag = Mathf.Abs(dis.x) + Mathf.Abs(dis.y);
			if (mag < disShortest)
			{
				disShortest = (int)mag;
				unit = u;
			}
		}
		return unit;
	}
	static public UnitPlayer unitPlayer_real;
	//static public UnitPlayer unitPlayer;
	static void helperDestroyAllGameObjects(List<UnitBase> l)
	{
		foreach (var u in l) GameObject.Destroy(u);
	}
	static public void init(int width, int height)
	{
		WORLD_SIZE = new Vector2(width, height);
		PLAYER_GOAL = new Vector2(0, 0);

		unitsUpdate01 = new List<UnitBase>();
		unitsUpdate00 = new List<UnitBase>();
		unitsStatic = new List<UnitBase>();
		unitsPlayers = new List<UnitBase>();
		unitsAnimation00 = new List<UnitBase>();
		unitPlayer_real = null;

		gridUnits = new UnitBase[width, height];
	}
	static public void initEveryTurn()
	{
		unitsAnimation00 = new List<UnitBase>();

	}

}