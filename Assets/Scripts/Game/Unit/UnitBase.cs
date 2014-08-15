using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class UnitBase : MonoBehaviour
{
	public static bool isSpawn(KEnums.UNIT tpyeUnit, int id)
	{
		return tpyeUnit == KEnums.UNIT.ENEMY && id == 10;
	}
	public static bool isTrap(KEnums.UNIT tpyeUnit, int id)
	{
		return tpyeUnit == KEnums.UNIT.ENEMY && (id >= 5 && id <= 9);
	}
	public enum TYPE_ATTACK { NONE, KILL,PUSH };
	public delegate bool DEL_ATTACK(UnitBase u, int dirX, int dirY);
	public DEL_ATTACK attack = delegate { return false; };
	//cast 
	static public explicit operator UnitBase(DataUnit data)
	{
		var obj = (Instantiate(Dir_GameObjects.dicUnits[data.typeUnit][data.id]) as GameObject).GetComponent<UnitBase>();
		obj.pos = new Vector2(data.x,data.y);
		obj.isBomb = data.isBomb;
		obj.isDestroyable_bomb = data.isDestroyable_bomb;
		obj.isDestroyable_simpleAttack = data.isDestroyable_simpleAttack;
		obj.isSwappable = data.isSwappable;
		//var spawn = obj.GetComponent<UnitEnemy_Spawn>();
		var spawn = obj.gameObject.GetComponent<UnitEnemy_Spawn>();
		if (spawn != null)
		{
			var dataSpawn = data.other;
			spawn.setSpawnEnemy(Dir_GameObjects.dicUnits[dataSpawn.typeUnit][dataSpawn.id], dataSpawn.dirFacing, dataSpawn.isBomb, dataSpawn.typeAttack, dataSpawn.isDestroyable_simpleAttack, dataSpawn.isDestroyable_bomb, dataSpawn.id >= 5);
		}
		return obj;
	}
	static public explicit operator UnitBase(SimpleJSON.JSONNode node)
	{
		var obj = (Instantiate(Dir_GameObjects.dicUnits[(KEnums.UNIT)node["typeUnit"].AsInt][node["id"].AsInt]) as GameObject).GetComponent<UnitBase>();
		obj.pos = new Vector2(node["x"].AsInt, node["y"].AsInt);
		obj.dirFacing =						node["dirFacing"].AsInt;
		obj.isBomb =						node["isBomb"].AsBool;
		obj.isDestroyable_bomb =			node["isDestroyable_bomb"].AsBool;
		obj.isDestroyable_simpleAttack =	node["isDestroyable_simpleAttack"].AsBool;
		obj.isSwappable =					node["isSwappable"].AsBool;
		obj.typeAttack = (TYPE_ATTACK)node["typeAttack"].AsInt;
		////var spawn = obj.GetComponent<UnitEnemy_Spawn>();
		var spawn = obj.gameObject.GetComponent<UnitEnemy_Spawn>();
		if (spawn != null)
		{
			var dataSpawn = node["other"].AsObject;
			spawn.setSpawnEnemy(Dir_GameObjects.dicUnits[(KEnums.UNIT) dataSpawn["typeUnit"].AsInt][dataSpawn["id"].AsInt], 
				dataSpawn["dirFacing"].AsInt,dataSpawn["isBomb"].AsBool,(TYPE_ATTACK) dataSpawn["typeAttack"].AsInt, 
				dataSpawn["isDestroyable_simpleAttack"].AsBool, dataSpawn["isDestroyable_bomb"].AsBool,
				isTrap((KEnums.UNIT)dataSpawn["typeUnit"].AsInt, dataSpawn["id"].AsInt));
		}
		return obj;
	}
	static bool attackNone(UnitBase u, int x, int y) { return false; }
	static bool attackPush(UnitBase u, int x, int y) { 
		var result = u.push(x,y);
		u.isPushed = u.isPushed || result;
		return result;  
	}
	static bool attackKill(UnitBase u, int x, int y) { return u.attacked(x,y); }
	static Dictionary<TYPE_ATTACK, DEL_ATTACK> dirAttacks = new Dictionary<TYPE_ATTACK, DEL_ATTACK>()
	{
		{TYPE_ATTACK.NONE,attackNone },
		{TYPE_ATTACK.KILL,attackKill },
		{TYPE_ATTACK.PUSH,attackPush }
	};

	public static KDels.EVENTHDR_REQUEST_SIMPLE_POS EVENT_EXPLOSION = delegate { };


	

	static bool IsDebug = false;

	internal protected TYPE_ATTACK typeAttack = TYPE_ATTACK.KILL;

	internal protected bool
		//unit state
		isUpdated = false,
		isMoved = false,
		isPushed = false,

		//unit property 
		isBomb = false,			//Will have "explosion" when the unit dies
		isSwappable = false,	// environment units count as "obstacles" when it comes to "mapping phase"
		isDestroyable_simpleAttack = true,	//Can be destroyed by "attacks" || "getting squeezed"
		isDestroyable_bomb = true;

	internal protected bool
		isAlive = true;
	public int 
				id,
				dirFacing = 0,
				health = 1;
	internal protected Vector2	pos = new Vector2(0, 0), 
						posBefore = new Vector2(0, 0);

	//get set
	public virtual KEnums.UNIT typeMe { get { return KEnums.UNIT.BASIC; } }

	public void setAttackType(TYPE_ATTACK type)
	{
		typeAttack = type;
		attack = dirAttacks[type];
	}
	//functions

	public virtual UnitBase init()
	{
		registerOnGrid();
		setAttackType(typeAttack);
		//transform.position = new Vector3(pos.x, pos.y, 0);
		return this;
	}
	internal UnitBase initPos(){
		transform.position = new Vector3(pos.x, pos.y, 0);
		return this;
	}
	public virtual UnitBase initPos(int x, int y){
		
		pos = new Vector2(x, y);
		transform.position = new Vector3(x, y, 0);
		return this;
	}
	public virtual void reset()
	{
		isUpdated = false;
		isMoved = false;
		isPushed = false;
	}
	public virtual void Awake(){}
	public virtual void Start() { }
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
	internal UnitBase[,] helperGetGrid() { return WorldInfo.gridUnits; }
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
	bool moveTry(int x, int y){
		if (IsDebug) Debug.Log(typeMe + " Move trying");
		var at = helperGetGrid()[x,y] as UnitBase;
		if (at.isUpdated) return false;
		int posX = (int)pos.x, posY = (int)pos.y;
		at.KUpdate();
		if ((int)pos.x != posX || (int)pos.y != posY)	return false;
		if (helperGetGrid()[x,y] != null)				return false;
		return move(x, y,true);
	}

	public void registerOnGrid(){
		helperGetGrid()	[(int)pos.x, (int)pos.y] = this;}
	public void unRegisterOnGrid(){
		helperGetGrid()[(int)pos.x, (int)pos.y] = null;}
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
			isMoved = isMoved || resultTry;
			return resultTry;
		}
		if (IsDebug) Debug.Log(typeMe + " " + "AVAILALBE");
		helperSetPosition(x, y);
		isMoved = true;
		return true;
	}
	public bool moveAttack(Vector2 dir, bool willMove = true,bool isFirstHit = false) 
	{ return moveAttack((int)(pos.x + dir.x), (int)(pos.y + dir.y), willMove,isFirstHit); }
	
	public virtual bool moveAttack(int x, int y, bool willMove = true,bool isFirstHit = false)
	{
		//if first hit, kill first! but otherwise, give the other one chance to runaway
		if (!isIndexValid(x, y) || (x == (int)pos.x && y ==(int)pos.y)) return false;
		var u = helperGetGrid()[x, y];
		if (u == null) return move(x, y);
		if (!helperIsValidAttackTarget(u.typeMe)) return false;

		if (!isFirstHit && !u.isUpdated)
		{
			u.KUpdate();
			u = helperGetGrid()[x, y];
			if (u == null) return move(x, y);
		}

		if (!attack(u, helperGetUnit((int)(x - pos.x)), helperGetUnit((int)(y - pos.y))))
			return false;
		if (willMove) return move(x, y);
		return true;
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
			return attacked(dirX, dirY);
		}
		return true;
	}
	public virtual bool attacked(int dirX, int dirY)
	{	
		if (!isDestroyable_simpleAttack) return false;
		if (--health <= 0)
		{
			kill(dirX, dirY);
			return true;
		}
		return false;
	}
	public virtual bool attackedBomb(int dirX, int dirY)
	{

		if (!isDestroyable_bomb) return false;
		if (--health <= 0)
		{
			EVENT_EXPLOSION((int)pos.x,(int)pos.y);
			kill(dirX, dirY);
			return true;
		}
		return false;
	}
	public virtual void kill(int dirX, int dirY)
	{
		unRegisterOnGrid();
		isAlive = false;
		if (isBomb) explode(dirX ,dirY);
	}
	void helperExplode(int x, int y)
	{
		if(!isIndexValid(x, y)) return;
		var unit = WorldInfo.gridUnits[x, y];
		if (unit != null) unit.attackedBomb((int)(unit.pos.x - pos.x), (int)(unit.pos.y - pos.y));
		else EVENT_EXPLOSION(x, y);
	}
	public virtual void explode(int dirX, int dirY)
	{
		Debug.Log("explode " + dirX + " " + dirY);
		helperExplode((int)pos.x, (int)pos.y);
		helperExplode((int)(pos.x+dirX), (int)(pos.y+dirY));
		helperExplode((int)(pos.x+dirY), (int)(pos.y+dirX));
		helperExplode((int)(pos.x-dirY), (int)(pos.y-dirX));
	}
	internal bool swap(UnitBase u)
	{
		if (!u.isSwappable) return false;
		var dummy = u.pos;
		u.posBefore = u.pos;
		u.pos = this.pos;
		this.posBefore = pos;
		this.pos = dummy;

		registerOnGrid();
		u.registerOnGrid();
		u.isMoved = true;
		WorldInfo.unitsAnimation00.Add(u);
		return true;
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
	public virtual void OnDestroy()
	{

	}
	public virtual void Destroy()
	{
		GameObject.Destroy(this.gameObject);
	}
	internal void UpdateAnimation()
	{
		var ani = GetComponent<RendererSprite>();
		ani.initAnimation(pos.x,pos.y, dirFacing);
	}

}