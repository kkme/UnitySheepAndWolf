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
		keyMove,
		keyUp,keyDown,keyLeft,keyRight,
		bttn_back,bttn_forward;
	bool isPlayerTryingToMove = false;
	public override void Awake()
	{
		base.Awake();
		myItems.Add(keyUp);
		myItems.Add(keyDown);
		myItems.Add(keyLeft);
		myItems.Add(keyRight);
		myItems.Add(bttn_forward);
		myItems.Add(bttn_back);
		myItems.Add(keyMove);

		keyMove.EVENT_CLICK += moveInitiated;
		bttn_forward.EVENT_CLICK += delegate { AudioManager.Play_Button01(); EVENT_REQUEST_MAP_CHANGE(1); };
		bttn_back.EVENT_CLICK += delegate { AudioManager.Play_Button00(); EVENT_REQUEST_MAP_CHANGE(-1); };

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
	void moveInitiated()
	{
		center = Input.mousePosition;
		isPlayerTryingToMove = true;
	}
	void helperSetSize(Transform  t, float width, float height)
	{
		var p = t.parent;
		width *= Camera.main.aspect;
		t.parent = null;
		t.localScale = new Vector3(width, height, 1);
		t.parent = p;

	}
	public override void positionReset()
	{
		float	height = .08f,
				widthRatio = 1/ WorldInfo.camGame.camera.aspect,
				width = height * widthRatio,
				_Height = .05f,
				_width = .05f;

		bttn_forward.position(new Vector4(1 - width, height, 1, .0f));
		bttn_back.position(new Vector4(.0f, height, width, .0f));
		// ratio : (3vs4) (.4 , .3)
		helperSetSize(keyUp.transform, _Height, _width);
		helperSetSize(keyDown.transform, _Height, _width);
		helperSetSize(keyLeft.transform, _width, _Height);
		helperSetSize(keyRight.transform, _width, _Height);
		helperSetSize(keyMove.transform, .9f, .9f);

		keyMove.position(new Vector4(.05f, .95f, .95f,.05f));
		keyLeft.position(new Vector4(0, .5f + _Height * .5f, _width, .5f - _Height * .5f));
		keyRight.position(new Vector4(1 - _width, .5f + _Height * .5f, 1, .5f - _Height * .5f));
		//
		keyUp.position(new Vector4(.5f - _Height * .5f, 1, .5f + _Height * .5f, 1 - _width));
		keyDown.position(new Vector4(.5f - _Height * .5f, _width, .5f + _Height * .5f, 0));
	}
	Vector3 center = new Vector3();
	void Update()
	{
		if (isPlayerTryingToMove && Input.GetMouseButtonUp(0))
		{
			var pos = Input.mousePosition - center;
			if (Mathf.Abs(pos.y) >Mathf.Abs( pos.x ) )
			{
				if (pos.y > 0) EVENT_INPUT_PLAYER(0);
				else EVENT_INPUT_PLAYER(2);
			}
			else
			{

				if (pos.x > 0) EVENT_INPUT_PLAYER(1);
				else EVENT_INPUT_PLAYER(3);

			}
			isPlayerTryingToMove = false;
			//Debug.Log(Input.mousePosition- new Vector3(Screen.width*.5f,Screen.height*.5f,0) );
			
		}
	}

}