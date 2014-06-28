using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Menu : UIOrganizer
{
	public static KDels.EVENTHDR_REQUEST_SIMPLE EVENT_REQUEST_GAME_START = delegate { };

	public UIItem ICN_title;
	public UIItem	BTTN_start,
						BTTN_exit;

	
	void Awake()
	{

		BTTN_start.EVENT_CLICK += EVENT_REQUEST_GAME_START;
		myItems.AddRange(new UIItem[] { ICN_title, BTTN_start, BTTN_exit });
	}
	public override void positionReset()
	{
		ICN_title.position(new Vector4(0, 1.0f, 1.0f, .8f));
		BTTN_start.position(new Vector4(0, .6f, 1.0f, 0.4f));
		BTTN_exit.position(new Vector4(0, .3f, 1.0f, 0.1f));
	}
}