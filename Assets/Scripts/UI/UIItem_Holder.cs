using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIItem_Holder : UIItem
{
	enum STATE {DetermineHeld, BeingHeld };
	STATE stateMe = STATE.DetermineHeld;
	public SpriteRenderer renderer;
	public Sprite spriteIdl, spriteSelected;
	bool isSelected = false;
	float timeElapsed, timeElapsedMaxDetermine = .2f, timeElapsedMax = .2f;
	public override void Awake()
	{
		base.Awake();
		enabled = false;
	}
	public override void OnMouseDown()
	{
		base.OnMouseDown();
		renderer.sprite = spriteSelected;
		enabled = true;
		stateMe = STATE.DetermineHeld;
		timeElapsed = 0;
		timeElapsedMax = .3f;
	}
	public override void show()
	{
		base.show();
		renderer.sprite = spriteIdl;
	}
	bool isStillOnMe()
	{
		var mousePos =  Camera.main.ScreenToWorldPoint( Input.mousePosition);
		return Physics2D.Raycast(mousePos, Vector2.zero).collider == this.collider2D ;
	}
	void determineWillBeHeld(){
		if (timeElapsed > timeElapsedMaxDetermine)
		{
			timeElapsed = 0;
			stateMe=  STATE.BeingHeld;
			return;
		}
	}
	void processBeingHeld()
	{
		if (timeElapsed < timeElapsedMax) return;
		timeElapsedMax = Mathf.Max(.1f, timeElapsedMax * .68f);
		timeElapsed = 0;
		EVENT_CLICK();//raise an event
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		//check conditions in which makes it "not being held"
		if (Input.GetMouseButton(0) && isStillOnMe())
		{
			switch (stateMe)
			{
				case STATE.DetermineHeld:
					determineWillBeHeld(); break;
				case STATE.BeingHeld:
					processBeingHeld(); break;
			}
			return;
		}
		renderer.sprite = spriteIdl;
		enabled = false;
	
	}
}
