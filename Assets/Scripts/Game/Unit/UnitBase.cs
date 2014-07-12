using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	public KEnums.UNIT TYPE { get { return myType; } }
	static bool IsDebug = false;

	public bool 
		isUpdated = false,
		isPushable = false,
		isAttackable = false,
		isMoved = false;
	public bool
		isAlive = true;
	protected int health = 0;
	public int dirFacing = 0; 
	protected KEnums.UNIT myType = KEnums.UNIT.BASIC;
	public Vector2	pos = new Vector2(0, 0), 
						posBefore = new Vector2(0, 0);


	public virtual void init() { }
	public virtual void Awake(){}
	public virtual void Start()
	{
		registerOnGrid();
	}
	//helper methods 
	protected bool isIndexValid(int x, int y)
	{
		return !(x < 0 || y < 0 ||
			x >= WorldInfo.WORLD_SIZE.x || y >= WorldInfo.WORLD_SIZE.y);
	}
	protected bool? helperIsGridAvailable<T>(T[,] grid, int x, int y)
	{
		if (!isIndexValid(x, y)) return null;
		return grid[x, y] == null;
	}

	//methods
	public virtual UnitBase[,] helperGetGrid(){return WorldInfo.gridUnits;}
	public void registerOnGrid(){helperGetGrid()	[(int)pos.x, (int)pos.y] = this;}
	public void unRegisterOnGrid(){helperGetGrid()[(int)pos.x, (int)pos.y] = null;}
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
	public bool move(Vector2 dir, bool moveTry = true) {return move((int)(pos.x + dir.x), (int)(pos.y + dir.y), moveTry);}
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
			isMoved = resultTry;
			return resultTry;
		}
		if (IsDebug) Debug.Log(myType + " " + "AVAILALBE");
		helperSetPosition(x, y);
		isMoved = true;
		return true;
	}
	public bool moveAttack(Vector2 dir)
	{
		return moveAttack((int)(pos.x + dir.x), (int)(pos.y + dir.y));
	}
	public virtual bool moveAttack(int x, int y)
	{
		var u = helperGetGrid()[x, y];
		if (u == null) {return move(x, y);}
		u.attacked();
		move(x, y);
		return true;
	}
	protected void helperSetPosition(int x, int y)
	{
		unRegisterOnGrid();
		posBefore = pos;
		pos = new Vector2(x, y);
		registerOnGrid();
	}
	public bool push(int dirX, int dirY )//direction to pushed
	{
		return false;
	}
	public virtual bool attacked()
	{
		if (!isAttackable) return false;
		kill();
		return true;
	}
	public virtual void kill()
	{
		unRegisterOnGrid();
		GameObject.Destroy(gameObject);
		isAlive = false;
	}
	public virtual void KUpdate()
	{
		isUpdated = true;
	}
}