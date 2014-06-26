using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_GameFinished : UIOrganizer
{
	public UIItem bttn_restart, bttn_next;
	public static KDels.EVENTHDR_REQUEST_SIMPLE 
		EVENT_REQUEST_RESTART_LEVEL = delegate { },
		EVENT_REQUEST_NEXT_LEVEL = delegate { };

	void Awake()
	{
		myItems.Add(bttn_restart);
		myItems.Add(bttn_next);
		bttn_restart.EVENT_CLICK += EVENT_REQUEST_RESTART_LEVEL;
		bttn_next.EVENT_CLICK += EVENT_REQUEST_NEXT_LEVEL;
	}
	public override void positionReset()
	{
		bttn_restart.position(new Vector4(0, 1, .5f, 0));
		bttn_next.position(new Vector4(.5f,1, 1.0f, 0.0f));
	}
}
