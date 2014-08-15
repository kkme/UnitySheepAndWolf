using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EditorLoop : MonoBehaviour
{
	UI_Editor ui;
	void Awake()
	{
		UI_Editor.EVENT_SCREEN_CLICK += EVENTHDR_SPAWN;
	}
	void EVENTHDR_SPAWN()
	{
		var pos = helperGetMouseAt();
		int x = (int)pos.x, y = (int)pos.y;
		if (x < 0 || y < 0 || x > 12 || y >12) return ;
		if (x == 0 || y == 0 || x == 12 || y == 12)
		{
			click_edge(x, y);
			return;
		}
		click_inside(x, y);

		
	}
	Vector2 helperGetMouseAt()
	{
		return WorldInfo.camGame.ScreenToWorldPoint(Input.mousePosition) + new Vector3(.5f, .5f, 0);
	}
	void event_moveCamera()
	{
		var pos = helperGetMouseAt();
		WorldInfo.camGame.transform.position = new Vector3(pos.x, pos.y,WorldInfo.camGame.transform.position.z);
	}
	void click_edge(int x, int y)
	{
		for (var door = WorldInfo.door; door != null;door = null )
		{
			door.Destroy();
			var wall = (Instantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.ENVIRONMENT][1]) as GameObject).GetComponent<UnitBase>()
				.initPos((int)door.pos.x, (int)door.pos.y).init();
			
		}
		WorldInfo.gridUnits[x, y].Destroy();
		var doorNew = (Instantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.ENVIRONMENT][0]) as GameObject).GetComponent<UnitBase>();
		doorNew.initPos(x, y).init();

	}
	
	void click_inside(int x, int y)
	{
		var uBefore = WorldInfo.gridUnits[x, y];
		if (uBefore != null) uBefore.Destroy();
		if (ui.isRemove) return;
		if (ui.typeUnit == KEnums.UNIT.PLAYER && ui.unitCount == 0)
		{
			if (WorldInfo.unitPlayer_real != null) WorldInfo.unitPlayer_real.Destroy();
		}
		//Debug.Log("ISBOMB " + ui.isBomb + " TYPE OF ATTACK " + ui.typeAttack + " IS SPAWN " +
		//	ui.isSpawn + " IS DES BY SIMPLE ATTACK " + ui.isDestroyable_simpleAttack);
		var unit = spawn(ui.typeUnit, ui.unitCount, ui.dirFacing, ui.isBomb, ui.typeAttack, ui.isDestroyable_simpleAttack, ui.destroyed_bomb,
			ui.isSpawn, ui.isTrap).GetComponent<UnitBase>();
		
		unit.initPos(x, y).init();
	}
	GameObject helperSpawnNormal(KEnums.UNIT typeUnit, int count, int? dir, bool isBomb, UnitBase.TYPE_ATTACK typeAttack, bool? isDestroyable_simpleAttack, bool? isDestroyable_bomb)
	{
		var obj = Instantiate(Dir_GameObjects.dicUnits[typeUnit][count]) as GameObject;
		var unit = obj.GetComponent<UnitBase>();
		if(dir!=null)unit.dirFacing = dir.Value;
		unit.typeAttack = typeAttack;
		unit.isBomb = isBomb;
		if(isDestroyable_bomb != null)
			unit.isDestroyable_bomb = isDestroyable_bomb.Value;
		if (isDestroyable_simpleAttack != null) 
			unit.isDestroyable_simpleAttack = isDestroyable_simpleAttack.Value;

		return obj;
	}
	GameObject helperSpawn_Spawnner(KEnums.UNIT typeUnit, int count, 
		int? dir, bool isBomb, UnitBase.TYPE_ATTACK typeAttack, 
		bool? isDestroyable_simpleAttack, bool? isDestroyable_bomb,bool isTrap)
	{
		var obj = Instantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.ENEMY][10]) as GameObject;
		var unit = obj.GetComponent<UnitEnemy_Spawn>();
		var unitSpawnned = Dir_GameObjects.dicUnits[typeUnit][count];

		int dirFacing = (dir == null) ? unitSpawnned.GetComponent<UnitBase>().dirFacing : dir.Value;
		unit.setSpawnEnemy(unitSpawnned, dirFacing, isBomb, typeAttack,
			isDestroyable_simpleAttack.Value,isDestroyable_bomb.Value,isTrap);
		return obj;
	}
	GameObject spawn(KEnums.UNIT typeUnit, int count, int? dir, bool isBomb,UnitBase.TYPE_ATTACK typeAttack, bool? isDestroyable_simpleAttack	, bool? isDestroyable_bomb,
		bool isSpawn = false,bool isTrap =false)
	{
		if (isSpawn) return helperSpawn_Spawnner(typeUnit, count, dir, isBomb, typeAttack, isDestroyable_simpleAttack, isDestroyable_bomb, isTrap);
		return helperSpawnNormal(typeUnit, count, dir, isBomb, typeAttack, isDestroyable_simpleAttack, isDestroyable_bomb);
		
	}
	public void init(UI_Editor ui){
		this.ui = ui;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			
			event_moveCamera();
		}
	}
}
