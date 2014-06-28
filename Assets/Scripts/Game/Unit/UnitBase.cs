using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	static bool IsDebug = false;
	public Vector2	pos = new Vector2(0, 0), 
					posBefore = new Vector2(0, 0);
	public Vector2 POS { get { return pos; } }
	
	
	bool isUpdated = false;
	protected KEnums.UNIT myType = KEnums.UNIT.BASIC;
	public KEnums.UNIT TYPE { get { return myType;  } }
	public bool IsUpdated { get { return isUpdated; } set { isUpdated = value; } }

	public virtual void  Awake()
	{}
	public virtual void  Start()
	{
		registerOnGrid();
	}
	protected bool helperIsIndexValid(int x, int y)
	{
		return !(x < 0 || y < 0 ||
			x >= WorldInfo.WORLD_SIZE.x || y >= WorldInfo.WORLD_SIZE.y);
	}
	protected bool? helperIsGridAvailable<T>(T[,] grid, int x, int y)
	{
		if (!helperIsIndexValid(x, y)) return null;

		return grid[x, y] == null;
	}

	public virtual System.Object[,] helperGetGrid(){
		return WorldInfo.gridUnits;
	}
	public void registerOnGrid()
	{
		helperGetGrid()	[(int)pos.x, (int)pos.y] = this;
	}
	public void unRegisterOnGrid()
	{
		helperGetGrid()[(int)pos.x, (int)pos.y] = null;
	}
	public bool move(Vector2 dir, bool moveTry = true)
	{
		return move((int)(pos.x + dir.x), (int)(pos.y + dir.y), moveTry);
	}
	public void move(float x, float y) { move((int)x, (int)y, true); }
	public void moveBack()
	{
		if(IsDebug)Debug.Log(myType + "Moving Back	"); 
		unRegisterOnGrid();
		pos = posBefore;
		registerOnGrid();
	}
	bool moveTry(int x, int y){
		if (IsDebug) Debug.Log(myType + " Move trying");
		var at = helperGetGrid()[x,y] as UnitBase;
		if (at == null) return move(x, y,true);
		if (at.isUpdated) return false;

		at.KUpdate();
		if (helperGetGrid()[x,y] != null) return false;
		return move(x, y,true);
	}
	public virtual bool move(int x, int y, bool tryAgain = true)
	{
		var isAvailable = helperIsGridAvailable(helperGetGrid(), x, y);
		if(isAvailable == null) return false;
		if (!isAvailable.Value)
		{
			if (!tryAgain) return false;
			if (IsDebug) Debug.Log(myType + " " + "NOT AVAILALBE ");
			bool resultTry = moveTry(x,y);
			if (IsDebug) Debug.Log(myType + " MOVE TRYING RESULT : " + resultTry);
			return resultTry;
		}
		if (IsDebug) Debug.Log(myType + " " + "AVAILALBE");
		unRegisterOnGrid();
		posBefore = pos;
		pos = new Vector2(x,y);
		registerOnGrid();
		return true;
	}
	public void kill()
	{
		unRegisterOnGrid();
		GameObject.Destroy(gameObject);
	}
	public virtual void KUpdate()
	{
		isUpdated = true;
	}
	public virtual void KUpdateEnd()
	{

	}
}