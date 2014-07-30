using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	public enum ATTACK_TYPE { NONE, KILL,PUSH };
	public delegate bool DEL_ATTACK(UnitBase u, int dirX, int dirY);
	public DEL_ATTACK attack = delegate { return false; };
	static bool attackNone(UnitBase u, int x, int y) { return false; }
	static bool attackPush(UnitBase u, int x, int y) { return u.push(x, y); }
	static bool attackKill(UnitBase u, int x, int y) { return u.attacked(); }
	static Dictionary<ATTACK_TYPE, DEL_ATTACK> dirAttacks = new Dictionary<ATTACK_TYPE, DEL_ATTACK>()
	{
		{ATTACK_TYPE.NONE,attackNone },
		{ATTACK_TYPE.KILL,attackKill },
		{ATTACK_TYPE.PUSH,attackPush }
	};

	public static KDels.EVENTHDR_REQUEST_SIMPLE_POS EVENT_EXPLOSION = delegate { };
	public KEnums.UNIT TYPE { get { return typeMe; } }

	

	static bool IsDebug = false;

	internal protected ATTACK_TYPE typeAttack = ATTACK_TYPE.KILL;

	internal protected bool
		//unit state
		isUpdated = false,
		isMoved = false,
		//unit property 
		isDestroyable = false,
		isBomb = false;

	internal protected bool
		isAlive = true;
	public int	dirFacing = 0,
				health=1,
				turnCount = 0; 
	protected KEnums.UNIT typeMe = KEnums.UNIT.BASIC;
	internal protected Vector2	pos = new Vector2(0, 0), 
						posBefore = new Vector2(0, 0);

	//get set
	public void setAttackType(ATTACK_TYPE type)
	{
		typeAttack = type;
		attack = dirAttacks[type];
	}

	public virtual void init()
	{
		registerOnGrid();
		setAttackType(typeAttack);

		
	}
	public virtual void Awake(){}
	public virtual void Start()
	{
		
		
	}
	//get set
	

	//helper methods 
	protected bool isIndexValid(int x, int y)
	{
		return !(x < 0 || y < 0 ||
			x >= WorldInfo.WORLD_SIZE.x || y >= WorldInfo.WORLD_SIZE.y);
	}
	protected bool? helperIsGridAvailable(UnitBase[,] grid, int x, int y)
	{

		if (!isIndexValid(x, y)) return null;
		return (grid[x, y] == null);
	}

	protected void helperSetPosition(int x, int y)
	{
		unRegisterOnGrid();
		posBefore = pos;
		pos = new Vector2(x, y);
		registerOnGrid();
	}
	public virtual UnitBase[,] helperGetGrid() { return WorldInfo.gridUnits; }
	public int helperToDir(Vector2 v) { return helperToDir((int)v.x, (int)v.y); }
	public int helperToDir(int h, int v)
	{
		if (h == 1) return 1;
		if (v == -1) return 2;
		if (h == -1) return 3;
		return 0;//(v == 1)
	}
	protected int helperGetUnit(int n)
	{
		return (n == 0) ? 0 : n / Mathf.Abs(n);
	}
	public virtual bool helperIsValidAttackTarget(KEnums.UNIT type)
	{
		return true;
	}

	//methods
	
	protected void face(Vector2 dir)
	{
		face((int)dir.x, (int)dir.y);
	}
	protected void face(int h, int v){
		face(helperToDir(h,v));
	}
	protected void face(int d)
	{
		isMoved = true;
		dirFacing = d;
	}
	void moveBack()
	{
		if (IsDebug) Debug.Log(typeMe + "Moving Back	");
		unRegisterOnGrid();
		pos = posBefore;
		registerOnGrid();
	}
	bool moveTry(int x, int y){
		if (IsDebug) Debug.Log(typeMe + " Move trying");
		var at = helperGetGrid()[x,y] as UnitBase;
		if (at == null) return move(x, y,true);
		if (at.isUpdated) return false;

		at.KUpdate();
		if (helperGetGrid()[x,y] != null) return false;
		return move(x, y,true);
	}

	public void registerOnGrid(){helperGetGrid()	[(int)pos.x, (int)pos.y] = this;}
	public void unRegisterOnGrid(){helperGetGrid()[(int)pos.x, (int)pos.y] = null;}
	public bool move(Vector2 dir, bool moveTry = true) {return move((int)(pos.x + dir.x), (int)(pos.y + dir.y), moveTry);}
	public virtual bool move(int x, int y, bool tryAgain = true)
	{
		var isAvailable = helperIsGridAvailable(helperGetGrid(), x, y);
		if (isAvailable == null) return false; //returned null; error.
		if (!isAvailable.Value) //grid is currently being occupied
		{
			if (!tryAgain) return false;
			if (IsDebug) Debug.Log(typeMe + " " + "NOT AVAILALBE ");
			bool resultTry = moveTry(x,y);
			if (IsDebug) Debug.Log(typeMe + " MOVE TRYING RESULT : " + resultTry);
			isMoved = resultTry;
			return resultTry;
		}
		if (IsDebug) Debug.Log(typeMe + " " + "AVAILALBE");
		helperSetPosition(x, y);
		isMoved = true;
		return true;
	}
	public bool moveAttack(Vector2 dir, bool willMove = true) { return moveAttack((int)(pos.x + dir.x), (int)(pos.y + dir.y), willMove); }
	public virtual bool moveAttack(int x, int y, bool willMove = true)
	{
		if (!isIndexValid(x, y) || (x == (int)pos.x && y ==(int)pos.y)) return false;
		var u = helperGetGrid()[x, y];
		if (u == null) return move(x, y);
		if (helperIsValidAttackTarget(u.typeMe))
		{
			if (!attack(u, helperGetUnit((int)(x - pos.x)), helperGetUnit((int)(y - pos.y))))
				return false;
			if (willMove) return move(x, y);
			return true;
		}
		return false;
	}

	public bool moveAttackRotation(Vector2 direction)
	{
		int dir = helperToDir(direction);
		if (dirFacing != dir)
		{
			face(dir);
			return true;
		}
		return moveAttack(direction);
	}
	public virtual bool push(int dirX, int dirY)//direction to pushed
	{
		if(!move((int)(pos.x + dirX), (int)(pos.y + dirY), false)){
			Debug.Log("I failed to get pushed " + dirX + " " + dirY);
			attacked();
		}
		return true;
	}
	public virtual bool attacked()
	{
		if (!isDestroyable) return false;
		if (--health <= 0)
		{
			kill();
			return true;
		}
		return false;
	}
	private void kill()
	{
		unRegisterOnGrid();
		GameObject.Destroy(gameObject);
		isAlive = false;
		if (isBomb) explode();
	}
	void helperExplode(int x, int y)
	{
		if(!isIndexValid(x, y)) return;
		var unit = WorldInfo.gridUnits[x, y];
		if (unit != null) unit.attacked();
		else EVENT_EXPLOSION(x, y);
	}
	public virtual void explode()
	{
		helperExplode((int)pos.x,(int) pos.y);
		helperExplode((int)(pos.x - 1), (int)(pos.y		));
		helperExplode((int)(pos.x + 1), (int)(pos.y		));
		helperExplode((int)(pos.x	 ), (int)(pos.y + 1	));
		helperExplode((int)(pos.x	 ), (int)(pos.y - 1	));
	}
	public void turn()
	{
		if (isUpdated || !isAlive) return;
		KUpdate(); return;
	}

	public virtual void KUpdate()
	{
		isUpdated = true;
	}

}