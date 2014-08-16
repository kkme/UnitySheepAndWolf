using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UI_Game : UIOrganizer
{
	static public KDels.EVENTHDR_REQUEST_SIMPLE_INT
		EVENT_REQUEST_MAP_CHANGE = delegate { },
		EVENT_INPUT_PLAYER = delegate { };
	public UIItem 
		keyUp,keyDown,keyLeft,keyRight,
		bttn_back,bttn_forward;
	public override void Awake()
	{
		base.Awake();
		myItems.Add(keyUp);
		myItems.Add(keyDown);
		myItems.Add(keyLeft);
		myItems.Add(keyRight);
		myItems.Add(bttn_forward);
		myItems.Add(bttn_back);
		bttn_forward.EVENT_CLICK += delegate { EVENT_REQUEST_MAP_CHANGE(1); };
		bttn_back.EVENT_CLICK += delegate { EVENT_REQUEST_MAP_CHANGE(-1); };

		//keyUp.rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		//keyDown.rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		//keyRight.rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		//keyLeft.rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

		keyUp.EVENT_CLICK +=	delegate { move(0); };
		keyRight.EVENT_CLICK += delegate { move(1); };
		keyDown.EVENT_CLICK +=	delegate { move(2); };
		keyLeft.EVENT_CLICK +=	delegate { move(3); };
	}
	void move(int dir)
	{
		Debug.Log("dir " + dir);
		EVENT_INPUT_PLAYER(dir);
	}
	void helperSetSize(Transform  t, float width, float height)
	{
		var p = t.parent;
		width *= WorldInfo.camGame.camera.aspect;
		t.parent = null;
		t.localScale = new Vector3(width, height, 1);
		t.parent = p;

	}
	public override void positionReset()
	{
		float	height = .08f,
				widthRatio = 1/ WorldInfo.camGame.camera.aspect,
				width = height * widthRatio,
				_Height = .45f,
				_width = .45f;

		bttn_forward.position(new Vector4(1 - width, height, 1, .0f));
		bttn_back.position(new Vector4(.0f, height, width, .0f));
		// ratio : (3vs4) (.4 , .3)
		helperSetSize(keyUp.transform, _Height, _width);
		helperSetSize(keyDown.transform, _Height, _width);
		helperSetSize(keyLeft.transform, _width, _Height);
		helperSetSize(keyRight.transform, _width, _Height);

		keyLeft.position(new Vector4(0, .5f + _Height * .5f, _width, .5f - _Height * .5f));
		keyRight.position(new Vector4(1 - _width, .5f + _Height * .5f, 1, .5f - _Height * .5f));
		//
		keyUp.position(new Vector4(.5f - _Height * .5f, 1, .5f + _Height * .5f, 1 - _width));
		keyDown.position(new Vector4(.5f - _Height * .5f, _width, .5f + _Height * .5f, 0));
	}

}