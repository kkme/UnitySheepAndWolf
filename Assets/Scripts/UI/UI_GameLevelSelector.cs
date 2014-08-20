using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UI_GameLevelSelector : UIOrganizer
{
	static public KDels.EVENTHDR_REQUEST_SIMPLE_INT EVENT_LEVEL_SELECTED;
	public UIItem text,
		bttn_back, bttn_forward;
	int levelSelected = 0;
	float timeElapsed=0, timeElapsedMax = 1.0f;
	public override void Awake()
	{
		base.Awake();
		myItems.Add(bttn_forward);
		myItems.Add(bttn_back);
		myItems.Add(text);
		bttn_back.EVENT_CLICK += delegate { AudioManager.Play_Button00(); CLICK(-1); };
		bttn_forward.EVENT_CLICK += delegate { AudioManager.Play_Button01(); CLICK(+1); };
	}
	public override void show()
	{
		enabled = true;
		timeElapsed = 0;
		levelSelected =  WorldInfo.level;
		text.TextMesh.text = "" + levelSelected;
		base.show();
	}
	public override void hide()
	{
		enabled = false;
		base.hide();
	}
	public override void positionReset()
	{
		base.positionReset();
		float height = .08f,
			widthRatio = 1/ WorldInfo.camGame.camera.aspect,
			width = height * widthRatio,
			textHeight = .3f,
			textWidth = textHeight * widthRatio;

		bttn_forward.position(new Vector4(1 - width, height, 1, .0f));
		bttn_back.position(new Vector4(.0f, height, width, .0f));
		text.position(new Vector4(.5f - textWidth, .5f+ textHeight, .5f + textWidth, .5f-textHeight));
		//text.TextMesh.text = ""+WorldInfo.id;
	}
	public void CLICK(int n)
	{
		timeElapsed = 0;
		levelSelected = Mathf.Max(0, n + levelSelected);

		text.TextMesh.text = "" + levelSelected;
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed > timeElapsedMax)
		{
			enabled = false;
			EVENT_LEVEL_SELECTED(levelSelected);
		}
	}
}
