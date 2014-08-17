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

			spawn.turnCount = node["turnCount"].AsInt;
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

	internal RendererSprite rSprite;
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
	public virtual void Awake() { rSprite = GetComponent<RendererSprite>(); }
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
	public int helperToDirFromRaw(Vector2 v) { return helperToDir (helperGetUnit((int)v.x),helperGetUnit((int)v.y) ) ;}
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

	public virtual bool processLocation(int x, int y)
	{
		return false;
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
	public virtual void UpdateAnimation()
	{
		var ani = GetComponent<RendererSprite>();
		//ani.initAnimation(pos.x,pos.y, dirFacing);
		rSprite.move(pos.x, pos.y);
		rSprite.rotate(dirFacing);
	}
	bool helperIsClearPathHere(int xNew, int yNew)
	{
		if (!isIndexValid(xNew, yNew)) return false;
		var unit = WorldInfo.gridUnits[xNew, yNew];
		return
			unit == null ||
			(unit.typeMe != KEnums.UNIT.ENVIRONMENT
			//&&!(unit.typeMe == KEnums.UNIT.ENEMY && (unit.id == 0 || (unit.id > 4 && unit.id < 10)))
			);
	}
	void helperMarkMap(ref bool[,] map, int x, int y, bool b)
	{
		if (!isIndexValid(x, y)) return;
		map[x, y] = b;
	}
	protected List<int[]> getOptimalRoute(int x, int y, int xTo, int yTo, int dirStart = 0)
	{
		List<int[]> l = new List<int[]>();
		int scoreMin = 99999999;
		for (int i = 0; i < 4; i++)
		{
			int dir = (dirStart + i) % 4;
			int[] xy = new int[4] { x + dirPath[dir][0], y + dirPath[dir][1], dir, 0 };
			xy[3] = helperGetScore(xy[0], xy[1], xTo, yTo);
			if (!isIndexValid(xy[0], xy[1])) continue;
			if (xy[3] < scoreMin)
			{
				scoreMin = xy[3];
				l.Insert(0, xy);
			}
			else if (xy[3] == scoreMin)
			{

				l.Insert(1, xy);
			}
			else l.Add(xy);
		}
		return l;
	}

	public int findPathToUnit(int destX, int destY)
	{
		int unitX = (int)pos.x, unitY = (int)pos.y;

		if ((Mathf.Abs(unitX - destX) == 1 && Mathf.Abs(unitY - destY) == 0) ||
			(Mathf.Abs(unitX - destX) == 0 && Mathf.Abs(unitY - destY) == 1))
		{
			closestTileX = destX; closestTileY = destY;
			return 1;
		}

		bool[,] map = new bool[13, 13];
		var routes = getOptimalRoute(unitX, unitY, destX, destY, dirFacing);
		foreach (var r in routes) map[r[0], r[1]] = true;
		foreach (var r in routes)
		{
			if (!helperIsClearPathHere(r[0], r[1]) ) continue;
			int cost = recursivePath(ref map, r[0], r[1], destX, destY, r[2]);
			if (cost == -1) continue;
			closestTileX = r[0];
			closestTileY = r[1];
			return cost;
		}
		return -1;
	}
	protected int helperGetScore(int x, int y, int destX, int destY)
	{
		//int disX = Mathf.Abs(x - (int)WorldInfo.unitPlayer_real.pos.x), disY = Mathf.Abs(y - (int)WorldInfo.unitPlayer_real.pos.y);
		int s = Mathf.Abs(x - destX) +
				Mathf.Abs(y - destY);
		if (s == 0) return s;
		var unit = WorldInfo.gridUnits[x, y];
		if (unit != null)
		{
			//if(isTrap(unit.typeMe,unit.id))s+=2;
			if (unit.typeMe == KEnums.UNIT.ENVIRONMENT) s += 100;
		}
		return s;
	}
	protected Dictionary<int, int[]> dirPath = new Dictionary<int, int[]>(){
		{0, new int[2]{0,1}},{1, new int[2]{1,0}},{2, new int[2]{0,-1}},{3, new int[2]{-1,0}}
	};
	protected int closestTileX = 0, closestTileY = 0;

	int recursivePath(ref bool[,] map, int x, int y, int xTo, int yTo, int dirBefore)
	{
		map[x, y] = true;
		List<int> dirsAvailable = new List<int>();
		var routes = getOptimalRoute(x, y, xTo, yTo, dirBefore);
		foreach (var r in routes)
		{
			if (map[r[0], r[1]] || !helperIsClearPathHere(r[0], r[1])) continue;
			if (r[3] == 0)
			{ return r[3]+1; } // found it
			var cost = recursivePath(ref map, r[0], r[1], xTo, yTo, r[2]);
			if (cost != -1) return 1 + r[3];
		}
		return -1;
	}

}