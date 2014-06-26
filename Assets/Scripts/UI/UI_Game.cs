using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Game : UIOrganizer
{
	public UIItem bttn_option, bttn_back;
	public void Awake()
	{
		myItems.Add(bttn_option);
		myItems.Add(bttn_back);
	}

	public override void positionReset()
	{
		bttn_option.position(new Vector4(.8f, 1, 1, .8f));
		bttn_back.position(new Vector4(.0f, 1, .2f, .8f));
	}

}