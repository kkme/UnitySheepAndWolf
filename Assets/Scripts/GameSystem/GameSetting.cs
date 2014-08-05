﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using ExtensionsUnityVectors;

public class GameSetting
{
	bool isInitiated = false;

	public enum OS { DESKTOP, ANDROID };
	OS myOS = OS.DESKTOP;
	List<GameObject> objectsInitiated = new List<GameObject>();
	GameObject units;
	public GameSetting(OS osChosen = OS.DESKTOP)
	{
		myOS = osChosen;
	}
	public void initGame(KLevel level, bool isGameLoopActive = true)
	{
		if (isInitiated) destoryPreviousGame();
		isInitiated = true;
		units = new GameObject("	GameSetting : Units");

		//add 2 for "edges" 
		//WorldInfo.init(level.WIDTH + 2, level.HEIGHT + 2); 
		WorldInfo.init(13,13);
		var kBoard = initBoard(13, 13);
		var loop = new GameObject("	GameSetting : GameLoop", typeof(GameLoop)).GetComponent<GameLoop>();
		loop.enabled = isGameLoopActive;

		//Instantiate units
		foreach (var u in level.units) initUnits(u, units.transform);
		
		//var spawn = helperInstantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.ENEMY][10], 1, 1).GetComponent<UnitEnemy_Spawn>();
		//spawn.setSpawnEnemy(Dir_GameObjects.dicUnits[KEnums.UNIT.ENEMY][5], 0, UnitBase.ATTACK_TYPE.PUSH, false, isTrap: true);
		//
		//helperInstantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.PLAYER][1], 2, 3).GetComponent<UnitBase>();
		//helperInstantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.PLAYER][2], 2, 5).GetComponent<UnitBase>();
		//helperInstantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.PLAYER][2], 2, 4).GetComponent<UnitBase>();


		loop.init();

		objectsInitiated.Add(kBoard.gameObject);
		objectsInitiated.Add(loop.gameObject);
		
		switch (myOS)
		{
			case OS.DESKTOP:
				var os = new GameObject("GameSystem : OS_DeskTop", typeof(OS_DeskTOp));
				os.GetComponent<OS_DeskTOp>().gameLoop = loop;
				objectsInitiated.Add(os);
				break;
			case OS.ANDROID:
				break;
		}
	}

	void destoryPreviousGame()
	{
		Debug.Log("GameSetting : Destroyed Previous Game");
		foreach (var o in objectsInitiated) if (o != null) MonoBehaviour.Destroy(o);
		foreach (var o in WorldInfo.unitsUpdate01) if (o != null) MonoBehaviour.Destroy(o.gameObject);
		foreach (var o in WorldInfo.unitsUpdate00) if (o != null) MonoBehaviour.Destroy(o.gameObject);
		foreach (var o in WorldInfo.unitsStatic) if(o != null) MonoBehaviour.Destroy(o.gameObject);

		objectsInitiated = new List<GameObject>();
		WorldInfo.init((int)WorldInfo.WORLD_SIZE.x, (int)WorldInfo.WORLD_SIZE.y);
	}
	Board initBoard(int w, int h)
	{
		var kBoard = (MonoBehaviour.Instantiate(Dir_GameObjects.BOARD, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Board>();
		kBoard.transform.localScale = new Vector3(w,h, 1);
		kBoard.init(w, h);
		return kBoard;
	}
	private GameObject initUnits(KLevel_Unit u, Transform parent = null)
	{
		var PREFAB = Dir_GameObjects.dicUnits[u.type00][u.type01];
		return helperInstantiate(PREFAB, u.position[0], u.position[1], parent);
	}
	GameObject helperInstantiate(GameObject PREFAB, int x, int y, Transform parent = null)
	{
		GameObject obj = MonoBehaviour.Instantiate(PREFAB, Vector3.zero, Quaternion.identity) as GameObject;
		
		var uBase = obj.GetComponent<UnitBase>();
		uBase.pos = new Vector2(x,y);
		uBase.init();
		uBase.transform.position = uBase.pos.XYZ();
		uBase.transform.parent = parent;
		return obj;
	}

}
