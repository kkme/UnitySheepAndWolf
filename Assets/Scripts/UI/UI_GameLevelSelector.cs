using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UI_GameLevelSelector : UIOrganizer
{
	static public KDels.EVENTHDR_REQUEST_SIMPLE_INT
		EVENT_REQUEST_BTTN_CLICK = delegate { },
		EVENT_LEVEL_SELECTED = delegate { };
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
		EVENT_REQUEST_BTTN_CLICK += delegate(int n)
		{
			if (n == -1) bttn_back.OnMouseDown();
			if (n == 1) bttn_forward.OnMouseDown();
		};
	}
	void helperShowLevel(int level) {
		text.TextMesh.text = "" + levelSelected;
		if (WorldInfo.levelStates[level])
		{
			text.TextMesh.color = new Color(.52f, .52f, .52f);
		}
		else
		{

			text.TextMesh.color = new Color(1.0f, .32f, .32f);
		}
	}
	public override void show()
	{
		enabled = true;
		timeElapsed = 0;
		levelSelected = WorldInfo.level;
		helperShowLevel(levelSelected);
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
		levelSelected =Mathf.Min(59, Mathf.Max(0, n + levelSelected));
		helperShowLevel(levelSelected);
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed > timeElapsedMax)
		{
			enabled = false;
			bttn_back.gameObject.SetActive(false);
			bttn_forward.gameObject.SetActive(false);
			EVENT_LEVEL_SELECTED(levelSelected);
		}
	}
}
