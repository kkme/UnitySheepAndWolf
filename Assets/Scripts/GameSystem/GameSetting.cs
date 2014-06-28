using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

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

		initWorldInfo(level.WIDTH, level.HEIGHT);
		var kBoard = initBoard(level.WIDTH, level.HEIGHT);
		foreach (var u in level.units) initUnits(u);
		//initUnits(level.units);

		var loop = new GameObject("	GameSystem : GameLoop", typeof(GameLoop)).GetComponent<GameLoop>();
		Debug.Log("SETTING ASSIGNED BOARD :  " + kBoard);
		loop.init(kBoard);
		switch (myOS)
		{
			case OS.DESKTOP:
				new GameObject("GameSystem : OS_DeskTop", typeof(OS_DeskTOp)).GetComponent<OS_DeskTOp>()
					.gameLoop = loop;
				break;
			case OS.ANDROID:
				break;
		}
	}

	void destoryPreviousGame()
	{
		Debug.Log("GameSetting : Destroyed Previous Game");
		foreach (var o in objectsInitiated) MonoBehaviour.Destroy(o);
		objectsInitiated = new List<GameObject>();
	}
	Board initBoard(int w, int h)
	{
		var kBoard = (MonoBehaviour.Instantiate(KLevelComponents.BOARD, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Board>();
		kBoard.transform.localScale = new Vector3(w,h, 1);
		kBoard.reset(w,h);
		return kBoard;
	}

	private void initUnits(KLevel_Unit u)
	{
		var PREFAB = KLevelComponents.dicUnits[u.type00][u.type01];
		GameObject obj = MonoBehaviour.Instantiate(PREFAB, Vector3.zero, Quaternion.identity) as GameObject;
		obj.GetComponent<UnitBase>().pos = new Vector2(u.position[0], u.position[1]);
	}

	static void initWorldInfo(int width, int height)
	{
		WorldInfo.worldSize = new Vector2(width,height);
		WorldInfo.gridUnits = new UnitBase[width,height];
		WorldInfo.gridHazard = new HazardBase[width, height];
		WorldInfo.gridUnitsDisregard = new KEnums.UNIT?[width, height];
	}
}
