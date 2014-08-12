using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Editor :UIOrganizer
{

	static float ICN_WITH = .061f, ICN_HEIGHT = .1f;
	KDels.EVENTHDR_REQUEST_SIMPLE 
		EVENT_CHOSEN_TRAP,
		EVENT_CHOSEN_NORMAL;
	public UIItem screen;

	public List<UIItem_Selector> 
		bttn_envrionment,
		bttn_players,
		bttn_setting, 
		bttn_enemy,
		bttn_traps;
	public static KDels.EVENTHDR_REQUEST_SIMPLE EVENT_SCREEN_CLICK = delegate { };

	internal bool isRemove = true; //just remove everything

	internal KEnums.UNIT typeUnit= KEnums.UNIT.ENEMY;
	internal int unitCount = 0; 
	internal UnitBase.ATTACK_TYPE typeAttack = UnitBase.ATTACK_TYPE.NONE;
	internal int? dirFacing = 0;
	internal bool 
		isBomb = false, 
		isSpawn = false,
		isTrap = false;

	internal bool?
		destroyed_bomb = false,
		destroyed_simpleAttack = false;


	void Awake()
	{
		myItems.AddRange(bttn_players.Select(s => (UIItem)s));
		myItems.AddRange(bttn_setting.Select(s => (UIItem)s));
		myItems.AddRange(bttn_enemy.Select(s => (UIItem)s));
		myItems.AddRange(bttn_envrionment.Select(s => (UIItem)s));
		myItems.AddRange(bttn_traps.Select(s => (UIItem)s));

		UIItem_Selector.HELPER_CREATE_LINK(bttn_players);
		UIItem_Selector.HELPER_CREATE_LINK(bttn_enemy);
		UIItem_Selector.HELPER_CREATE_LINK(bttn_envrionment);
		screen.EVENT_CLICK += delegate { EVENT_SCREEN_CLICK(); };

		//enemies
		foreach (var b in bttn_enemy) { 
			b.EVENT_CLICK += off_traps; 
			b.EVENT_CLICK += off_players; 
			b.EVENT_CLICK += off_env;
			b.EVENT_CLICK += click_enemy;
		}
		//settings
		bttn_setting[0].EVENT_CLICK += disable_players;
		bttn_setting[0].EVENT_CLICK += off_env;
		bttn_setting[1].EVENT_CLICK += delegate { if (bttn_envrionment[0].isSelected) { isRemove = true; bttn_envrionment[0].setSelected(false); } };
		bttn_setting[2].EVENT_CLICK += disable_players;
		bttn_setting[2].EVENT_CLICK += off_env;
		foreach (var b in bttn_setting)
		{
			b.EVENT_CLICK += click_setting;
		}
		//traps
		foreach (var b in bttn_traps) {
			b.EVENT_CLICK += off_enemies;
			b.EVENT_CLICK += off_players;
			b.EVENT_CLICK += off_env;
			b.EVENT_CLICK += click_traps;
			//process_enemyTraps
		}
		//players
		bttn_players[0].EVENT_CLICK += off_settings;
		foreach (var b in bttn_players){
			b.EVENT_CLICK += off_traps;
			b.EVENT_CLICK += off_enemies;
			b.EVENT_CLICK += off_env;
			b.EVENT_CLICK += delegate { bttn_setting[0].setSelected(false); bttn_setting[2].setSelected(false); };
			b.EVENT_CLICK += click_players;
		}
		//envs
		bttn_envrionment[0].EVENT_CLICK += off_settings;
		bttn_envrionment[1].EVENT_CLICK += delegate { bttn_setting[0].setSelected(false); bttn_setting[2].setSelected(false); };
		foreach (var b in bttn_envrionment){
			b.EVENT_CLICK += off_traps;
			b.EVENT_CLICK += off_enemies;
			b.EVENT_CLICK += off_players;
			b.EVENT_CLICK += click_env;
		}

	}
	int helperGetSelected(List<UIItem_Selector> l)
	{
		for (int i = 0; i < l.Count; i++) if (l[i].isSelected) return i;
		return -1;
	}
	void click_env()
	{
		int n = helperGetSelected(bttn_envrionment);
		if (n == -1) {isRemove = true; return;}

		typeUnit = KEnums.UNIT.ENVIRONMENT;
		unitCount = n + 1;

		isRemove = false;
		isSpawn = false;
		destroyed_bomb = null;
		destroyed_simpleAttack = null;
		dirFacing = null;
		typeAttack = UnitBase.ATTACK_TYPE.KILL;
		click_setting();

	}
	void click_setting()
	{
		isBomb = bttn_setting[1].isSelected;//bombBttn
		isSpawn = bttn_setting[2].isSelected;
		if (bttn_setting[0].isSelected) //push attack
		{
			typeAttack = UnitBase.ATTACK_TYPE.PUSH;
			destroyed_simpleAttack = false;
		}
		else
		{
			typeAttack = UnitBase.ATTACK_TYPE.KILL;
			destroyed_simpleAttack = true;
		}
	}
	void process_dirFacing()
	{
		if (typeUnit == KEnums.UNIT.ENEMY && (unitCount == 3 || unitCount == 4))
		{
			dirFacing = null;
			return;
		}
	}
	void click_enemy()
	{
		int enemySelected = helperGetSelected(bttn_enemy);
		if (enemySelected == -1) { isRemove = true; return; }
		typeUnit = KEnums.UNIT.ENEMY;
		isRemove = false;
		isTrap = false;
		destroyed_bomb = true; // because all enemies can be destroyed 

		unitCount = enemySelected;
		click_setting();
		process_dirFacing();
	}
	Dictionary<string[], int> dic = new Dictionary<string[], int>()
		{
			{new string[]{"1000","0100","0010","0001"}, 5},
			{new string[]{"1100","0110","0011","1001"}, 6},
			{new string[]{"1010","0101"}, 7},
			{new string[]{"1110","0111","1011","1101"}, 8},
			{new string[]{"1111"}, 9}
		};
	void click_traps()
	{
		if (helperGetSelected(bttn_traps) == -1) { isRemove = true; return; }
		typeUnit = KEnums.UNIT.ENEMY;
		isRemove = false;
		isTrap = true;
		destroyed_bomb = true; // because all enemies can be destroyed 

		string content = "";
		for (int i = 0; i < 4; i++) content += (bttn_traps[i].isSelected) ? "1" : "0";

		foreach(var item in dic){
			var matches = item.Key;
			bool isBreak = false;
			for (int i = 0; i < matches.Length; i++)
			{
				if (matches[i] == content)
				{
					dirFacing = i;
					unitCount = item.Value;
					isBreak = true;
					break;
				}
			}
			if (isBreak) break;
		}
	}
	void disable_players()
	{
		if (typeUnit == KEnums.UNIT.PLAYER) isRemove = true;
		off_players();
	}
	void click_players()
	{
		int n = helperGetSelected(bttn_players);
		if (n == -1) { isRemove = true; }
		typeUnit = KEnums.UNIT.PLAYER;
		isRemove = false;
		isTrap = false;
		unitCount = n;
		destroyed_bomb = null;
		destroyed_simpleAttack = null;

		click_setting();

	}
	
	void off_traps()
	{
		if (typeUnit == KEnums.UNIT.ENEMY && isTrap) isRemove = true;
		foreach (var b in bttn_traps) b.setSelected(false);
	}

	void off_enemies()
	{
		if (typeUnit == KEnums.UNIT.ENEMY) isRemove = true;
		foreach (var b in bttn_enemy) b.setSelected(false);
	}
	void off_players()
	{
		if (typeUnit == KEnums.UNIT.PLAYER) isRemove = true;
		foreach (var b in bttn_players) b.setSelected(false);
	}

	void off_env()
	{
		if (typeUnit == KEnums.UNIT.ENVIRONMENT) isRemove = true;
		foreach (var b in bttn_envrionment) b.setSelected(false);
	}

	void off_settings()
	{
		foreach (var b in bttn_setting) b.setSelected(false);
	}

	void helperPosition(List<UIItem_Selector> icns, Vector2 corner, float width, float height, int count = 6)
	{
		for (float i = 0; i < icns.Count; i++)
		{
			Vector2 incremenets = new Vector2(i % count, (float)(int)(i / count));

			icns[(int)i].position(new Vector4(
				corner.x + width * incremenets.x, corner.y - height * incremenets.y,
				corner.x + width * (incremenets.x + 1), corner.y - height * (incremenets.y + 1)));
		}
	}
	public override void positionReset()
	{
		base.positionReset();
		screen.position(0.0f, 1.0f, 1.0f, 0.0f);
		screen.transform.position = new Vector3(screen.transform.position.x, screen.transform.position.y, screen.transform.position.z + 1);

		Vector2 corner = new Vector2(.5f, .5f);
		helperPosition(bttn_envrionment, new Vector2(.0f, .6f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_players, new Vector2(.0f, .5f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_setting, new Vector2(.0f, .4f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_enemy, new Vector2(.0f, .3f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_traps, new Vector2(.0f, .2f), ICN_WITH, ICN_HEIGHT);

	}
}
