using UnityEngine;
using System.Collections.Generic;

public class KLevel{
	Vector2 size;
	//x y type count
	public List<Vector4> units = new List<Vector4>();

	public int WIDTH { get { return (int)size.x; } }
	public int HEIGHT { get { return (int)size.y; } }

	public KLevel(int w = 2, int h = 2)
	{
		size = new Vector2(w, h);
	}
	protected void addUnit(object type, object which, int x, int y)
	{
		units.Add(new Vector4((int)type,(int) which, x, y));
	}
}
