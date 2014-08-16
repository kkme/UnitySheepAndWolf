using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class RendererSprite : MonoBehaviour
{
	static Vector2 OFFSET = new Vector2(-.03f, .03f);
	public Sprite mySprite;
	public Color colorMain,colorShadow;
	GameObject spriteA, spriteB;
	public AniMover aniA, aniB; //for some reason setting them public works

	GameObject helperInstantiate(string name, Sprite s, Color c ,Vector3 offset)
	{
		GameObject obj = new GameObject(name, new System.Type[]{ typeof(SpriteRenderer) ,typeof(AniMover) });
		obj.transform.parent = transform;
		obj.transform.position = transform.position + offset;
		obj.layer = gameObject.layer;
		
		var r =obj.GetComponent<SpriteRenderer>();
		r.sprite = s;
		r.color = c;
		return obj;
	}
	public void Awake()
	{
		spriteA = helperInstantiate("Sprite_Main", mySprite, colorMain, new Vector3(OFFSET.x, OFFSET.y, 0));
		spriteB = helperInstantiate("Sprite_Shadow", mySprite, colorShadow, new Vector3(OFFSET.x * -1, OFFSET.y * -1, +1));
		aniA = spriteA.GetComponent<AniMover>();
		aniB = spriteB.GetComponent<AniMover>();
		gameObject.AddComponent<AniMover>();
		
		var unit = gameObject.GetComponent<UnitBase>();
		if(unit == null) return;
		int angle = (360 - 90 * unit.dirFacing);
		spriteA.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		spriteB.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}
	public void move(float x, float y)
	{
		gameObject.GetComponent<AniMover>().move(x, y);
	}
	public void rotate(int n)
	{
		aniA.rotate(n);
		aniB.rotate(n);
	}

	public void initAnimation(float x, float y, int dir)
	{
		//var u = GetComponent<UnitBase>();
		move(x, y);
		rotate(dir);
	}
	public bool isAnimating()
	{
		return GetComponent<AniMover>().enabled || aniA.enabled || aniB.enabled;
	}
	
}
