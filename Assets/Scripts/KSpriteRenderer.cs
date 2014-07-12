using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class KSpriteRenderer : MonoBehaviour
{
	static Vector2 OFFSET = new Vector2(-.05f, .05f);
	public Sprite mySprite;
	public Color colorMain,colorShadow;
	GameObject spriteA, spriteB;
	AniMover aniA, aniB;
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
	}
	void Start()
	{
		
	}
	public void move(float x, float y)
	{
		this.GetComponent<AniMover>().move(x, y);
	}
	public void rotate(int n)
	{
		aniA.rotate(n);
		aniB.rotate(n);
	}

	public void initAnimation(float x, float y, int dir)
	{
		//var u = GetComponent<UnitBase>();
		move(x+ .5f, y + .5f);
		rotate(dir);
	}
	
}
