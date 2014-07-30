using UnityEngine;
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
	public GameSetting(OS osChosen = OS.DESKTOP)
	{
		myOS = osChosen;
	}
	public void initGame(KLevel level)
	{
		if (isInitiated) destoryPreviousGame();
		isInitiated = true;

		//add 2 for "edges" 
		WorldInfo.init(level.WIDTH+2, level.HEIGHT+2); 
		var kBoard = initBoard(level.WIDTH + 2, level.HEIGHT +2);
		var loop = new GameObject("	GameSystem : GameLoop", typeof(GameLoop)).GetComponent<GameLoop>();
		
		//Instantiate units
		foreach (var u in level.units) initUnits(u);
		var spawn = helperInstantiate(Dir_GameObjects.dicUnits[KEnums.UNIT.ENEMY][10], 1, 1).GetComponent<UnitEnemy_Spawn>();
		spawn.setSpawnEnemy(Dir_GameObjects.dicUnits[KEnums.UNIT.ENEMY][5],2,isTrap:true);
		spawn.init();

		kBoard.initCorners();
		loop.init(kBoard);

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
		foreach (var o in objectsInitiated) MonoBehaviour.Destroy(o);
		foreach (var o in WorldInfo.units) MonoBehaviour.Destroy(o.gameObject);
		foreach (var o in WorldInfo.unitsStatic) MonoBehaviour.Destroy(o.gameObject);

		objectsInitiated = new List<GameObject>();
		WorldInfo.init((int)WorldInfo.WORLD_SIZE.x, (int)WorldInfo.WORLD_SIZE.y);
	}
	Board initBoard(int w, int h)
	{
		var kBoard = (MonoBehaviour.Instantiate(Dir_GameObjects.BOARD, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Board>();
		kBoard.transform.localScale = new Vector3(w,h, 1);
		kBoard.reset(w,h);
		return kBoard;
	}
	private GameObject initUnits(KLevel_Unit u)
	{
		var PREFAB = Dir_GameObjects.dicUnits[u.type00][u.type01];
		return helperInstantiate(PREFAB, u.position[0], u.position[1]);
	}
	GameObject helperInstantiate(GameObject PREFAB, int x, int y)
	{
		GameObject obj = MonoBehaviour.Instantiate(PREFAB, Vector3.zero, Quaternion.identity) as GameObject;
		
		var uBase = obj.GetComponent<UnitBase>();
		uBase.pos = new Vector2(x,y);
		uBase.init();
		uBase.transform.position = uBase.pos.XYZ();
		return obj;
	}

}
