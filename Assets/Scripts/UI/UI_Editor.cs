using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Editor :UIOrganizer
{
	static float ICN_WITH = .1f, ICN_HEIGHT = .1f;
	public List<UIItem_Selector> bttn_players,bttn_setting, bttn_enemy;
	void Awake()
	{
		myItems.AddRange(bttn_players.Select(s => (UIItem)s));
		myItems.AddRange(bttn_setting.Select(s => (UIItem)s));
		myItems.AddRange(bttn_enemy.Select(s => (UIItem)s));
		UIItem_Selector.HELPER_CREATE_LINK(bttn_players);
		UIItem_Selector.HELPER_CREATE_LINK(bttn_enemy);
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
		Vector2 corner = new Vector2(.5f, .5f);
		helperPosition(bttn_players,	new Vector2(.0f, .5f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_setting, new Vector2(.0f, .4f), ICN_WITH, ICN_HEIGHT);
		helperPosition(bttn_enemy,		new Vector2(.0f, .3f), ICN_WITH, ICN_HEIGHT);
			
	}
}
