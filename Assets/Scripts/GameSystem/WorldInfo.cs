using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldInfo
{

	public static int level = 22; //world id, which is level number
	public static WorldCamera camGame; 
	public static Vector2	WORLD_SIZE,
							PLAYER_GOAL, // I think this is deprecated, but let's keep it for now. Productivity > Pretty.
							PLAYER_INPUT;


	public static UnitBase door;//the final goal, game "success" sequence	
	public static UnitPlayer unitPlayer_real;//static public UnitPlayer unitPlayer;
	public static UnitBase[,] gridUnits;

	public static List<UnitBase>	unitsUpdate01,
									unitsUpdate00,
									unitsStatic,
									unitsPlayers,
									//group of units for animation phase 00, right before player input 
									unitsAnimation00;

	public static UnitBase getClosestPlayerUnit(Vector2 pos,int dirFacing )
	{
		int disShortest = 99999;
		UnitBase unit = null;
		Vector2 dis;
		foreach (var u in unitsPlayers)
		{
			dis = u.pos - pos;
			var mag = u.findPathToUnit((int)pos.x, (int)pos.y);// Mathf.Abs(dis.x) + Mathf.Abs(dis.y);
			if (mag != 0 && dirFacing != -1)
			{
				if ((dirFacing == 0 || dirFacing == 2) && dis.x != 0) mag++;
				if ((dirFacing == 1 || dirFacing == 3) && dis.y != 0) mag++;
			}
			if (mag < disShortest)
			{
				//Debug.Log("Chosen "+u);
				disShortest = (int)mag;
				unit = u;
			}
		}
		//Debug.Log("Chosen Final" + unit + " SCORE : "  + disShortest);
		return unit;
	}
	public static UnitBase getClosestPlayerUnit(int x, int y,int dirFacing)
	{
		return getClosestPlayerUnit(new Vector2(x,y),dirFacing );
	}
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
	static public List<SimpleJSON.JSONNode> toData()
	{
		var units = new List<SimpleJSON.JSONNode>(); 
		for (int i = 1; i < 13-1; i++)
			for (int j = 1; j < 13-1; j++)
			{
				var unit = gridUnits[i, j];
				if (unit == null) continue;
				//Debug.Log(unit + " AND " + (DataUnit)unit );
				units.Add((SimpleJSON.JSONNode)unit);
			}
		if (door != null) units.Add((SimpleJSON.JSONNode)door);
		return units;
	}

}