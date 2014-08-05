using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Editor :UIOrganizer
{
	static float ICN_WITH = .1f, ICN_HEIGHT = .1f;
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
	void Awake()
	{
		myItems.AddRange(bttn_players.Select(s => (UIItem)s));
		myItems.AddRange(bttn_setting.Select(s => (UIItem)s));
		myItems.AddRange(bttn_enemy.Select(s => (UIItem)s));
		UIItem_Selector.HELPER_CREATE_LINK(bttn_players);
		UIItem_Selector.HELPER_CREATE_LINK(bttn_enemy);

		foreach (var b in bttn_enemy) { 
			b.EVENT_CLICK += off_traps; 
			b.EVENT_CLICK += off_players; 
			b.EVENT_CLICK += off_env; 
		}
		foreach (var b in bttn_traps) {
			b.EVENT_CLICK += off_units; 
			b.EVENT_CLICK += off_players; 
			b.EVENT_CLICK += off_env; 
		}
		foreach (var b in bttn_players){
			b.EVENT_CLICK += off_traps;
			b.EVENT_CLICK += off_units;
			b.EVENT_CLICK += off_env;
		}
		foreach (var b in bttn_envrionment){
			b.EVENT_CLICK += off_traps;
			b.EVENT_CLICK += off_units;
			b.EVENT_CLICK += off_players;
		}
	}
	void helperPosition(List<UIItem_Selector> icns, Vector2 corner, float width, float height,int count= 6)
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
		screen.transform.position = new Vector3(screen.transform.position.x, screen.transform.position.y, screen.transform.position.z+1);

		Vector2 corner = new Vector2(.5f, .5f);
		helperPosition(bttn_envrionment, new Vector2(.0f, .6f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_players, new Vector2(.0f, .5f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_setting, new Vector2(.0f, .4f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_enemy, new Vector2(.0f, .3f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_traps, new Vector2(.0f, .2f), ICN_WITH, ICN_HEIGHT);
			
	}
	void off_traps()
	{
		foreach (var b in bttn_traps) b.setSelected(false);
	}

	void off_units()
	{
		foreach (var b in bttn_enemy) b.setSelected(false);
	}
	void off_players()
	{
		foreach (var b in bttn_players) b.setSelected(false);
	}

	void off_env()
	{
		foreach (var b in bttn_envrionment) b.setSelected(false);
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) Debug.Log("MOUSEDOWN");
	}
}
