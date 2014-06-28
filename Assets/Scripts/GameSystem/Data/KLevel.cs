using UnityEngine;
using System.Collections.Generic;

public class KLevel{
	public int[] size;
	public List<KLevel_Unit> units = new List<KLevel_Unit>();
	
	public int WIDTH { get { return (int)size[0]; } }
	public int HEIGHT { get { return (int)size[1]; } }

	public KLevel(int w = 2, int h = 2)
	{
		size = new int[] { w, h };
	}
	protected void addUnit(object type, object which, int x, int y)
	{
		//units.Add(new Vector4((int)type,(int) which, x, y));
	}

	public static System.Type [] LEVELS = new System.Type[] {typeof(Level00) };
}
