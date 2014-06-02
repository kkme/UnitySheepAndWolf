using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldInfo
{

	static public Vector2 worldSize;
	static public EnumUnitTypes.UNIT? [,] gridUnits;

	static public List<UnitBase> units = new List<UnitBase>();
	static public UnitPlayer unitPlayer;
}