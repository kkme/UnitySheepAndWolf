using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameSetting : MonoBehaviour
{
	enum OS { DESKTOP, ANDROID };
	static OS myOS = OS.DESKTOP;
	static bool isInitiated = false;
	static List<GameObject> objectsInitiated = new List<GameObject>();

	public static void initGame(KLevel level)
	{
		if (isInitiated) destoryPreviousGame();
		isInitiated = true;

		initWorldInfo(level.WIDTH, level.HEIGHT);
		var kBoard = initBoard(level.WIDTH, level.HEIGHT);
		initUnits(level.units);

		var loop = new GameObject("GameSystem : GameLoop", typeof(GameLoop)).GetComponent<GameLoop>();
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
	static void destoryPreviousGame()
	{
		Debug.Log("GameSetting : Destroyed Previous Game");
		foreach (var o in objectsInitiated) Destroy(o);
		objectsInitiated = new List<GameObject>();
	}
	static Board initBoard(int w, int h)
	{
		var kBoard = (Instantiate(KLevelComponents.me.board, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Board>();
		kBoard.transform.localScale = new Vector3(w,h, 1);
		kBoard.reset(w,h);
		return kBoard;
	}
	static void initUnits(List<Vector4> units)
	{

		Dictionary<int, IEnumerable<GameObject>> dic = new Dictionary<int, IEnumerable<GameObject>>(){
		{(int)KEnums.UNIT.PLAYER ,new GameObject[]{KLevelComponents.me.UNIT_PLAYER }},
		{(int)KEnums.UNIT.ENEMY  ,KLevelComponents.DicUntiEnemy} };
		foreach (var data in units)
		{
			var PREFAB = dic[(int)data.x].ElementAt((int)data.y);
			var u =( Instantiate(PREFAB, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<UnitBase>();
			u.pos = new Vector2(data.z, data.w);

		}
	}
	static void initWorldInfo(int width, int height)
	{
		WorldInfo.worldSize = new Vector2(width,height);
		WorldInfo.gridUnits = new UnitBase[width,height];
		WorldInfo.gridHazard = new HazardBase[width, height];
		WorldInfo.gridUnitsDisregard = new KEnums.UNIT?[width, height];
	}
}
