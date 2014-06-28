using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldInfo
{

	static public Vector2 WORLD_SIZE;
	public static Vector2 PLAYER_GOAL;

	static public UnitBase[,]		gridUnits;
	static public HazardBase[,]		gridHazard;
	static public UnitObj[,]	gridObjectives;


	static public List<UnitBase> units;
	static public UnitPlayer unitPlayer;

	static public void init(int width, int height)
	{
		WORLD_SIZE = new Vector2(width, height);
		PLAYER_GOAL = new Vector2(0, 0);
		units = new List<UnitBase>();
		unitPlayer = null;

		gridUnits = new UnitBase[width, height];
		gridHazard = new HazardBase[width, height];
		gridObjectives = new UnitObj[width, height];
	}

}